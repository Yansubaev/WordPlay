using Cysharp.Threading.Tasks;
using Source.Domain.Entities;
using System.Threading;

namespace Source.Domain.Serivces
{
    public interface ILevelLoadingService
    {
        UniTask<LevelData> LoadLevel(string levelId, CancellationToken ct = default);
        UniTask<string[]> LoadLevelChain(CancellationToken ct = default);
    }
}