using AutoMapper;
using MyBookListAPI.Dto;
using MyBookListAPI.Models;

namespace MyBookListAPI.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ApplicationUser, User>().ReverseMap();
        }
    }
}
