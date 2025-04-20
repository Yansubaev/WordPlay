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
        [SerializeField] private Button _exitButton;
        
        protected override void OnCreated()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnSettingsButtonClicked()
        {
            SignalBus.Fire<OpenSettingsSignal>();
        }

        private void OnExitButtonClicked()
        {
            throw new NotImplementedException();
        }
    }
}
