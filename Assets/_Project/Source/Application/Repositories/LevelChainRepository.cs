using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Source.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Source.Application.Repositories
{
    public class LevelChainRepository : BaseRepository, ILevelChainRepository
    {
        private readonly string _addressTemplate;

        public LevelChainRepository([Inject(Id = "addressTemplate")] string addressTemplate)
        {
            _addressTemplate = addressTemplate;
        }

        public UniTask<string[]> LoadLevelChain(CancellationToken ct = default)
        {
            return Execute<string[]>(async () =>
            {
                var handle = Addressables.LoadAssetAsync<TextAsset>(string.Format(_addressTemplate, "level_chain"));

                await handle.ToUniTask(cancellationToken: ct);

                if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
                    throw new Exception("Failed to load level chain from Addressables.");

                var json = handle.Result.text;

                var chain = JsonConvert.DeserializeAnonymousType(json, new { chain = new List<string>() });

                return chain.chain.ToArray() ?? Array.Empty<string>();
            });
        }
    }
}