using R3;
using Yans.UI.UIScreens;

namespace Source.UI.Screens
{
    public abstract class ViewModelPanel : UIPanel
    {
        #region private fields
        private CompositeDisposable _disposables;
        #endregion

        #region public methods
        public abstract void OnSubscribeToViewModelEvents(CompositeDisposable disposables);
        #endregion

        #region protected methods

        protected override void OnStarted()
        {
            base.OnStarted();
            _disposables = new CompositeDisposable();
            OnSubscribeToViewModelEvents(_disposables);
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            _disposables?.Dispose();
            _disposables = null;
        }

        #endregion
    }
}