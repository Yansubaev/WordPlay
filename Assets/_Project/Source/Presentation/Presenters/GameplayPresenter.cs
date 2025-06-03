using Source.Presentation.Popups;
using Source.Presentation.Screens;
using Yans.UI;
using Zenject;

namespace Source.Presentation.Presenters
{
    public class GameplayPresenter
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

        public async void ShowGameplayScreen()
        {
            await _uiService.OpenScreen<GameplayPanel>();
        }

        public async void ShowPausePopup()
        {
            await _uiService.OpenScreen<PausePopup>();
        }

        public async void ClosePausePopup()
        {
            await _uiService.CloseTop();
        }

        public void CloseGameplayScreen()
        {
            _uiService.CloseAll();
        }

        #endregion
    }
}