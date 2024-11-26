using Newtonsoft.Json;

namespace Company.Function
{
    public class Counter
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }  // Nullable property

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string? PartitionKey { get; set; }  // Nullable property
    }
}
