using Source.Game.Core;
using Source.UI.Screens;

namespace Source.Game.Commands
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
        #endregion

        public PlaceClusterCommand(
            IClusterGameService clusterGameService,
            GameplayViewModel gameplayViewModel,
            string cluster,
            int wordIndex,
            int startIndex)
        {
            _clusterGameService = clusterGameService;
            _gameplayViewModel = gameplayViewModel;
            _cluster = cluster;
            _wordIndex = wordIndex;
            _startIndex = startIndex;
        }

        #region public methods

        public bool Execute()
        {
            var success = _clusterGameService.TryPlaceCluster(_cluster, _wordIndex, _startIndex);
            _gameplayViewModel.UpdateLevelData();
            return success;
        }

        public void Undo()
        {
            _clusterGameService.TryRemoveCluster(_cluster, _wordIndex, _startIndex);
            _gameplayViewModel.UpdateLevelData();
        }

        #endregion
    }
}