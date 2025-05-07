using Yans.UI;
using Yans.ViewModels;
using Zenject;

namespace Source.UI.ViewModels
{
    public class ViewModelFactory<VM> : IViewModelFactory<VM> where VM : ViewModel
    {
        private DiContainer _container;

        public ViewModelFactory(DiContainer container)
        {
            _container = container;
        }

        public VM GetViewModel(ILifecycleOwner lifecycleOwner)
        {
            return _container.Instantiate<VM>(new object[] { lifecycleOwner });
        }

    }
}
