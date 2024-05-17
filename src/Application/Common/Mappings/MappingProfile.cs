using Application.Common.Dtos;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate;

namespace Application.Common.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ConstructUsing(src => new TransactionDto(
                src.CategoryId,
                src.Amount.Amount,
                src.Amount.Currency.Code,
                src.Description.Value,
                src.OperationDate.ToString()
            ));

        CreateMap<TransactionCategory, TransactionCategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => (int)src.OperationType));
    }
}