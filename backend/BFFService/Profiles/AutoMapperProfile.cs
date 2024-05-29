using AutoMapper;
using BFFService.Dtos;
using BFFService.Models;

namespace BFFService.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<CreateClientDto, Person>();
            CreateMap<CreateAccountDto, Account>();
        }
    }
}
