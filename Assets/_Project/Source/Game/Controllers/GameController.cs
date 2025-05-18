using Source.Game.Core;
using System;
using System.Threading;
using Zenject;

namespace Source.Game.Controllers
{
    public class GameController
    {
        #region private fields
        private ILevelLoaderService _levelLoaderService;
        private IClusterGameService _clusterGameService;
        private IGameProgressService _gameProgressService;
        private LevelData _levelData;
        private CancellationTokenSource _cancellationTokenSource = new();
        #endregion

        public event Action<LevelData> OnLevelStarted;

        #region public methods

        [Inject]
        public void Inject(
            ILevelLoaderService levelLoaderService,
            IClusterGameService clusterGameService,
            IGameProgressService gameProgressService)
        {
            _levelLoaderService = levelLoaderService;
            _clusterGameService = clusterGameService;
            _gameProgressService = gameProgressService;
        }

        public async void StartGame()
        {
            var progress = _gameProgressService.LoadProgress();

            _levelData = await _levelLoaderService.LoadLevel(progress.CurrentLevelId, _cancellationTokenSource.Token);
            _clusterGameService.StartLevel(_levelData);
            OnLevelStarted?.Invoke(_levelData);
        }

        public void CloseGame()
        {
            if (_clusterGameService.Validate(out var matches))
            {
            }

            var progress = _gameProgressService.LoadProgress();
            GameProgress newProgress = new()
            {
                CurrentLevelId = _levelData.LevelId,
                CompletedLevels = progress.CompletedLevels,
                CompletedWords = matches,
            };

            _gameProgressService.SaveProgress(newProgress);

            _cancellationTokenSource.Cancel();
        }

        #endregion
    }
}