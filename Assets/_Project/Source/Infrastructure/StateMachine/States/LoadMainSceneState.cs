using Cysharp.Threading.Tasks;
using ModestTree;
using Source.Domain.Repositories;
using Source.Domain.Serivces;
using Source.Infrastructure.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    
    public class LoadMainSceneState : IState
    {
        private const int MaxLevelsToLoad = 10;

        #region private fields
        private GameStateMachine _stateMachine;
        private ISceneLoaderService _sceneLoader;
        private ILevelChainRepository _levelChainRepository;
        private IGameProgressService _gameProgressService;
        private ILevelPreloadingService _levelPreloadingService;
        #endregion

        [Inject]
        public void Inject(
            GameStateMachine stateMachine,
            ISceneLoaderService sceneLoader,
            ILevelChainRepository levelChainRepository,
            IGameProgressService gameProgressService,
            ILevelPreloadingService levelPreloadingService)
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
            _levelChainRepository = levelChainRepository;
            _gameProgressService = gameProgressService;
            _levelPreloadingService = levelPreloadingService;
        }

        #region public methods

        public async UniTask Enter()
        {
            var levelChain = await _levelChainRepository.LoadLevelChain();
            var progress = _gameProgressService.LoadProgress();

            var levelsToLoad = new List<string>();

            if (progress == null)
            {
                levelsToLoad.AddRange(levelChain.Take(MaxLevelsToLoad));
            }
            else
            {
                var t = levelChain.IndexOf(progress.CurrentLevelId);
                levelsToLoad.AddRange(levelChain.Skip(t + 1).Take(MaxLevelsToLoad));
            }

            var res = await _levelPreloadingService.LoadLevels(levelsToLoad.ToArray(), progress =>
            {
                UnityEngine.Debug.Log($"Preloading progress: {progress * 100:00}%");
            });

            EnterMainMenu();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        #endregion

        private async void EnterMainMenu()
        {
            await _sceneLoader.LoadSceneAsync("Main");
            await _stateMachine.Enter<MainMenuState>();
        }
    }
}