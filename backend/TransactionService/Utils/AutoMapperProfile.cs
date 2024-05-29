using AutoMapper;
using TransactionService.Dtos;
using TransactionService.Models;

namespace TransactionService.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Transaction, TransactionDto>();
            CreateMap<CreateTransactionDto, Transaction>();
            CreateMap<TransactionWithAccountsDto, TransactionDto>()
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts));
        }
    }
}
