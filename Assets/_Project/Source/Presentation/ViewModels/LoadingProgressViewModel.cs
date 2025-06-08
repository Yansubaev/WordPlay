using R3;
using Source.Infrastructure.Signals;
using System;
using Yans.ViewModels;
using Zenject;

namespace Source.Presentation.ViewModels
{
    public class LoadingProgressViewModel : ViewModel
    {
        public ReadOnlyReactiveProperty<float> Progress => _progress;

        #region private fields
        private ReactiveProperty<float> _progress = new(0f);
        private SignalBus _signalBus;
        #endregion

        public event Action OnFailedToLoad;

        public LoadingProgressViewModel(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        #region protected methods

        protected override void OnCreated()
        {
            _signalBus.Subscribe<LoadingProgressSignal>(OnLoadingProgressSignalReceived);
        }

        protected override void OnAborted()
        {
            _signalBus.TryUnsubscribe<LoadingProgressSignal>(OnLoadingProgressSignalReceived);
        }

        #endregion

        private void OnLoadingProgressSignalReceived(LoadingProgressSignal signal)
        {
            if (signal == null)
            {
                throw new ArgumentNullException(nameof(signal), "LoadingProgressSignal cannot be null");
            }

            if (signal.ProgressNomalized < 0f || signal.ProgressNomalized > 1f)
            {
                OnFailedToLoad?.Invoke();
                return;
            }

            _progress.Value = signal.ProgressNomalized;
        }
    }
}