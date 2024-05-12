using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Errors;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Categories;
public sealed record DeleteCategoryCommand(string UserId, Guid CategoryId) : ICommand<Unit>;

internal sealed class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICategoryAuthorizationService categoryAuthorizationService)
    : ICommandHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ICategoryAuthorizationService _categoryAuthorizationService = categoryAuthorizationService;

    public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        bool isAccessible = await this._categoryAuthorizationService.IsCategoryAccessible(
           request.UserId,
           request.CategoryId,
           cancellationToken);

        if (!isAccessible)
        {
            return Result.Failure<Unit>(CategoryErrors.InvalidUser);
        }

        var category = await this._categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Unit>(CategoryErrors.NotFound);
        }

        await this._categoryRepository.Delete(category, cancellationToken);

        return Result.Success(Unit.Value);
    }
}