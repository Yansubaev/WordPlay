using Cysharp.Threading.Tasks;
using ModestTree;
using Source.Domain.Entities;
using Source.Domain.Repositories;
using Source.Domain.Serivces;
using Source.Infrastructure.SceneManagement;
using Source.Infrastructure.Signals;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class LoadMainSceneState : IState, ILateDisposable
    {
        private const int MaxLevelsToLoad = 10;

        #region private fields
        private SignalBus _signalBus;
        private GameStateMachine _stateMachine;
        private ISceneLoaderService _sceneLoader;
        private ILevelChainRepository _levelChainRepository;
        private IGameProgressService _gameProgressService;
        private ILevelPreloadingService _levelPreloadingService;
        private CancellationTokenSource _cts;
        #endregion

        #region public methods

        [Inject]
        public void Inject(
            SignalBus signalBus,
            GameStateMachine stateMachine,
            ISceneLoaderService sceneLoader,
            ILevelChainRepository levelChainRepository,
            IGameProgressService gameProgressService,
            ILevelPreloadingService levelPreloadingService)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
            _levelChainRepository = levelChainRepository;
            _gameProgressService = gameProgressService;
            _levelPreloadingService = levelPreloadingService;

            _cts = new CancellationTokenSource();
        }

        public async UniTask Enter()
        {
            var levelChain = await _levelChainRepository.LoadLevelChain(_cts.Token);
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

            var shouldPreloadLevels = await _levelPreloadingService.ShouldPreloadLevels(levelsToLoad.ToArray(), _cts.Token);
            shouldPreloadLevels = true;

            if (!shouldPreloadLevels)
            {
                Debug.Log("All levels are cached and cache is relevant. Skipping preloading.");
                EnterMainMenu();
                return;
            }
            else
            {
                Debug.Log("Some levels are not cached or cache is outdated. Preloading levels...");
                LoadLevelsWithProgress(levelsToLoad.ToArray());
            }

        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        public void LateDispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        #endregion

        #region private methods

        private async void EnterMainMenu()
        {
            await _sceneLoader.LoadSceneAsync("Main", cancellationToken: _cts.Token);
            await _stateMachine.Enter<MainMenuState>();
        }

        private async void LoadLevelsWithProgress(string[] levelIds)
        {
            await _sceneLoader.LoadSceneAsync("Loading", cancellationToken: _cts.Token);

            var result = await _levelPreloadingService.LoadLevels(
                levelIds,
                progress =>
                {
                    _signalBus.Fire(new LoadingProgressSignal(progress));
                },
                _cts.Token);

            if (result == LevelPreloadingResult.Completed || result == LevelPreloadingResult.CompletedPartially)
            {
                EnterMainMenu();
            }
            else
            {
                Debug.LogError("Failed to load levels.");
                _signalBus.Fire(new LoadingProgressSignal(-1f));

                // EnterMainMenu();
            }

        }

        #endregion
    }
}