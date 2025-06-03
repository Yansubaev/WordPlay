using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Domain.Repositories
{
    public interface ILevelChainRepository
    {
        UniTask<string[]> LoadLevelChain(CancellationToken ct = default);
    }
}