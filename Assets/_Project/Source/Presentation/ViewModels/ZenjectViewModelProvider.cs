using Yans.ViewModels;
using Zenject;

namespace Source.Presentation.ViewModels
{
    public class ZenjectViewModelProvider : ViewModelProvider
    {
        private DiContainer _container;

        public ZenjectViewModelProvider(DiContainer container)
        {
            _container = container;
        }

        protected override V CreateViewModelInstance<V>()
        {
            return _container.Instantiate<V>();
        }
    }
}