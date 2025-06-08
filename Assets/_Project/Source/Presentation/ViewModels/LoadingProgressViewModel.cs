using R3;
using Source.Infrastructure.Signals;
using System;
using Yans.ViewModels;
using Zenject;

namespace Source.Presentation.ViewModels
{
    public class LoadingProgressViewModel : ViewModel
    {
        public event Action OnFailedToLoad;

        public ReadOnlyReactiveProperty<float> Progress => _progress;

        #region private fields
        private ReactiveProperty<float> _progress = new(0f);
        private SignalBus _signalBus;
        #endregion

        public LoadingProgressViewModel(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void RetryLoading()
        {
            _signalBus.Fire<RetryLoadingSignal>();
        }

        #region protected methods

        protected override void OnCreated()
        {
            _signalBus.SubscribeId<float>("LoadingProgress", OnLoadingProgressSignalReceived);
        }

        protected override void OnAborted()
        {
            _signalBus.TryUnsubscribeId<float>("LoadingProgress", OnLoadingProgressSignalReceived);
        }

        #endregion

        private void OnLoadingProgressSignalReceived(float progress)
        {
            if (progress < 0f || progress > 1f)
            {
                OnFailedToLoad?.Invoke();
                return;
            }

            _progress.Value = progress;
        }
    }
}