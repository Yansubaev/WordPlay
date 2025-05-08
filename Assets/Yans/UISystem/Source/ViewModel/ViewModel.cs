namespace Yans.ViewModels
{
    public abstract class ViewModel
    {
        private ViewModel() { }

        public void Create()
        {
            OnCreated();
        }

        public void Abort()
        {
            OnAborted();
        }

        protected virtual void OnCreated() { }

        protected virtual void OnAborted() { }

    }
}
