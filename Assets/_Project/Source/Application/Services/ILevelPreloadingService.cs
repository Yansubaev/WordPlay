
using System;
using Cysharp.Threading.Tasks;

namespace Source.Application.Services
{
    public interface ILevelPreloadingService
    {
        UniTask<LevelPreloadingResult> LoadLevels(string[] levelIds, Action<float> onProgress);
    }

    public enum LevelPreloadingResult
    {
        Completed,
        CompletedPartially,
        Failed
    }
}