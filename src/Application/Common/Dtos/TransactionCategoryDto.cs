namespace Application.Common.Dtos;
public sealed record TransactionCategoryDto(
    Guid Id,
    string Name,
    string Description,
    int OperationType);