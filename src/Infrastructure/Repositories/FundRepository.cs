using Domain.Funds;
using Domain.Currencies;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    internal sealed class FundRepository(ApplicationDbContext dbContext)
        : Repository<Fund>(dbContext), IFundRepository
    {
    }
}