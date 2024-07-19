using MongoDB.Driver;

namespace JobAllocation.JobAllocationContext.Infrastructure;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase Database;

    public MongoDbContext(IMongoDatabase database)
    {
        Database = database;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
        => Database.GetCollection<T>(collectionName);

}