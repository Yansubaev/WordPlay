using Newtonsoft.Json;
using Source.Domain.Entities;
using Source.Domain.Serivces;
using UnityEngine;

namespace Source.Application.Services
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