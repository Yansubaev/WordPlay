using R3;
using Source.Infrastructure.Signals;
using Source.Presentation.Adapters;
using Source.Presentation.Settings;
using Source.Presentation.ViewModels;
using Source.Presentation.Views;
using UnityEngine;
using Yans.UI.Views;
using Zenject;

namespace Source.Presentation.Screens
{
    public class GameplayPanel : ViewModelPanel
    {
        #region private fields

        [SerializeField]
        private ImageButtonView _pauseButton;

        [SerializeField]
        private ImageButtonView _undoButton;

        [SerializeField]
        private WordListView _wordListView;

        [SerializeField]
        private ClusterListView _clusterListView;

        [SerializeField]
        private RectTransform _dragArea;

        [SerializeField]
        private ColorPalette _colorPalette;

        private SignalBus _signalBus;
        private GameplayViewModel _viewModel;
        private WordListAdapter _wordListAdapter;
        private ClusterListAdapter _clusterListAdapter;
        #endregion

        public override void OnSubscribeToViewModelEvents(CompositeDisposable disposables)
        {
            _viewModel.LevelData
                .Where(levelData => levelData != null)
                .Subscribe(levelData =>
                {
                    _wordListAdapter.UpdateDataset(levelData.Grid, levelData.ValidatedWords);
                    _clusterListAdapter.UpdateDataset(levelData.Clusters);
                })
                .AddTo(disposables);

            _viewModel.CanUndo
                .Subscribe(canUndo =>
                {
                    _undoButton.Interactable = canUndo;
                })
                .AddTo(disposables);
        }

        #region protected methods

        protected override void OnCreated()
        {
            _viewModel = ViewModelProvider.Get<GameplayViewModel>(this);
            _wordListAdapter = new WordListAdapter(_wordListView, _colorPalette);
            _clusterListAdapter = new ClusterListAdapter(_clusterListView, _dragArea);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            _pauseButton.OnClick += HandlePauseButtonClicked;
            _undoButton.OnClick += HandleUndoButtonClicked;
            _clusterListAdapter.OnHoveringOverEnter += _wordListAdapter.HandleHoveringOverEnter;
            _clusterListAdapter.OnHoveringOverExit += _wordListAdapter.HandleHoveringOverExit;
            _clusterListAdapter.OnReleasedCluster += _wordListAdapter.HandleReleasedCluster;
            _wordListAdapter.OnClusterPlaced += _viewModel.HandleClusterPlaced;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _pauseButton.OnClick -= HandlePauseButtonClicked;
            _undoButton.OnClick -= HandleUndoButtonClicked;
            _clusterListAdapter.OnHoveringOverEnter -= _wordListAdapter.HandleHoveringOverEnter;
            _clusterListAdapter.OnHoveringOverExit -= _wordListAdapter.HandleHoveringOverExit;
            _clusterListAdapter.OnReleasedCluster -= _wordListAdapter.HandleReleasedCluster;
            _wordListAdapter.OnClusterPlaced -= _viewModel.HandleClusterPlaced;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            _wordListAdapter.Dispose();
            _clusterListAdapter.Dispose();
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

        private void HandleUndoButtonClicked(View view)
        {
            _viewModel.HandleUndoCommand();
        }

        #endregion
    }
}