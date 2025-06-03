using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Source.Application.Services
{
    public class LevelPreloadingService : ILevelPreloadingService
    {
        private readonly string _levelAddressTemplate;

        public LevelPreloadingService([Inject(Id = "addressTemplate")] string addressTemplate)
        {
            _levelAddressTemplate = addressTemplate;
        }

        public async UniTask<LevelPreloadingResult> LoadLevels(string[] levelIds, Action<float> onProgress = null)
        {
            if (levelIds.Length == 0)
            {
                Debug.LogWarning("No levels to preload.");
                return LevelPreloadingResult.Completed;
            }

            var addresses = levelIds.Select(id => string.Format(_levelAddressTemplate, id)).ToArray();
            var loadResults = new List<bool>(addresses.Length);

            for (int i = 0; i < addresses.Length; i++)
            {
                var result = await LoadSingleLevel(addresses[i], i, addresses.Length, onProgress);
                loadResults.Add(result);
            }

            return DetermineResult(loadResults);
        }

        private async UniTask<bool> LoadSingleLevel(string address, int index, int totalCount, Action<float> onProgress)
        {
            Debug.Log($"Preloading level: {address}");

            var handle = Addressables.LoadAssetAsync<TextAsset>(address);

            while (!handle.IsDone)
            {
                var overallProgress = (index + handle.PercentComplete) / totalCount;
                onProgress?.Invoke(overallProgress);
                await UniTask.Yield();
            }

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                Debug.Log($"Successfully preloaded level: {address}");
                return true;
            }

            Debug.LogError($"Failed to preload level: {address}");
            return false;
        }

        private static LevelPreloadingResult DetermineResult(List<bool> results)
        {
            if (results.All(success => success))
                return LevelPreloadingResult.Completed;

            if (results.Any(success => success))
                return LevelPreloadingResult.CompletedPartially;

            return LevelPreloadingResult.Failed;
        }
    }
}