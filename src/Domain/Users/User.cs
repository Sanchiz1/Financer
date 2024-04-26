using Domain.Shared;
using Domain.Abstractions;

namespace Domain.Users
{
    public sealed class User : Entity
    {
        public Guid PreferredCurrencyId { get; private set; }
        public Name FirstName { get; private set; }
        public Name LastName { get; private set; }
        public Email Email { get; private set; }

        public User(Name firstName, Name lastName, Email email)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
    }
}
