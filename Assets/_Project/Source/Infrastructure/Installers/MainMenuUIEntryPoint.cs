using Source.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.UI
{
    public class MainMenuUIEntryPoint
    {
        private IUIService _uiService;

        [Inject]
        public void Inject(IUIService uiService)
        {
            _uiService = uiService;

            Debug.Log("<color=magenta>MainMenuUIEntryPoint injected</color>");
        }

        public async void ShowMainMenu()
        {
            Debug.Log("<color=blue>MainMenuUIEntryPoint: ShowMainMenu()</color>");
            
            await _uiService.OpenScreen<MainMenuScreen>();
        }
    }

}
