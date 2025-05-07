using Source.Signals;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Screen;
using Zenject;

namespace Source.UI.Popups
{
    public class PausePopup : UIPopup
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _exitButton;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnStarted()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            _signalBus.Fire<ExitGameSignal>();
        }

        private void OnResumeButtonClicked()
        {
            _signalBus.Fire<ResumeGameSignal>();
        }

    }

}

