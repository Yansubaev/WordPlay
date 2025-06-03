using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Source.Application.Services
{
    public interface ILevelChainRepository
    {
        UniTask<string[]> LoadLevelChain(CancellationToken ct = default);
    }

    public class LevelChainRepository : ILevelChainRepository
    {
        private readonly string _addressTemplate;

        public LevelChainRepository([Inject(Id = "addressTemplate")] string addressTemplate)
        {
            _addressTemplate = addressTemplate;
        }

        public async UniTask<string[]> LoadLevelChain(CancellationToken ct = default)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<TextAsset>(string.Format(_addressTemplate, "level_chain"));

                await handle.ToUniTask(cancellationToken: ct);

                if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded || handle.Result == null)
                    throw new Exception("Failed to load level chain from Addressables.");

                var json = handle.Result.text;

                var chain = JsonConvert.DeserializeAnonymousType(json, new { chain = new List<string>() });

                return chain.chain.ToArray() ?? Array.Empty<string>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading level chain: {ex.Message}");
                throw;
            }

        }
    }
}