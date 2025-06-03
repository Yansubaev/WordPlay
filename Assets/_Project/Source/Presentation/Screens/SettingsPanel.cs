using System;
using Source.Signals;
using Source.Presentation.Views;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.UIScreens;
using Yans.UI.Views;
using Zenject;

namespace Source.Presentation.Screens
{
    public class SettingsPanel : UIPanel
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
            base.OnStarted();
            _backButton.OnClick += HandleBackButtonClicked;
            _soundsToggle.onValueChanged.AddListener(OnSoundsToggleValueChanged);
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _backButton.OnClick -= HandleBackButtonClicked;
            _soundsToggle.onValueChanged.RemoveListener(OnSoundsToggleValueChanged);
        }

        private void HandleBackButtonClicked(View view)
        {
            _signalBus.Fire<CloseSettingsSignal>();
        }

        private void OnSoundsToggleValueChanged(bool isOn)
        {
            throw new NotImplementedException();
        }
    }
}
