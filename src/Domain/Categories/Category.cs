using Domain.Shared;
using Domain.Abstractions;

namespace Domain.Categories
{
    public class Category : Entity
    {
        public Guid UserId { get; set; }
        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public OperationType OperationType { get; private set; }

        private Category() { }

        public Category(Guid userId, Name name, Description description, OperationType operationType)
        {
            this.UserId = userId;
            this.Name = name;
            this.Description = description;
            this.OperationType = operationType;
        }
    }
}