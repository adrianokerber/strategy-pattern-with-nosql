using Asp.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Serilog;
using Serilog.Exceptions;
using JobAllocation.Shared;
using JobAllocation.HttpService.Configurations;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Companies;
using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using JobAllocation.JobAllocationContext.Infrastructure;

namespace JobAllocation.HttpService.StartupInfra;

internal static class ServicesExtensions
{
    public static IServiceCollection AddLogs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithExceptionDetails()
            .CreateLogger();

        // Ensure the logger is used by the .NET Core logging subsystem
        services.AddLogging(loggingBuilder =>
            loggingBuilder.ClearProviders().AddSerilog(dispose: true));

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }
    
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddAuthentication();
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddOpenApiSpecs(this IServiceCollection services)
    {
        services.AddOpenApiDocument();
        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(
            o =>
                o.AddPolicy(
                    "default",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    }
                )
        );

        return services;
    }

    public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck("self-check", () => HealthCheckResult.Healthy(), new[] { "ready" })
            .AddMongoDb(mongodbConnectionString: GetMongoDbConfiguration(configuration).ConnectionString);
        return services;
    }

    public static IServiceCollection AddSingleDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbConfiguration = GetMongoDbConfiguration(configuration);

        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        var database = client.GetDatabase(mongoDbConfiguration.DatabaseName);
        var mongoDbContext = new MongoDbContext(database);

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, t => true);

        BsonSerializer.RegisterSerializer(typeof(IPolicy), new GenericPolicySerializer());

        BsonClassMap.RegisterClassMap<Company>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
        });

        services.AddSingleton(mongoDbContext);

        return services;
    }

    private static MongoDbConfiguration GetMongoDbConfiguration(IConfiguration configuration)
    {
        var mongoDbConfiguration = configuration
            .GetSection(nameof(MongoDbConfiguration))
            .Get<MongoDbConfiguration>();
        
        if (mongoDbConfiguration is null
            || string.IsNullOrEmpty(mongoDbConfiguration.ConnectionString)
            || string.IsNullOrEmpty(mongoDbConfiguration.DatabaseName))
        {
            throw new MongoConfigurationException($"Invalid '{nameof(MongoDbConfiguration)}' definition!");
        }

        return mongoDbConfiguration;
    }

    public static IServiceCollection AddHttpGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<HttpGlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}