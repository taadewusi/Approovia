using Approovia.ViewModels.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approovia.ViewModels.ModelParameters
{
    public class TaskListParameters: RequestParameters
    {
        public TaskListParameters()
        {
            OrderBy = "CreateDate";
        }

        public string SearchTerm { get; set; }
       
        public string Name { get; set; }
    }
}
