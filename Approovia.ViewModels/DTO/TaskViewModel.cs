using System;
using System.ComponentModel.DataAnnotations;

namespace Approovia.ViewModels.DTO
{
    public class TaskViewModel
    {
        public Guid TaskId { get; set; }

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
                return StartDate.AddDays(ElapsedTime).Date;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return StartDate.AddDays(AllottedTime).Date;
            }
        }

        public int DaysOverdue
        {
            get
            {
                if (TaskStatus == false) // TaskStatus is PENDING
                {
                    return Math.Abs(ElapsedTime - AllottedTime); 
                   
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
                    return Math.Abs(AllottedTime - ElapsedTime);
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
