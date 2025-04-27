using System;
using Source.Infrastructure.UI;
using Source.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Screens
{
    public class GameplayScreen : UIScreen
    {
        [SerializeField] private Button _pauseButton;

        protected override void OnStarted()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            SignalBus.Fire<PauseGameSignal>();
        }
    }
}
