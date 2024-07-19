using JobAllocation.JobAllocationContext.Domain.Shared.Services;
using JobAllocation.JobAllocationContext.Infrastructure;
using MongoDB.Driver;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;

public sealed class PoliciesRepository : IPoliciesRepository
{
    private readonly IMongoCollection<IPolicy> _collection;
    private readonly CacheService _cacheService;

    public PoliciesRepository(MongoDbContext mongoDbContext, CacheService cacheService)
    {
        _collection = mongoDbContext.GetCollection<IPolicy>("Policies");
        _cacheService = cacheService; // TODO: implement decorator pattern and use "Scrutor" package
    }

    public async Task<IEnumerable<IPolicy>> FindAll(CancellationToken cancellationToken)
        => await (await _collection.FindAsync(_ => true, null, cancellationToken)).ToListAsync(cancellationToken);
}