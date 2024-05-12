using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Errors;
using SharedKernel.Result;

namespace Application.UseCases.Categories;
public sealed record GetCategoryByIdQuery(string UserId, Guid CategoryId) : IQuery<TransactionCategoryDto>;

internal sealed class GetCategoryByIdQueryHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper,
    ICategoryAuthorizationService categoryAuthorizationService)
    : IQueryHandler<GetCategoryByIdQuery, TransactionCategoryDto>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ICategoryAuthorizationService _categoryAuthorizationService = categoryAuthorizationService;

    public async Task<Result<TransactionCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        bool isAccessible = await this._categoryAuthorizationService.IsCategoryAccessible(
            request.UserId,
            request.CategoryId,
            cancellationToken);

        if (!isAccessible)
        {
            return Result.Failure<TransactionCategoryDto>(CategoryErrors.InvalidUser);
        }

        var category = await this._categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<TransactionCategoryDto>(CategoryErrors.NotFound);
        }

        return this._mapper.Map<TransactionCategoryDto>(category);
    }
}
