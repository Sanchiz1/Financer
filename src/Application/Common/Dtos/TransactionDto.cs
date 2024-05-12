namespace Application.Common.Dtos;

public sealed record TransactionDto(
    Guid Id,
    Guid CategoryId,
    decimal MoneyAmount,
    string MoneyCurrency,
    string Description,
    DateTime OperationDate);