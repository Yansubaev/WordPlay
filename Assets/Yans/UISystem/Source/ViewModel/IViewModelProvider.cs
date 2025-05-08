using Yans.UI;

namespace Yans.ViewModels
{
    public interface IViewModelProvider
    {
        VM GetViewModel<VM>(IViewModelOwner lifecycleOwner) where VM : ViewModel;
        void ClearViewModel(IViewModelOwner lifecycleOwner);
    }
}
