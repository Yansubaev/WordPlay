using Source.Infrastructure.Signals;
using Source.Presentation.Views;
using UnityEngine;
using Yans.UI.UIScreens;
using Yans.UI.Views;
using Zenject;

namespace Source.Presentation.Screens
{
    public class MainMenuPanel : UIPanel
    {
        [SerializeField] private ButtonView _playButton;
        [SerializeField] private ButtonView _settingsButton;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            _playButton.OnClick += HandlePlayButtonClicked;
            _settingsButton.OnClick += HandleSettingsButtonClicked;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _playButton.OnClick -= HandlePlayButtonClicked;
            _settingsButton.OnClick -= HandleSettingsButtonClicked;
        }

        private void HandlePlayButtonClicked(View view)
        {
            _signalBus.Fire<OpenGameSignal>();
        }

        private void HandleSettingsButtonClicked(View view)
        {
            _signalBus.Fire<OpenSettingsSignal>();
        }
    }
}
