using R3;
using Source.Game.Commands;
using Source.Game.Controllers;
using Source.Game.Core;
using System;
using System.Linq;
using Yans.ViewModels;

namespace Source.UI.Screens
{
    #region public classes

    public class GameplayViewModel : ViewModel
    {
        #region public properties
        public ReactiveProperty<LevelViewData> LevelData => _levelData;
        public ReactiveProperty<bool> CanUndo => _canUndo;
        #endregion

        #region private fields
        private readonly ReactiveProperty<LevelViewData> _levelData = new();
        private readonly ReactiveProperty<bool> _canUndo = new(false);
        private readonly GameSessionManager _gameSessionManager;
        private readonly IClusterGameService _clusterGameService;
        private readonly CommandManager _commandManager = new();
        #endregion

        public GameplayViewModel(GameSessionManager sessionManager, IClusterGameService clusterGameService)
        {
            _gameSessionManager = sessionManager;
            _clusterGameService = clusterGameService;
        }

        #region public methods

        public void HandleClusterPlaced(string cluster, int wordIndex, int startIndex)
        {
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
            var data = _gameSessionManager.GetCurrentLevelData();

            var levelViewData = new LevelViewData(
                data.LevelId,
                _clusterGameService.GetGridState(),
                _clusterGameService.GetAvailableClusters().ToArray(),
                data.Words.Select(e => e.Hint).ToArray(),
                posInGrid.ToArray()
            );

            _levelData.Value = levelViewData;

            _gameSessionManager.ValidateCurrentLevel();
        }

        #endregion

        #region protected methods

        protected override void OnCreated()
        {
            _gameSessionManager.OnLevelStarted += HandleLevelStarted;
        }

        protected override void OnAborted()
        {
            _gameSessionManager.OnLevelStarted -= HandleLevelStarted;
        }

        #endregion

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