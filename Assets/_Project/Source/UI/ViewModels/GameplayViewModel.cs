using R3;
using Source.Game.Commands;
using Source.Game.Controllers;
using Source.Game.Core;
using System;
using System.Linq;
using Yans.ViewModels;
using Zenject;

namespace Source.UI.Screens
{
    #region public classes

    public class GameplayViewModel : ViewModel
    {
        private const int MaxHistorySize = 50;

        #region public properties
        public ReactiveProperty<LevelViewData> LevelData => _levelData;
        public ReactiveProperty<bool> CanUndo => _canUndo;
        #endregion

        #region private fields
        private ReactiveProperty<LevelViewData> _levelData = new();
        private ReactiveProperty<bool> _canUndo = new(false);
        private GameSessionService _gameSessionService;
        private IClusterGameService _clusterGameService;
        private CommandManager _commandManager = new();
        #endregion

        public GameplayViewModel(GameSessionService gameController, IClusterGameService clusterGameService)
        {
            _gameSessionService = gameController;
            _clusterGameService = clusterGameService;
        }

        #region public methods

        public void HandleClusterPlaced(string cluster, int wordIndex, int startIndex)
        {
            // Создаем команду для размещения кластера
            var command = new PlaceClusterCommand(_clusterGameService, this, cluster, wordIndex, startIndex);

            _commandManager.ExecuteCommand(command);
            _canUndo.Value = _commandManager.CanUndo;
        }

        public void HandleUndoCommand()
        {
            _commandManager.UndoLastCommand();
            _canUndo.Value = _commandManager.CanUndo;
        }

        public void UpdateLevelData()
        {
            var validationSuccessful = _clusterGameService.Validate(out var matches, out var posInGrid);
            var data = _gameSessionService.GetCurrentLevelData();

            var levelViewData = new LevelViewData(
                data.LevelId,
                _clusterGameService.GetGridState(),
                _clusterGameService.GetAvailableClusters().ToArray(),
                data.Words.Select(e => e.Hint).ToArray(),
                posInGrid.ToArray()
            );

            _levelData.Value = levelViewData;
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

        #region protected methods

        protected override void OnCreated()
        {
            _gameSessionService.OnLevelStarted += HandleLevelStarted;
        }

        protected override void OnAborted()
        {
            _gameSessionService.OnLevelStarted -= HandleLevelStarted;
        }

        #endregion

        #region private methods

        private void HandleLevelStarted(LevelData data)
        {
            // Очищаем историю команд при старте нового уровня
            _commandManager.ClearHistory();
            _canUndo.Value = false;

            var levelViewData = new LevelViewData(
                data.LevelId,
                _clusterGameService.GetGridState(),
                _clusterGameService.GetAvailableClusters().ToArray(),
                data.Words.Select(e => e.Hint).ToArray()
            );

            _levelData.Value = levelViewData;
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

    #endregion 
}