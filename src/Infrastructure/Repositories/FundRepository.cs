using Infrastructure.Data;
using Domain.Entities.FundAggregate;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

internal sealed class FundRepository(ApplicationDbContext dbContext)
    : Repository<Fund>(dbContext), IFundRepository
{
}