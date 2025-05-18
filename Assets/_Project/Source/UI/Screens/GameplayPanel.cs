using R3;
using Source.Signals;
using Source.UI.Adapters;
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
        [SerializeField]
        private WordListView _wordListView;
        [SerializeField]
        private ClusterListView _clusterListView;
        [SerializeField]
        private RectTransform _dragArea;
        

        private SignalBus _signalBus;
        private GameplayViewModel _viewModel;
        private WordListAdapter _wordListAdapter;
        private ClusterListAdapter _clusterListAdapter;

        #endregion

        #region public methods

        public override void OnSubscribeToViewModelEvents(CompositeDisposable disposables)
        {
            _viewModel.LevelData
                .Where(levelData => levelData != null)
                .Subscribe(levelData =>
                {
                    _wordListAdapter.UpdateDataset(levelData.Grid);
                    _clusterListAdapter.UpdateDataset(levelData.Clusters);
                })
                .AddTo(disposables);
        }

        #endregion

        #region protected methods

        protected override void OnCreated()
        {
            _viewModel = ViewModelProvider.Get<GameplayViewModel>(this);
            _wordListAdapter = new WordListAdapter(_wordListView);
            _clusterListAdapter = new ClusterListAdapter(_clusterListView, _dragArea);
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