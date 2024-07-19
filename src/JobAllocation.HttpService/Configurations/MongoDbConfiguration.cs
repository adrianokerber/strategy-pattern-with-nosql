namespace JobAllocation.HttpService.Configurations;

public record MongoDbConfiguration(
    string DatabaseName,
    string ConnectionString
);