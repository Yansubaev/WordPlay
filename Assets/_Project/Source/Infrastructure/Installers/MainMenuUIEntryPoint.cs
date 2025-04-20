using Source.Infrastructure.Services;
using Source.UI.Screens;
using UnityEngine;
using Zenject;

namespace Source.UI
{
    public class MainMenuPresenter
    {
        private IScreenService _uiService;
        private DiContainer _container;
        private MainMenuScreen _mainMenuScreen;
        private SettingsScreen _settingsScreen;

        [Inject]
        public void Inject(IScreenService uiService, DiContainer container)
        {
            _uiService = uiService;
            _container = container;

            Debug.Log("<color=magenta>MainMenuPresenter injected</color>");
        }

        public async void ShowMainMenu()
        {
            Debug.Log("<color=blue>MainMenuPresenter: ShowMainMenu()</color>");

            _mainMenuScreen = await _uiService.OpenScreen<MainMenuScreen>();

        }

        public async void ShowSettingsScreen()
        {
            Debug.Log("<color=blue>MainMenuPresenter: ShowSettingsScreen()</color>");

            _settingsScreen = await _uiService.OpenScreen<SettingsScreen>();
        }

        public async void CloseSettings()
        {
            Debug.Log("<color=blue>MainMenuPresenter: CloseSettings()</color>");

            if (_settingsScreen != null)
                await _uiService.CloseScreen(_settingsScreen);
        }
    }

}
