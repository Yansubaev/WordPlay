using System;
using Source.Infrastructure.UI;
using Source.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.UI.Screens
{
    public class SettingsScreen : UIScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Toggle _soundsToggle;

        protected override void OnCreated()
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
            SignalBus.Fire<CloseSettingsSignal>();
        }
    }
}
