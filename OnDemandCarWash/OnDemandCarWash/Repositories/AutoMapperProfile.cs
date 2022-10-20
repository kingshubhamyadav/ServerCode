using AutoMapper;
using OnDemandCarWash.Dtos;
using OnDemandCarWash.Models;

namespace OnDemandCarWash.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.User, Dtos.WasherProfileDto>();
            CreateMap<WasherProfileDto, User>();
        }
    }
}
