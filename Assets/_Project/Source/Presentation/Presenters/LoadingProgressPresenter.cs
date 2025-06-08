using Cysharp.Threading.Tasks;
using Source.Presentation.Panels;
using Yans.UI;
using Zenject;

namespace Source.Presentation.Presenters
{
    public class LoadingProgressPresenter
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

        public void ShowLoadingProgress()
        {
            _uiService.OpenScreen<LoadingProgressPanel>().Forget();
        }

        #endregion
    }
}