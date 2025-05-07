using System;
using Source.Infrastructure.UI;
using Source.Infrastructure.UI.Views;
using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Source.UI.Screens
{
    public class SettingsScreen : UIScreen
    {
        [SerializeField] private ImageButtonView _backButton;
        [SerializeField] private Toggle _soundsToggle;

        protected override void OnStarted()
        {
            _backButton.OnClick += HandleBackButtonClicked;
            _soundsToggle.onValueChanged.AddListener(OnSoundsToggleValueChanged);
        }

        protected override void OnStopped()
        {
            _backButton.OnClick -= HandleBackButtonClicked;
            _soundsToggle.onValueChanged.RemoveListener(OnSoundsToggleValueChanged);
        }

        private void HandleBackButtonClicked(View view)
        {
            SignalBus.Fire<CloseSettingsSignal>();
        }

        private void OnSoundsToggleValueChanged(bool isOn)
        {
            throw new NotImplementedException();
        }
    }
}
