using Application.Common.Interfaces;
using Domain.AggregatesModel.TransactionAggregate.Repositories;

namespace Infrastructure.Common;
internal sealed class CategoryAuthorizationService(ICategoryRepository categoryRepository) : ICategoryAuthorizationService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    public async Task<bool> IsCategoryAccessible(string userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await this._categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        return category?.UserId.ToString() == userId;
    }
}