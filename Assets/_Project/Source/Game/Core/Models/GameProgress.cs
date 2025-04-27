using System.Collections.Generic;
using Newtonsoft.Json;

namespace Source.Game.Core
{
    [JsonObject]
    public class GameProgress
    {
        [JsonProperty("currentLevel")]
        public string CurrentLevelId { get; set; }

        [JsonProperty("completedLevels")]
        public List<string> CompletedLevels { get; set; } = new List<string>();

        [JsonProperty("completedWords")]
        public List<string> CompletedWords { get; set; } = new List<string>();
    }
}