using Domain.Entities.TransactionAggregate;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class CategoryRepository(ApplicationDbContext dbContext) 
    : Repository<TransactionCategory>(dbContext), ICategoryRepository
{
}
