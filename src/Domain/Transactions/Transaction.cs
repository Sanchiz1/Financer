using Domain.Funds;
using Domain.Shared;
using Domain.Categories;
using Domain.Abstractions;

namespace Domain.Transactions
{
    public class Transaction : Entity
    {
        public Guid FundId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Money Amount { get; private set; }
        public Description Description { get; private set; }
        public DateTime UtcNow { get; private set; }

        internal Transaction(
            Guid id,
            Guid fundId,
            Guid categoryId,
            Money amount,
            Description description,
            DateTime utcNow)
        {
            this.Id = id;
            this.FundId = fundId;
            this.CategoryId = categoryId;
            this.Amount = amount;
            this.Description = description;
            this.UtcNow = utcNow;
        }

        public static Result<Transaction> Create(
            Fund fund,
            Category category,
            Money amount,
            Description description,
            DateTime utcNow)
        {
            if (amount <= amount.Currency.MinPositiveValue)
            {
                return Result.Failure<Transaction>(TransactionErrors.InvalidAmount);
            }

            return new Transaction(
                Guid.NewGuid(),
                fund.Id,
                category.Id,
                amount,
                description,
                utcNow);
        }
    }
}