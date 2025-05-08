using System;
using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Screen;
using Zenject;

namespace Source.UI.Screens
{
    public class SettingsScreen : UIPanel
    {
        [SerializeField] private ImageButtonView _backButton;
        [SerializeField] private Toggle _soundsToggle;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

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
