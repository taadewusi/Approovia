using Approovia.Models.DataFile;
using Approovia.ViewModels.DTO;
using AutoMapper;

namespace Approovia.ViewModels.Mappings
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<TaskList, TaskViewModel>().ReverseMap();
            CreateMap<TaskList, TaskDTO>().ReverseMap();
            CreateMap<TaskList, UpdateTaskDTO>().ReverseMap();
        }
    }
}
