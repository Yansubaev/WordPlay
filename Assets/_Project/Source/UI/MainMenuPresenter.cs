using Source.UI.Popups;
using Source.UI.Screens;
using UnityEngine;
using Yans.UI;
using Zenject;

namespace Source.UI
{
    public class MainMenuPresenter
    {
        private IScreenManager _uiService;

        [Inject]
        public void Inject(IScreenManager uiService)
        {
            _uiService = uiService;
        }

        public async void ShowMainMenu()
        {
            await _uiService.OpenPanel<MainMenuPanel>();
        }

        public async void ShowSettingsScreen()
        {
            await _uiService.OpenPanel<SettingsPanel>();
        }

        public async void CloseSettings()
        {
            await _uiService.CloseTop();
        }
    }

    public class GameplayPresenter
    {
        private IScreenManager _uiService;
        private GameplayPanel _gameplayScreen;
        private PausePopup _pausePopup;

        [Inject]
        public void Inject(IScreenManager uiService)
        {
            _uiService = uiService;
        }

        public async void ShowGameplayScreen()
        {
            _gameplayScreen = await _uiService.OpenPanel<GameplayPanel>();
        }

        public async void ShowPausePopup()
        {
            _pausePopup = await _uiService.OpenPopup<PausePopup>();
        }

        public async void ClosePausePopup()
        {
            if (_pausePopup != null)
            {
                await _uiService.ClosePopup(_pausePopup);
                _pausePopup = null;
            }
        }
    }

}
