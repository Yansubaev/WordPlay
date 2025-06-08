using R3;
using Source.Presentation.ViewModels;
using Source.Presentation.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Views;

namespace Source.Presentation.Panels
{
    public class LoadingProgressPanel : ViewModelPanel
    {
        #region private fields

        [SerializeField]
        private Slider _progressBar;

        [SerializeField]
        private TextMeshProUGUI _progressText;

        [SerializeField]
        private TextButtonView _tryAgainButton;

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
            _tryAgainButton.OnClick += HandleRetryButtonClick;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _viewModel.OnFailedToLoad -= HandleFailedToLoad;
            _tryAgainButton.OnClick -= HandleRetryButtonClick;
        }

        #endregion

        #region private methods

        private void HandleRetryButtonClick(View view)
        {
            _tryAgainButton.SetVisibility(false);
            _progressBar.gameObject.SetActive(true);
            _viewModel.RetryLoading();
        }

        private void HandleFailedToLoad()
        {
            _progressText.text = "Failed to load assets.";
            _progressBar.gameObject.SetActive(false);
            _tryAgainButton.SetVisibility(true);
        }

        private void SetProgress(float progress)
        {
            _progressBar.value = progress;
            _progressText.text = $"{progress * 100f:0.00}%";
        }

        #endregion
    }
}