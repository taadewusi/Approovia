using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approovia.Datatier.Repositories
{
    public interface ILoggerManager
    {
        void LogInformation(string message);
        void LogError(string message);
    }
}
