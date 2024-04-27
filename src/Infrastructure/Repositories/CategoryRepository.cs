using Domain.Categories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    internal sealed class CategoryRepository(ApplicationDbContext dbContext) 
        : Repository<Category>(dbContext), ICategoryRepository
    {
    }
}
