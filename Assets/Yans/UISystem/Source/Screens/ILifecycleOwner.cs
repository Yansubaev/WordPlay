namespace Yans.UI
{
    public interface ILifecycleOwner
    {
        internal void AddLifecycleListener(ILifecycleListener lifecycleListener);
    }
}
