using System;
using Source.Infrastructure.UI;
using Source.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.UI.Screens
{
    public class MainMenuScreen : UIScreen
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        
        protected override void OnCreated()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            SignalBus.Fire<OpenGameSignal>();
        }

        private void OnSettingsButtonClicked()
        {
            SignalBus.Fire<OpenSettingsSignal>();
        }
    }
}
