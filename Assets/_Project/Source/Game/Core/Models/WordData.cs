using Newtonsoft.Json;

namespace Source.Game.Core
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