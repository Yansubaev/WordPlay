namespace Yans.ViewModels
{
    public abstract class ViewModel
    {
        private ViewModel() { }

        #region public methods

        public void Create()
        {
            OnCreated();
        }

        public void Abort()
        {
            OnAborted();
        }

        #endregion

        #region protected methods
        protected virtual void OnCreated() { }

        protected virtual void OnAborted() { }
        #endregion
    }
}