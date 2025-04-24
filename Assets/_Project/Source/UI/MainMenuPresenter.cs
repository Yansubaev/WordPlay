using Source.Infrastructure.Services;
using Source.UI.Screens;
using UnityEngine;
using Zenject;

namespace Source.UI
{
    public class MainMenuPresenter
    {
        private IScreenService _uiService;
        private MainMenuScreen _mainMenuScreen;
        private SettingsScreen _settingsScreen;

        [Inject]
        public void Inject(IScreenService uiService)
        {
            _uiService = uiService;
        }

        public async void ShowMainMenu()
        {
            _mainMenuScreen = await _uiService.OpenScreen<MainMenuScreen>();
        }

        public async void ShowSettingsScreen()
        {
            _settingsScreen = await _uiService.OpenScreen<SettingsScreen>();
        }

        public async void CloseSettings()
        {
            if (_settingsScreen != null)
            {
                await _uiService.CloseScreen(_settingsScreen);
                _settingsScreen = null;
            }
            else
            {
                Debug.LogWarning("<color=red>Settings screen is null</color>");
            }
        }
    }

}
