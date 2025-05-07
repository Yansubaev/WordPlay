namespace Yans.UI
{
    internal interface ILifecycleListener
    {
        void OnOwnerCreated();
        void OnOwnerAborted();
    }
}
