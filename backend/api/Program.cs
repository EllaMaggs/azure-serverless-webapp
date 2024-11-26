using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Cosmos;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Register the CosmosClient with the correct connection string
        services.AddSingleton<CosmosClient>(sp =>
        {
            var cosmosConnectionString = Environment.GetEnvironmentVariable("AzureResumeConnectionString");
            return new CosmosClient(cosmosConnectionString);
        });
    })
    .Build();

host.Run();