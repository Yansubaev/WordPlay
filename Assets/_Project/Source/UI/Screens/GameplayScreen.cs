using Source.Infrastructure.UI;
using Source.Infrastructure.UI.Views;
using Source.Signals;
using Source.UI.Views;
using UnityEngine;

namespace Source.UI.Screens
{
    public class GameplayScreen : UIScreen
    {
        [SerializeField] private ImageButtonView _pauseButton;

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
            SignalBus.Fire<PauseGameSignal>();
        }
    }
}
