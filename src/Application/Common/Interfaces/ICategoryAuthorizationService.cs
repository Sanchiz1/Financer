namespace Application.Common.Interfaces;

public interface ICategoryAuthorizationService
{
    Task<bool> IsCategoryAccessible(string userId, Guid categoryId, CancellationToken cancellationToken = default);
}