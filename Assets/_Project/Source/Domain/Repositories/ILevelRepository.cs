using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Domain.Entities;

namespace Source.Domain.Repositories
{
    public interface ILevelRepository
    {
        UniTask<AddressableResult<LevelData>> LoadLevel(string levelId, Action<float> onProgress = null, CancellationToken ct = default);
    }
}