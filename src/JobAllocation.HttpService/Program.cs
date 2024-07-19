using Autofac;
using Autofac.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using FastEndpoints;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using JobAllocation.HttpService.StartupInfra;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
var assemblyName = Assembly.GetExecutingAssembly().GetName();
var appName = assemblyName.Name;
var appVersion = Environment.GetEnvironmentVariable("DD_VERSION")
                 ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString();

try
{
    if (appName is null || appVersion is null)
        throw new ApplicationException($"{nameof(appName)} or {nameof(appVersion)} is null");
    
    Log.ForContext("ApplicationName", appName).Information("Starting application");
    Result.Configuration.ErrorMessagesSeparator = "§";

    builder.Services
        .AddHttpGlobalExceptionHandler()
        .AddEndpointsApiExplorer()
        .AddFastEndpoints()
        .AddOpenApiSpecs()
        .AddSingleDatabase(builder.Configuration)
        .AddHealthCheck(builder.Configuration)
        .AddLogs(builder.Configuration)
        .AddCache()
        .AddSecurity()
        .AddOptions()
        .AddVersioning()
        .AddCustomCors();

    builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
    {
        cb.RegisterModule(new ApplicationModule());
    });
    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    var app = builder.Build();
    app.UseFastEndpoints();
    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = registration => !registration.Tags.Contains("ready"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        // Add OpenAPI 3.0 document serving middleware
        // Available at: http://localhost:<port>/swagger/v1/swagger.json
        app.UseOpenApi();

        // Add web UIs to interact with the document
        // Available at: http://localhost:<port>/swagger
        app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
    }

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.ForContext("ApplicationName", appName)
        .Fatal(ex, "Program terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}