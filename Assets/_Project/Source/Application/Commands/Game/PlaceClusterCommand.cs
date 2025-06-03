using System;
using Source.Domain.Serivces;
using Source.Presentation.ViewModels;

namespace Source.Application.Commands
{
    /// <summary>
    /// Command to place cluster on the game board
    /// </summary>
    public class PlaceClusterCommand : ICommand
    {
        public string Description => $"Place cluster '{_cluster}' at word {_wordIndex}, position {_startIndex}";

        #region private fields
        private readonly IClusterGameService _clusterGameService;
        private readonly GameplayViewModel _gameplayViewModel;
        private readonly string _cluster;
        private readonly int _wordIndex;
        private readonly int _startIndex;
        private readonly Action _onCommandExecuted;
        #endregion

        public PlaceClusterCommand(
            IClusterGameService clusterGameService,
            GameplayViewModel gameplayViewModel,
            string cluster,
            int wordIndex,
            int startIndex,
            Action onCommandExecuted = null)
        {
            _clusterGameService = clusterGameService;
            _gameplayViewModel = gameplayViewModel;
            _cluster = cluster;
            _wordIndex = wordIndex;
            _startIndex = startIndex;
            _onCommandExecuted = onCommandExecuted;
        }

        #region public methods

        public bool Execute()
        {
            var success = _clusterGameService.TryPlaceCluster(_cluster, _wordIndex, _startIndex);
            _onCommandExecuted?.Invoke();
            return success;
        }

        public void Undo()
        {
            _clusterGameService.TryRemoveCluster(_cluster, _wordIndex, _startIndex);
            _onCommandExecuted?.Invoke();
        }

        #endregion
    }
}