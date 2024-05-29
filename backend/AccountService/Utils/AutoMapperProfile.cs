using AutoMapper;
using AccountService.Dtos;
using AccountService.Models;

namespace AccountService.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<CreateAccountDto, Account>();
            CreateMap<DeleteAccountDto, Account>();
            CreateMap<DebitCreditAccountDto, Transaction>();
        }
    }
}
