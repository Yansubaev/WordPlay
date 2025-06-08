using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Domain.Entities;

namespace Source.Domain.Repositories
{
    public interface ILevelRepository
    {
        UniTask<bool> IsLevelAvaialble(string levelId, CancellationToken ct = default);
        UniTask<AddressableResult<LevelData>> LoadLevel(string levelId, Action<float> onProgress = null, CancellationToken ct = default);
    }
}