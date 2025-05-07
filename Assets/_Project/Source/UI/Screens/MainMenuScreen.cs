using Source.Signals;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Screen;
using Zenject;

namespace Source.UI.Screens
{
    public class MainMenuScreen : UIPanel
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnCreated()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            _signalBus.Fire<OpenGameSignal>();
        }

        private void OnSettingsButtonClicked()
        {
            _signalBus.Fire<OpenSettingsSignal>();
        }
    }
}
