using System.Threading;
using Newtonsoft.Json;
using Source.Game.Core;
using UnityEngine;
using Zenject;

namespace Source.Game.Controllers
{
    public class GameController
    {
        private ILevelLoaderService _levelLoaderService;
        private IClusterGameService _clusterGameService;
        private IGameProgressService _gameProgressService;
        private LevelData _levelData;
        private CancellationTokenSource _cancellationTokenSource = new();

        [Inject]
        public void Inject(
            ILevelLoaderService levelLoaderService,
            IClusterGameService clusterGameService,
            IGameProgressService gameProgressService
            )
        {
            Debug.Log("<color=green>[ZEN] GameController.Inject</color>");

            _levelLoaderService = levelLoaderService;
            _clusterGameService = clusterGameService;
            _gameProgressService = gameProgressService;
        }

        public async void StartGame()
        {
            Debug.Log("<color=green>GameController.StartGame</color>");

            var progress = _gameProgressService.LoadProgress();

            _levelData = await _levelLoaderService.LoadLevel(progress.CurrentLevelId, _cancellationTokenSource.Token);

            Debug.Log($"<color=green>GameController.StartGame</color> level:\n{JsonConvert.SerializeObject(_levelData)}");

            _clusterGameService.StartLevel(_levelData);
        }

        public void CloseGame()
        {
            Debug.Log("<color=green>GameController.CloseGame</color>");

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
    }
}

