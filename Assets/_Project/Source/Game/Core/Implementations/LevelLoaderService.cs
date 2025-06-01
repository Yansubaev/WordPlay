using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async UniTask<List<string>> LoadLevelChain(CancellationToken ct)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<TextAsset>(string.Format(_addressTemplate, "level_chain"));

                await handle.ToUniTask(cancellationToken: ct);

                if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded || handle.Result == null)
                    throw new Exception("Failed to load level chain from Addressables.");

                var json = handle.Result.text;

                var chain = JsonConvert.DeserializeAnonymousType(json, new { chain = new List<string>() });
                return chain.chain ?? new List<string>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading level chain: {ex.Message}");
                throw;
            }
        }
    }
}