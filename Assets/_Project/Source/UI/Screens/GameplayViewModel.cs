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
        private void Inject(GameController gameController)
        {
            _gameController = gameController;
        }

        private void HandleLevelStarted(LevelData data)
        {
            var levelViewData = new LevelViewData(
                data.LevelId,
                data.TargetWords.ToArray(),
                data.Clusters.ToArray(),
                data.Hints.ToArray()
            );

            _levelData.Value = levelViewData;
        }

        #endregion
    }

    public class LevelViewData
    {
        #region public properties
        public string LevelId { get; private set; }
        public string[] TargetWords { get; private set; }
        public string[] Clusters { get; private set; }
        public string[] Hints { get; private set; }
        #endregion

        public LevelViewData(string levelId, string[] targetWords, string[] clusters, string[] hints)
        {
            LevelId = levelId;
            TargetWords = targetWords;
            Clusters = clusters;
            Hints = hints;
        }
    }
}