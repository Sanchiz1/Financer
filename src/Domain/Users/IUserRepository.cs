namespace Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetById(Guid id, CancellationToken cancellationToken = default);

        void Add(User user);
    }
}
