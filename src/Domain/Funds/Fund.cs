using Domain.Shared;
using Domain.Currencies;
using Domain.Abstractions;

namespace Domain.Funds
{
    public sealed class Fund : Entity
    {

        public Guid UserId { get; private set; }
        public Currency Currency { get; private set; }
        public Name Name { get; private set; }
        public Description Description { get; private set; }

        private Fund() { }

        public Fund(Guid userId, Currency currency, Name name, Description description)
        {
            this.UserId = userId;
            this.Currency = currency;
            this.Name = name;
            this.Description = description;
        }
    }
}
