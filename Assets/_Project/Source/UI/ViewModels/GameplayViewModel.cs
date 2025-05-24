using System;
using System.Linq;
using R3;
using Source.Game.Controllers;
using Source.Game.Core;
using Yans.ViewModels;
using Zenject;

namespace Source.UI.Screens
{
    public class GameplayViewModel : ViewModel
    {
        #region private fields
        private ReactiveProperty<LevelViewData> _levelData = new();
        private GameController _gameController;
        private IClusterGameService _clusterGameService;
        #endregion

        #region public properties
        public ReactiveProperty<LevelViewData> LevelData => _levelData;
        #endregion

        #region protected methods

        protected override void OnCreated()
        {
            _gameController.OnLevelStarted += HandleLevelStarted;
        }

        protected override void OnAborted()
        {
            _gameController.OnLevelStarted -= HandleLevelStarted;
        }

        #endregion

        #region private methods

        [Inject]
        private void Inject(GameController gameController, IClusterGameService clusterGameService)
        {
            _gameController = gameController;
            _clusterGameService = clusterGameService;
        }

        private void HandleLevelStarted(LevelData data)
        {
            var levelViewData = new LevelViewData(
                data.LevelId,
                _clusterGameService.GetGridState(),
                _clusterGameService.GetAvailableClusters().ToArray(),
                data.Words.Select(e => e.Hint).ToArray()
            );

            _levelData.Value = levelViewData;
        }

        public void HandleClusterPlaced(string cluster, int wordIndex, int startIndex)
        {
            if (_clusterGameService.TryPlaceCluster(cluster, wordIndex, startIndex))
            {
                var validationSuccessful = _clusterGameService.Validate(out var matches, out var posInGrid);

                var data = _gameController.GetCurrentLevelData();

                var levelViewData = new LevelViewData(
                    data.LevelId,
                    _clusterGameService.GetGridState(),
                    _clusterGameService.GetAvailableClusters().ToArray(),
                    data.Words.Select(e => e.Hint).ToArray(),
                    posInGrid.ToArray()
                );

                _levelData.Value = levelViewData;
            }
        }

        public void ValidateLevel()
        {
            if (_clusterGameService.Validate(out var matches, out var posInGrid))
            {
                // Handle successful validation, e.g., show success message or proceed to next level

            }
            else
            {
                // Handle validation failure, e.g., show error message
            }
        }

        #endregion
    }

    public class LevelViewData
    {
        #region public properties
        public string LevelId { get; private set; }
        public string[,] Grid { get; private set; }
        public string[] Clusters { get; private set; }
        public string[] Hints { get; private set; }
        public int[] ValidatedWords { get; private set; } = Array.Empty<int>();
        #endregion

        public LevelViewData(string levelId, string[,] grid, string[] clusters, string[] hints, int[] validatedWords = null)
        {
            LevelId = levelId;
            Grid = grid;
            Clusters = clusters;
            Hints = hints;
            ValidatedWords = validatedWords ?? Array.Empty<int>();
        }
    }
}