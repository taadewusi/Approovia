using System;
using System.ComponentModel.DataAnnotations;

namespace Approovia.ViewModels.DTO
{
    public class TaskViewModel
    {
        public Guid TaskId { get; set; }

        //public TaskViewModel()
        //{
        //    TaskId = Guid.NewGuid(); // Generate a new Guid for each instance
        //}
        //[Required(ErrorMessage = "Task name cannot be blank")]
        //public string TaskName { get; set; }

        //[Required(ErrorMessage = "Task description cannot be blank")]
        //public string Description { get; set; }

        //[Required(ErrorMessage = "Task start date cannot be blank")]
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public DateTime DueDate { get; set; }
        //public int AllottedTime { get; set; }
        //public int DaysOverdue { get; set; }
        //public int DaysLate { get; set; }
        //public int ElapsedTime { get; set; }
        //public bool TaskStatus { get; set; }
        //public bool IsActive { get; set; }


        [Required(ErrorMessage = "Task name cannot be blank")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Task description cannot be blank")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Task start date cannot be blank")]
        public DateTime StartDate { get; set; }

        public int AllottedTime { get; set; }

        public int ElapsedTime { get; set; }

        public bool TaskStatus { get; set; }

        public bool IsActive { get; set; }

        public DateTime EndDate
        {
            get
            {
                return StartDate.AddDays(ElapsedTime);
            }
        }

        public DateTime DueDate
        {
            get
            {
                return StartDate.AddDays(AllottedTime);
            }
        }

        public int DaysOverdue
        {
            get
            {
                if (TaskStatus == false) // TaskStatus is PENDING
                {
                    return ElapsedTime - AllottedTime;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int DaysLate
        {
            get
            {
                if (TaskStatus == true) // TaskStatus is CLOSED
                {
                    return AllottedTime - ElapsedTime;
                }
                else
                {
                    return 0;
                }
            }
        }   

    public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
