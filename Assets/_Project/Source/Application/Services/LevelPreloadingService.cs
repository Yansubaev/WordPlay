using Cysharp.Threading.Tasks;
using Source.Domain.Entities;
using Source.Domain.Repositories;
using Source.Domain.Serivces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async UniTask<LevelPreloadingResult> LoadLevels(string[] levelIds, Action<float> onProgress = null)
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
                    progress =>
                    {
                        var overallProgress = (i + progress) / levelIds.Length;
                        onProgress?.Invoke(overallProgress);
                    },
                    default);

                loadResults.Add(result.Status == AddressableStatus.Success);
            }

            return DetermineResult(loadResults);
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