using Newtonsoft.Json;

namespace Source.Domain.Entities
{
    [JsonObject]
    public class WordData
    {
        [JsonProperty("solution")]
        public string Solution { get; set; }

        [JsonProperty("hint")]
        public string Hint { get; set; }
    }
}