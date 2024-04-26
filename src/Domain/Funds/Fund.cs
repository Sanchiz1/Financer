using Domain.Abstractions;

namespace Domain.Funds
{
    public sealed class Fund : Entity
    {
        public Guid UserId { get; private set; }
        public Guid CurrencyId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
