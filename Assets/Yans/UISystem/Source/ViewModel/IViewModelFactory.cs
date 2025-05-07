using Yans.UI;

namespace Yans.ViewModels
{
    public interface IViewModelFactory<VM> where VM : ViewModel
    {
        VM GetViewModel(ILifecycleOwner lifecycleOwner);
    }
}
