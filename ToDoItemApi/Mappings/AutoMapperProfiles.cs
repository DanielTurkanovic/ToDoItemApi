using AutoMapper;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;

namespace ToDoItemApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<ToDoItem, ToDoItemDto>().ReverseMap();
            CreateMap<CreateToDoItemRequestDto, ToDoItem>();
            CreateMap<UpdateToDoItemRequestDto, ToDoItem>();
            CreateMap<User, UserDto>();
        }
    }
}
