using Cysharp.Threading.Tasks;
using Source.Domain.Entities;
using Source.Domain.Repositories;
using Source.Domain.Serivces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Source.Application.Services
{
    public class LevelPreloadingService : ILevelPreloadingService
    {
        private readonly ILevelRepository _levelRepository;

        public LevelPreloadingService(ILevelRepository levelRepository)
        {
            _levelRepository = levelRepository;
        }

        #region public methods

        public async UniTask<bool> ShouldPreloadLevels(string[] levelIds, CancellationToken cancellationToken = default)
        {
            if (levelIds == null || levelIds.Length == 0)
            {
                Debug.LogWarning("No levels provided for preloading.");
                return false;
            }

            foreach (var levelId in levelIds)
            {
                var isAvailable = await _levelRepository.IsLevelAvaialble(levelId, cancellationToken);

                if (!isAvailable)
                    return true; // At least one level is not cached, so preloading is needed

            }


            return false; // All levels are cached and cache is relevant
        }

        public async UniTask<LevelPreloadingResult> LoadLevels(
            string[] levelIds,
            Action<float> onProgress = null,
            CancellationToken cancellationToken = default)
        {
            if (levelIds.Length == 0)
            {
                Debug.LogWarning("No levels to preload.");
                return LevelPreloadingResult.Completed;
            }

            var loadResults = new List<bool>(levelIds.Length);

            for (int i = 0; i < levelIds.Length; i++)
            {
                var result = await _levelRepository.LoadLevel(
                    levelIds[i],
                    onProgress: progress =>
                    {
                        var overallProgress = (i + progress) / levelIds.Length;
                        onProgress?.Invoke(overallProgress);
                    },
                    ct: cancellationToken);

                var overallProgress = (i + 1f) / levelIds.Length;
                onProgress?.Invoke(overallProgress);

                loadResults.Add(result.Status == AddressableStatus.Success);
            }

            return DetermineResult(loadResults);
        }

        #endregion

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