using Source.Signals;
using Source.UI.Views;
using UnityEngine;
using Yans.UI.Screen;
using Yans.UI.Views;
using Zenject;

namespace Source.UI.Screens
{
    public class GameplayPanel : UIPanel
    {
        [SerializeField] private ImageButtonView _pauseButton;

        private SignalBus _signalBus;

        [Inject]
        private void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void OnStarted()
        {
            _pauseButton.OnClick += HandlePauseButtonClicked;
        }

        protected override void OnStopped()
        {
            _pauseButton.OnClick -= HandlePauseButtonClicked;
        }

        private void HandlePauseButtonClicked(View view)
        {
            _signalBus.Fire<PauseGameSignal>();
        }
    }
}
