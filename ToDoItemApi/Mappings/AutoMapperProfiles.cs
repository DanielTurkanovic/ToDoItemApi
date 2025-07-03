using AutoMapper;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;

namespace ToDoItemApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<ToDoItems, ToDoItemDto>().ReverseMap();
            CreateMap<CreateToDoItemRequestDto, ToDoItems>();
            CreateMap<UpdateToDoItemRequestDto, ToDoItems>();
        }
    }
}
