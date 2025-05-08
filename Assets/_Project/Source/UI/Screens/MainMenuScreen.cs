using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Screen;
using Zenject;

namespace Source.UI.Screens
{
    public class MainMenuScreen : UIPanel
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
            _signalBus.Fire<OpenGameSignal>();
        }

        private void HandleSettingsButtonClicked(View view)
        {
            _signalBus.Fire<OpenSettingsSignal>();
        }
    }
}
