using Domain.AggregatesModel.TransactionAggregate;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
internal sealed class CategoryRepository(ApplicationDbContext dbContext)
    : Repository<TransactionCategory>(dbContext), ICategoryRepository
{
}