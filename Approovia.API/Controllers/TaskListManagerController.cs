using Approovia.Datatier.Repositories;
using Approovia.Models.DataFile;
using Approovia.ViewModels.Generics;
using AutoMapper;
//using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using Approovia.ViewModels.ModelParameters;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Approovia.ViewModels.DTO;
using System.Collections.Generic;
using System.Linq;

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



        [HttpGet, Route("get-all")]
        [AllowAnonymous]
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
                        Data = pagedList,
                        Status = Constants.SuccessCode,
                        Message = tasksExist
                    });


                    //var taskReModels = _mapper.Map<PagedList<TaskViewModel>>(taskList);

                    //return Ok(new Response<PagedList<TaskViewModel>>
                    //{
                    //    MetaData = taskList.MetaData,
                    //    Data = taskReModels,
                    //    Status = Constants.SuccessCode,
                    //    Message = tasksExist
                    //});
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
                _logger.LogError($"ApprooviaAPI: - {MethodBase.GetCurrentMethod()?.Name} - {ex.Message}");

                return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }

        }


        [HttpGet, Route("get-by-id/{id}")]
        [AllowAnonymous]
        public IActionResult GetById(Guid id)
        {
            var task = _taskLists.GetById(id);
            if (task == null)
                return Ok(new { message = "Task not available!" });
            return Ok(task);
        }

        [HttpPost, Route("add-task")]
        public IActionResult AddMovie([FromBody] TaskDTO model)
        {
            try
            {

                var newTask = new TaskList()
                {
                    TaskId = Guid.NewGuid(),
                    TaskName = model.TaskName,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    AllottedTime = model.AllottedTime,
                    ElapsedTime = model.AllottedTime,
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
                _logger.LogError($"{MethodBase.GetCurrentMethod().Name} - {ex.Message}");
                return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }
        }

        [HttpPut, Route("update-Task")]
        public IActionResult UpdateMovie([FromBody] TaskDTO model)
        {
            model.UpdateDate = DateTime.Now;
            var entity = _mapper.Map<TaskList>(model);
            _taskLists.Update(entity);
            _taskLists.Save();


            return Ok(_taskLists.GetById(model.TaskId));
        }


        [HttpDelete, Route("delete-task/{id}")]
        public IActionResult DeleteMovie(Guid id)
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
