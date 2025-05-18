using System.Collections.Generic;
using Newtonsoft.Json;

namespace Source.Game.Core
{
    [JsonObject]
    public class LevelData
    {
        [JsonProperty("levelId")]
        public string LevelId { get; set; }

        [JsonProperty("words")]
        public List<WordData> Words { get; set; }

        [JsonProperty("clusters")]
        public List<string> Clusters { get; set; }
    }
}