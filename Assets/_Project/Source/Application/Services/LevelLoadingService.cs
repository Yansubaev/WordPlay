using Cysharp.Threading.Tasks;
using Source.Domain.Entities;
using Source.Domain.Repositories;
using Source.Domain.Serivces;
using System.Threading;

namespace Source.Application.Services
{
    public class LevelLoadingService : ILevelLoadingService
    {
        #region private fields
        private readonly ILevelChainRepository _levelChainRepository;
        private readonly ILevelRepository _levelRepository;
        #endregion

        public LevelLoadingService(
            ILevelChainRepository levelChainRepository,
            ILevelRepository levelRepository)
        {
            _levelChainRepository = levelChainRepository;
            _levelRepository = levelRepository;
        }

        #region public methods

        public async UniTask<LevelData> LoadLevel(string levelId, CancellationToken ct = default)
        {
            var res = await _levelRepository.LoadLevel(levelId, null, ct);
            if (res.Status != AddressableStatus.Success)
                return null;

            return res.Result;
        }

        public UniTask<string[]> LoadLevelChain(CancellationToken ct = default)
        {
            return _levelChainRepository.LoadLevelChain(ct);
        }

        #endregion
    }
}