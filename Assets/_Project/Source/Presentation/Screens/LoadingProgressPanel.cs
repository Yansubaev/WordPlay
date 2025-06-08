using R3;
using Source.Presentation.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Presentation.Panels
{
    public class LoadingProgressPanel : ViewModelPanel
    {
        #region private fields

        [SerializeField]
        private Slider _progressBar;

        [SerializeField]
        private TextMeshProUGUI _progressText;

        private LoadingProgressViewModel _viewModel;
        #endregion

        public override void OnSubscribeToViewModelEvents(CompositeDisposable disposables)
        {
            _viewModel.Progress
                .DistinctUntilChanged()
                .Subscribe(SetProgress)
                .AddTo(disposables);
        }

        #region protected methods

        protected override void OnCreated()
        {
            _viewModel = ViewModelProvider.Get<LoadingProgressViewModel>(this);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            _viewModel.OnFailedToLoad += HandleFailedToLoad;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _viewModel.OnFailedToLoad -= HandleFailedToLoad;
        }

        #endregion

        #region private methods

        private void HandleFailedToLoad()
        {
            Debug.LogError("Failed to load levels. Please check your configuration or network connection.");
            _progressText.text = "Failed to load assets.";
            _progressBar.gameObject.SetActive(false);
        }

        private void SetProgress(float progress)
        {
            _progressBar.value = progress;
            _progressText.text = $"{progress * 100f:0.00}%";
        }

        #endregion
    }
}