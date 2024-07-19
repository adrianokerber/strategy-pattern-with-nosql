using CSharpFunctionalExtensions;
using JobAllocation.JobAllocationContext.Domain.Shared.Services;
using JobAllocation.JobAllocationContext.Infrastructure;
using MongoDB.Driver;

namespace JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;

public sealed class CompaniesRepository : ICompaniesRepository
{
    private readonly IMongoCollection<Company> _collection;
    private readonly CacheService _cacheService;

    public CompaniesRepository(MongoDbContext mongoDbContext, CacheService cacheService)
    {
        _collection = mongoDbContext.GetCollection<Company>("Companies");
        _cacheService = cacheService;
    }

    public async Task<Maybe<Company>> FindById(string id, CancellationToken cancellationToken)
    {
        var companies = await _cacheService.GetCache("Companies_Key", FindAllFromDb);

        return companies.SingleOrDefault(c => c.Code == id);
    }

    private async Task<IEnumerable<Company>> FindAllFromDb()
        => await (await _collection.FindAsync(_ => true)).ToListAsync();
}