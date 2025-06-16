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
            CreateMap<ToDoItemRequestDto, ToDoItems>()
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.CompletedAt, opt => opt.Ignore());
        }
    }
}
