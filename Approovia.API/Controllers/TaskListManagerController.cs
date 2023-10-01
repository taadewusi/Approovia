using Approovia.Datatier.Repositories;
using Approovia.Models.DataFile;
using Approovia.ViewModels.Generics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using System.Linq;
using Approovia.ViewModels.ModelParameters;
using Newtonsoft.Json;
using Approovia.ViewModels.DTO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Approovia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListManagerController : BaseController
    {
        private readonly IRepository<TaskList> _taskLists;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public TaskListManagerController(IRepository<TaskList> taskLists, IMapper mapper, ILoggerManager logger)
        {
            _taskLists = taskLists;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all the Task Lists
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>PagedList of the Tasks</returns>
        [HttpGet, Route("get-all")]

        public IActionResult GetAll([FromQuery] TaskListParameters parameters)
        {
            try
            {
                var tasksExist = "Task(s) Retrieved Successfully";
                var taskDoesNotExist = "No Task Found";

                var taskList = _taskLists.GetAll(parameters);

                if (taskList.MetaData.TotalCount > 0)
                {

                    var taskModels = _mapper.Map<IEnumerable<TaskViewModel>>(taskList);

                    var pagedList = new PagedList<TaskViewModel>(taskModels.ToList(), taskList.MetaData.TotalCount, taskList.MetaData.CurrentPage, taskList.MetaData.PageSize);

                    return Ok(new Response<PagedList<TaskViewModel>>
                    {
                        MetaData = pagedList.MetaData,
                        Data = pagedList,
                        Status = Constants.SuccessCode,
                        Message = tasksExist
                    });

                }


                return Ok(new Response<PagedList<TaskList>>
                {
                    MetaData = taskList.MetaData,
                    Data = taskList,
                    Status = Constants.NoData,
                    Message = taskDoesNotExist
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Approovia.API Get All: - {MethodBase.GetCurrentMethod()?.Name} - {ex.Message}");

                return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }

        }

        /// <summary>
        /// Get a task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A model of the Task</returns>
        [HttpGet, Route("get-by-id/{id}")]

        public IActionResult GetById(Guid id)
        {
            var task = _taskLists.GetById(id);
            if (task == null)
                return Ok(new { message = "Task not available!" });
            var taskModel = _mapper.Map<TaskViewModel>(task);
            return Ok(taskModel);
        }

        /// <summary>
        /// Add a new Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add-task")]
        public IActionResult AddTask([FromBody] TaskDTO model)
        {
            try
            {
                // Check if a task with the same properties already exists in the database
                var existingTask = _taskLists.GetAll().Where(task =>
                    task.TaskName == model.TaskName &&
                    task.Description == model.Description &&
                    task.StartDate == model.StartDate &&
                    task.AllottedTime == model.AllottedTime &&
                    task.ElapsedTime == model.ElapsedTime).FirstOrDefault();

                if (existingTask != null)
                {
                    // Return a conflict response indicating that a similar task already exists
                    return CustomResult(System.Net.HttpStatusCode.Conflict, Constants.ConflictCode, "A similar task already exists.");
                }



                //Create a new task and save
                var newTask = new TaskList()
                {
                    TaskId = Guid.NewGuid(),
                    TaskName = model.TaskName,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    AllottedTime = model.AllottedTime,
                    ElapsedTime = model.ElapsedTime,
                    TaskStatus = model.TaskStatus,
                    IsActive = true,
                    CreateDate = DateTime.Now,
                };
                _taskLists.Add(newTask);
                _taskLists.Save();
                _logger.LogInformation($"New Task Created - {JsonConvert.SerializeObject(newTask)}");

                return Ok(new Response<TaskList>
                {
                    Message = "Task added successfully",
                    Status = Constants.SuccessCode,
                    Data = newTask

                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Approovia.API Add Task: - {MethodBase.GetCurrentMethod()?.Name} - {ex.Message}");

                return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }
        }

        /// <summary>
        /// Update a Task
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Updated model of the task</returns>
        [HttpPut, Route("update-Task")]
        public IActionResult UpdateTask([FromBody] UpdateTaskDTO model)
        {
            try
            {
                model.UpdateDate = DateTime.Now;
                var entity = _mapper.Map<TaskList>(model);
                _taskLists.Update(entity);
                _taskLists.Save();
                var task = _taskLists.GetById(model.TaskId);
                var taskModel = _mapper.Map<TaskViewModel>(task);
                return Ok(taskModel);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Approovia.API Update Task: - {MethodBase.GetCurrentMethod()?.Name} - {ex.Message}");

                return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }
        }

        /// <summary>
        /// Delete a Task using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> A message if it was successful or not</returns>
        [HttpDelete, Route("delete-task/{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = _taskLists.GetById(id);
            if (task == null)
                return Ok(new { message = "Task not available!" });

            _taskLists.Delete(task.TaskId);
            _taskLists.Save();
            return Ok(new { message = "Task deleted successfully!" });


        }


    }
}
