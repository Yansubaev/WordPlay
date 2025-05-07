using Source.Signals;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Screen;
using Zenject;

namespace Source.UI.Screens
{
    public class GameplayScreen : UIPanel
    {
        [SerializeField] private Button _pauseButton;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnStarted()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            _signalBus.Fire<PauseGameSignal>();
        }
    }
}
