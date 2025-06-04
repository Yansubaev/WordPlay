
using System;
using Cysharp.Threading.Tasks;
using Source.Domain.Entities;

namespace Source.Domain.Serivces
{
    public interface ILevelPreloadingService
    {
        UniTask<LevelPreloadingResult> LoadLevels(string[] levelIds, Action<float> onProgress);
    }
}