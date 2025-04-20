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

        protected override void OnCreated()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            Debug.Log("<color=blue>SettingsScreen: OnBackButtonClicked()</color>");
            SignalBus.Fire<CloseSettingsSignal>();
        }
    }
}
