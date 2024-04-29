using Domain.Entities.TransactionAggregate;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Reports;

public class Report
{
    private Report(
        IEnumerable<Transaction> transactions,
        DateRange dateRange,
        Currency preferredCurrency,
        ICategoryRepository categoryRepository)
    {
        this.Transactions = transactions.ToList();
        this.DateRange = dateRange;
        this.PreferredCurrency = preferredCurrency;
        this._categoryRepository = categoryRepository;
    }

    private readonly ICategoryRepository _categoryRepository;

    public List<Transaction> Transactions { get; }

    public DateRange DateRange { get; }

    public Currency PreferredCurrency { get; }

    public Money TotalIncome { get; private set; }

    public Money TotalExpense { get; private set; }

    public Dictionary<Guid, decimal> ExpensePercentages { get; private set; }

    public Dictionary<Guid, decimal> IncomePercentages { get; private set; }

    public static async Task<Report> Create(
        IEnumerable<Transaction> transactions,
        DateRange dateRange,
        Currency preferredCurrency,
        ICategoryRepository categoryRepository)
    {
        var report = new Report(transactions, dateRange, preferredCurrency, categoryRepository);

        await Task.WhenAll(
            report.CalculateTotals(),
            report.CalculateCategoryPercentages());

        return report;
    }

    private async Task CalculateTotals()
    {
        Money totalIncome = Money.Zero(this.PreferredCurrency);
        Money totalExpense = Money.Zero(this.PreferredCurrency);

        foreach (var transaction in this.Transactions)
        {
            var isIncomeTask = IsIncomeTransaction(transaction);
            var isExpenseTask = IsExpenseTransaction(transaction);

            await Task.WhenAll(isIncomeTask, isExpenseTask);

            var isIncome = isIncomeTask.Result;
            var isExpense = isExpenseTask.Result;

            if (isIncome.Value)
            {
                totalIncome += transaction.Amount;
            }

            if (isExpense.Value)
            {
                totalExpense += transaction.Amount;
            }
        }

        this.TotalIncome = totalIncome;
        this.TotalExpense = totalExpense;
    }

    private async Task CalculateCategoryPercentages()
    {
        this.ExpensePercentages = await CalculateCategoryPercentagesByType(OperationType.Expense);
        this.IncomePercentages = await CalculateCategoryPercentagesByType(OperationType.Income);
    }

    private async Task<Dictionary<Guid, decimal>> CalculateCategoryPercentagesByType(OperationType type)
    {
        var transactions = await Task.WhenAll(this.Transactions.Select(async t =>
        {
            if ((await GetOperationType(t)).Value == type)
            {
                return t;
            }
            return null;
        }));

        var filteredTransactions = transactions.Where(t => t != null).ToList();

        var groupedTransactions = filteredTransactions
            .GroupBy(t => t!.CategoryId)
            .ToDictionary(g => g.Key, g => g.Sum(t => t!.Amount.Amount));

        var percentages = new Dictionary<Guid, decimal>();
        decimal totalAmount = type == OperationType.Expense
            ? this.TotalExpense.Amount
            : this.TotalIncome.Amount;

        foreach (var group in groupedTransactions)
        {
            var categoryId = group.Key;
            var totalInCategory = group.Value;
            percentages[categoryId] = (totalInCategory / totalAmount) * 100;
        }

        return percentages;
    }

    private async Task<Result<bool>> IsIncomeTransaction(Transaction transaction) =>
        (await this.GetOperationType(transaction)).Value == OperationType.Income;

    private async Task<Result<bool>> IsExpenseTransaction(Transaction transaction) =>
        !(await this.IsIncomeTransaction(transaction)).Value;

    private async Task<Result<OperationType>> GetOperationType(Transaction transaction)
    {
        var category = await this._categoryRepository.GetByIdAsync(transaction.CategoryId);

        if (category is null)
        {
            return Result.Failure<OperationType>(CategoryErrors.NotFound);
        }

        return category.OperationType;
    }
}