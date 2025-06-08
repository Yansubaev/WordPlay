using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Domain.Entities;

namespace Source.Domain.Serivces
{
    public interface ILevelPreloadingService
    {
        public UniTask<bool> ShouldPreloadLevels(string[] levelIds, CancellationToken cancellationToken = default);
        UniTask<LevelPreloadingResult> LoadLevels(string[] levelIds, Action<float> onProgress, CancellationToken cancellationToken = default);
    }
}