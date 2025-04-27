using System.Collections.Generic;
using Newtonsoft.Json;

namespace Source.Game.Core
{
    [JsonObject]
    public class LevelData
    {
        [JsonProperty("levelId")]
        public string LevelId { get; set; }

        [JsonProperty("targetWords")]
        public List<string> TargetWords { get; set; }

        [JsonProperty("clusters")]
        public List<string> Clusters { get; set; }

        [JsonProperty("hints")]
        public List<string> Hints { get; set; }
    }
}