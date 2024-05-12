namespace Application.Common.Interfaces;
public interface ITransactionAuthorizationService
{
    Task<bool> IsTransactionAccessible(string userId, Guid transactionId, CancellationToken cancellationToken = default);
}
