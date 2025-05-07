using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Game.Core
{
    public class LevelLoaderService : ILevelLoaderService
    {
        private const string AddressTemplate = "Level/ru/{0}";

        public async UniTask<LevelData> LoadLevel(string levelId, CancellationToken ct)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<TextAsset>(string.Format(AddressTemplate, levelId));

                await handle.ToUniTask(cancellationToken: ct);

                if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded || handle.Result == null)
                    throw new Exception($"Failed to load level: {levelId} from Addressables.");

                var json = handle.Result.text;
                return JsonConvert.DeserializeObject<LevelData>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading level {levelId}: {ex.Message}");
                throw;
            }

        }
    }
}