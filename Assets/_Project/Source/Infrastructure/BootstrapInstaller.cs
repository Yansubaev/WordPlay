using Zenject;

namespace Source.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Add your bindings here
            // For example:
            // Container.Bind<IYourService>().To<YourService>().AsSingle();
        }
    }

}
