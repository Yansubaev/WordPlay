using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Game.Core
{
    public interface ILevelLoaderService
    {
        UniTask<LevelData> LoadLevel(string levelId, CancellationToken ct);
        UniTask<List<string>> LoadLevelChain(CancellationToken ct);
    }
}