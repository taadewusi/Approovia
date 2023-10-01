using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approovia.ViewModels.DTO
{
    public class UpdateTaskDTO
    {
        public Guid TaskId { get; set; }

        [Required(ErrorMessage = "Task name cannot be blank")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Task description cannot be blank")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Task start date cannot be blank")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Task Allotted Time cannot be blank")]
        public int AllottedTime { get; set; }

        [Required(ErrorMessage = "Task Elapsed Time cannot be blank")]
        public int ElapsedTime { get; set; }
        public bool TaskStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
