using Newtonsoft.Json;
using UnityEngine;

namespace Source.Game.Core
{
    public class GameProgressService : IGameProgressService
    {
        private const string ProgressKey = "GameProgress";

        public void SaveProgress(GameProgress progress)
        {
            PlayerPrefs.SetString(ProgressKey, JsonConvert.SerializeObject(progress));
            PlayerPrefs.Save();
        }

        public GameProgress LoadProgress()
        {
            string json = PlayerPrefs.GetString(ProgressKey, null);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<GameProgress>(json);
            }

        }
    }
}