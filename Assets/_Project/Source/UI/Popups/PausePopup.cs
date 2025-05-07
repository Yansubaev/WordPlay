using System;
using Source.Infrastructure.UI;
using Source.Infrastructure.UI.Views;
using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Popups
{
    public class PausePopup : UIPopup
    {
        [SerializeField] private ButtonView _resumeButton;
        [SerializeField] private ButtonView _exitButton;

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
            SignalBus.Fire<ResumeGameSignal>();
        }

        private void HandleExitButtonClicked(View view)
        {
            SignalBus.Fire<ExitGameSignal>();
        }
    }

}

