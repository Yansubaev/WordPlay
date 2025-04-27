using System;
using Source.Infrastructure.UI;
using Source.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Popups
{
    public class PausePopup : UIPopup
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _exitButton;

        protected override void OnStarted()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            SignalBus.Fire<ExitGameSignal>();
        }

        private void OnResumeButtonClicked()
        {
            SignalBus.Fire<ResumeGameSignal>();
        }

    }

}

