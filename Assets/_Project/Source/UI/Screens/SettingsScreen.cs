using System;
using Source.Signals;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Screen;
using Zenject;

namespace Source.UI.Screens
{
    public class SettingsScreen : UIPanel
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Toggle _soundsToggle;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnStarted()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _soundsToggle.onValueChanged.AddListener(OnSoundsToggleValueChanged);
        }

        private void OnSoundsToggleValueChanged(bool isOn)
        {
            throw new NotImplementedException();
        }

        private void OnBackButtonClicked()
        {
            Debug.Log("<color=blue>SettingsScreen: OnBackButtonClicked()</color>");
            _signalBus.Fire<CloseSettingsSignal>();
        }
    }
}
