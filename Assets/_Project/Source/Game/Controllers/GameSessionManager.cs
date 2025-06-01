using Cysharp.Threading.Tasks;
using Source.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;

namespace Source.Game.Controllers
{
    public class GameSessionManager
    {
        #region private fields
        private readonly ILevelLoaderService _levelLoaderService;
        private readonly IClusterGameService _clusterGameService;
        private readonly IGameProgressService _gameProgressService;
        private List<string> _levelChain;
        private List<string> _completedLevels = new();
        private LevelData _currentLevelData;
        private CancellationTokenSource _cancellationTokenSource = new();
        #endregion

        public event Action<LevelData> OnLevelStarted;

        #region public methods

        public GameSessionManager(
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
            _levelChain = await _levelLoaderService.LoadLevelChain(_cancellationTokenSource.Token);
            var progress = _gameProgressService.LoadProgress();
            if (progress == null)
            {
                if (_levelChain.Count == 0)
                {
                    throw new Exception("No levels available in the level chain.");
                }

                progress = new GameProgress
                {
                    CurrentLevelId = _levelChain[0],
                    CompletedLevels = new List<string>(),
                    CompletedWords = new List<string>()
                };
            }
            _completedLevels = progress.CompletedLevels;

            await StartLevel(progress.CurrentLevelId);
        }

        public void CloseGame()
        {
            _clusterGameService.Validate(out var matches, out var posInGrid);

            GameProgress newProgress = new()
            {
                CurrentLevelId = _currentLevelData.LevelId,
                CompletedLevels = _completedLevels,
                CompletedWords = matches,
            };

            _gameProgressService.SaveProgress(newProgress);

            _cancellationTokenSource.Cancel();
        }

        public LevelData GetCurrentLevelData()
        {
            return _currentLevelData;
        }

        public void ValidateCurrentLevel()
        {
            if (_clusterGameService.Validate(out var matchedWords, out var posInGrid))
            {
                if (_currentLevelData.Words.Select(e => e.Solution).All(word => matchedWords.Contains(word)))
                {
                    _completedLevels.Add(_currentLevelData.LevelId);
                    StartNextLevel();
                }
            }
        }
        #endregion

        private void StartNextLevel()
        {
            int currentIndex = _levelChain.IndexOf(_currentLevelData.LevelId);
            if (currentIndex < _levelChain.Count - 1)
            {
                string nextLevelId = _levelChain[currentIndex + 1];
                StartLevel(nextLevelId).Forget();
            }
            else
            {
                // Handle end of game logic here, e.g., show completion screen
            }
        }

        private async UniTask StartLevel(string levelId)
        {
            _currentLevelData = await _levelLoaderService.LoadLevel(levelId, _cancellationTokenSource.Token);
            _clusterGameService.StartLevel(_currentLevelData);
            OnLevelStarted?.Invoke(_currentLevelData);
        }
    }
}