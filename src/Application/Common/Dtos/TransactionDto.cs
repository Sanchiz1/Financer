namespace Application.Common.Dtos;

public sealed class TransactionDto
{
    public TransactionDto() { }

    public TransactionDto(Guid categoryId, decimal moneyAmount, string moneyCurrency, string description, string operationDate)
    {
        this.Id = Guid.NewGuid();
        this.CategoryId = categoryId;
        this.MoneyAmount = moneyAmount;
        this.MoneyCurrency = moneyCurrency;
        this.Description = description;
        this.OperationDate = operationDate;
    }

    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public decimal MoneyAmount { get; set; }
    public string MoneyCurrency { get; set; }
    public string Description { get; set; }
    public string OperationDate { get; set; }
}