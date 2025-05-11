using R3;
using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using Yans.UI.Views;
using Zenject;

namespace Source.UI.Screens
{
    public class GameplayPanel : ViewModelPanel
    {
        #region private fields

        [SerializeField]
        private ImageButtonView _pauseButton;

        private SignalBus _signalBus;
        private GameplayViewModel _viewModel;
        #endregion

        #region public methods

        public override void OnSubscribeToViewModelEvents(CompositeDisposable disposables)
        {
            _viewModel.LevelData
                .Where(levelData => levelData != null)
                .Subscribe(levelData =>
                {
                    // Handle level data updates here
                    Debug.Log($"Level ID: {levelData.LevelId}");
                    Debug.Log($"Target Words: {string.Join(", ", levelData.TargetWords)}");
                    Debug.Log($"Clusters: {string.Join(", ", levelData.Clusters)}");
                    Debug.Log($"Hints: {string.Join(", ", levelData.Hints)}");
                })
                .AddTo(disposables);
        }

        #endregion

        #region protected methods

        protected override void OnCreated()
        {
            _viewModel = ViewModelProvider.Get<GameplayViewModel>(this);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            _pauseButton.OnClick += HandlePauseButtonClicked;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _pauseButton.OnClick -= HandlePauseButtonClicked;
        }

        #endregion

        #region private methods

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void HandlePauseButtonClicked(View view)
        {
            _signalBus.Fire<PauseGameSignal>();
        }

        #endregion
    }
}