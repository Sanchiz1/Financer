namespace Application.Common.Dtos;
public sealed record TransactionCategoryDto(
    Guid Id,
    Guid UserId,
    string Name,
    string Description,
    int OperationType);