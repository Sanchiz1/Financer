using Application.Common.Dtos;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate;

namespace Application.Common.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
           .ForMember(dest => dest.MoneyAmount, opt => opt.MapFrom(src => src.Amount.Amount))
           .ForMember(dest => dest.MoneyCurrency, opt => opt.MapFrom(src => src.Amount.Currency.Code))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
           .ForMember(dest => dest.OperationDate, opt => opt.MapFrom(src => src.OperationDate));

        CreateMap<TransactionCategory, TransactionCategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => (int)src.OperationType));
    }
}