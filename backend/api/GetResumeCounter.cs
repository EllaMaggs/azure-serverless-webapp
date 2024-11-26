using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System.Net;

namespace Company.Function
{
    public class GetResumeCounter
    {
        private readonly ILogger<GetResumeCounter> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private const string DatabaseName = "EMWebAppProjectv4";
        private const string ContainerName = "VisitorCounter";  // Updated container name

        public GetResumeCounter(ILogger<GetResumeCounter> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;

            _logger.LogInformation($"Attempting to get container {ContainerName} from database {DatabaseName}");

            try
            {
                _container = _cosmosClient.GetContainer(DatabaseName, ContainerName);
                _logger.LogInformation("Successfully got container reference");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting container reference");
                throw;
            }
        }

        [Function("GetResumeCounter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Starting GetResumeCounter function");

            try
            {
                // Attempt to read the counter document (upsert if not found)
                var response = await _container.ReadItemAsync<ResumeCounter>(
                    "1",  // document id
                    new PartitionKey("1")  // partition key, using '1' as partition key
                );

                _logger.LogInformation($"Successfully read counter document. Current count: {response.Resource.Count}");

                var counter = response.Resource;
                counter.Count += 1;

                _logger.LogInformation($"Updating counter to new value: {counter.Count}");

                // Update the counter (upsert if document doesn't exist)
                await _container.UpsertItemAsync(counter, new PartitionKey(counter.PartitionKey));

                _logger.LogInformation("Successfully updated counter");

                var httpResponse = req.CreateResponse(HttpStatusCode.OK);
                httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");  // Added CORS header
                await httpResponse.WriteAsJsonAsync(counter);

                return httpResponse;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"Counter document not found. Status: {ex.StatusCode}, Sub-status: {ex.SubStatusCode}");
                _logger.LogWarning($"Error message: {ex.Message}");

                // If counter doesn't exist, create it
                _logger.LogInformation("Creating new counter document");

                var newCounter = new ResumeCounter 
                { 
                    Id = "1",  // Explicitly setting id to 1
                    PartitionKey = "1", // Partition key for Cosmos DB
                    Count = 1 
                };

                // Create the new counter document
                await _container.CreateItemAsync(newCounter, new PartitionKey(newCounter.PartitionKey));

                _logger.LogInformation("Successfully created new counter document");

                var httpResponse = req.CreateResponse(HttpStatusCode.OK);
                httpResponse.Headers.Add("Access-Control-Allow-Origin", "*");  // Added CORS header
                await httpResponse.WriteAsJsonAsync(newCounter);

                return httpResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing counter");
                var httpResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await httpResponse.WriteStringAsync($"An error occurred while processing the request: {ex.Message}");
                return httpResponse;
            }
        }

        public class ResumeCounter
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("partitionKey")]
            public string PartitionKey { get; set; } = "1"; // Default partition key
        }
    }
}
