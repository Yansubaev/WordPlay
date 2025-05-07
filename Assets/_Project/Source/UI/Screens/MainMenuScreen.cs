using Source.Infrastructure.UI;
using Source.Infrastructure.UI.Views;
using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using Zenject;

namespace Source.UI.Screens
{
    public class MainMenuScreen : UIScreen
    {
        [SerializeField] private ButtonView _playButton;
        [SerializeField] private ButtonView _settingsButton;

        protected override void OnStarted()
        {
            _playButton.OnClick += HandlePlayButtonClicked;
            _settingsButton.OnClick += HandleSettingsButtonClicked;
        }

        protected override void OnStopped()
        {
            _playButton.OnClick -= HandlePlayButtonClicked;
            _settingsButton.OnClick -= HandleSettingsButtonClicked;
        }

        private void HandlePlayButtonClicked(View view)
        {
            SignalBus.Fire<OpenGameSignal>();
        }

        private void HandleSettingsButtonClicked(View view)
        {
            SignalBus.Fire<OpenSettingsSignal>();
        }
    }
}
