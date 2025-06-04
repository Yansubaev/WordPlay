using Source.Presentation.Panels;
using Yans.UI;
using Zenject;

namespace Source.Presentation.Presenters
{

    public class MainMenuPresenter
    {
        #region private fields
        private IScreenManager _uiService;
        #endregion

        #region public methods

        [Inject]
        public void Inject(IScreenManager uiService)
        {
            _uiService = uiService;
        }

        public async void ShowMainMenu()
        {
            await _uiService.OpenScreen<MainMenuPanel>();
        }

        public async void ShowSettingsScreen()
        {
            await _uiService.OpenScreen<SettingsPanel>();
        }

        public async void CloseSettings()
        {
            await _uiService.CloseTop();
        }

        #endregion
    }
}