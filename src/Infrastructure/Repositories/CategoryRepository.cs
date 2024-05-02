using Domain.AggregatesModel.TransactionAggregate;
using Domain.Entities.TransactionAggregate;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class CategoryRepository(ApplicationDbContext dbContext) 
    : Repository<TransactionCategory>(dbContext), ICategoryRepository
{
}
