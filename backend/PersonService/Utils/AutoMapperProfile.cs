using AutoMapper;
using PersonService.Dtos;
using PersonService.Models;

namespace PersonService.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PersonWithAccountsDto, PersonDto>()
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts))
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));

            CreateMap<AccountDto, Account>();

            CreateMap<TransactionDto, Transaction>();

            CreateMap<CreatePersonDto, Person>();

            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts));

            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts));

            CreateMap<Account, AccountDto>();

            CreateMap<Transaction, TransactionDto>();
        }
    }
}
