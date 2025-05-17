using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using Yans.UI.UIScreens;
using Yans.UI.Views;
using Zenject;

namespace Source.UI.Popups
{
    public class PausePopup : UIPopup
    {
        [SerializeField] private ButtonView _resumeButton;
        [SerializeField] private ButtonView _exitButton;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnStarted()
        {
            _resumeButton.OnClick += HandleResumeButtonClicked;
            _exitButton.OnClick += HandleExitButtonClicked;
        }

        protected override void OnStopped()
        {
            _resumeButton.OnClick -= HandleResumeButtonClicked;
            _exitButton.OnClick -= HandleExitButtonClicked;
        }

        private void HandleResumeButtonClicked(View view)
        {
            _signalBus.Fire<ResumeGameSignal>();
        }

        private void HandleExitButtonClicked(View view)
        {
            _signalBus.Fire<ExitGameSignal>();
        }
    }

}

