using Yans.UI;

namespace Yans.ViewModels
{
    public abstract class ViewModel : ILifecycleListener
    {
        private ILifecycleOwner _lifecycleOwner;

        public ViewModel(ILifecycleOwner lifecycleOwner)
        {
            _lifecycleOwner = lifecycleOwner;
            _lifecycleOwner.AddLifecycleListener(this);
        }

        public void OnOwnerCreated()
        {
            OnCreated();
        }

        public void OnOwnerAborted()
        {
            OnAborted();
        }

        protected virtual void OnCreated() { }

        protected virtual void OnAborted() { }

    }
}
