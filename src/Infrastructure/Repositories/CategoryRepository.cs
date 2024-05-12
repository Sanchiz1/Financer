using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Entities.TransactionAggregate;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
internal sealed class CategoryRepository(ApplicationDbContext dbContext)
    : Repository<TransactionCategory>(dbContext), ICategoryRepository
{
}