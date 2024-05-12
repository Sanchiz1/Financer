using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using SharedKernel.Result;

namespace Application.UseCases.Categories;
public sealed record GetCategoriesQuery(string UserId) : IQuery<IReadOnlyList<TransactionCategoryDto>>;

internal sealed class GetGategoriesQueryHandler(
    ICategoryRepository categoryRepository, 
    IMapper mapper)
    : IQueryHandler<GetCategoriesQuery, IReadOnlyList<TransactionCategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IReadOnlyList<TransactionCategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await this._categoryRepository.GetAllAsync(cancellationToken);

        return this._mapper.Map<List<TransactionCategoryDto>>(categories);
    }
}