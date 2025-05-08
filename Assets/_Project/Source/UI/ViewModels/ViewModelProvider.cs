using System.Collections.Generic;
using Yans.UI;
using Yans.ViewModels;
using Zenject;

namespace Source.UI.ViewModels
{
    public class ViewModelProvider : IViewModelProvider
    {
        private DiContainer _container;
        private Dictionary<string, ViewModel> _instantiatedViewModels = new();

        public ViewModelProvider(DiContainer container)
        {
            _container = container;
        }

        public T GetViewModel<T>(IViewModelOwner viewModelOwner) where T : ViewModel
        {
            if (_instantiatedViewModels.TryGetValue(viewModelOwner.GetInstanceId(), out var viewModel))
            {
                return (T)viewModel;
            }

            viewModel = _container.Instantiate<T>();
            _instantiatedViewModels[viewModelOwner.GetInstanceId()] = viewModel;
            viewModel.Create();
            return (T)viewModel;
        }

        public void ClearViewModel(IViewModelOwner viewModelOwner)
        {
            if (_instantiatedViewModels.TryGetValue(viewModelOwner.GetInstanceId(), out var viewModel))
            {
                viewModel.Abort();
                _instantiatedViewModels.Remove(viewModelOwner.GetInstanceId());
            }
        }

    }
}
