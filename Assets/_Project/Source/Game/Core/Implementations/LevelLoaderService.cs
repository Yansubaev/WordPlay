using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Game.Core
{
    public class LevelLoaderService : ILevelLoaderService
    {
        private readonly string _addressTemplate;

        public LevelLoaderService(string addressTemplate)
        {
            _addressTemplate = addressTemplate;
        }

        public async UniTask<LevelData> LoadLevel(string levelId, CancellationToken ct)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<TextAsset>(string.Format(_addressTemplate, levelId));

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