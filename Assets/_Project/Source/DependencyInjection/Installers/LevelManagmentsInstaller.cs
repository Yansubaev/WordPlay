using Source.Application.Services;
using Source.Domain.Serivces;
using Zenject;
using Source.Domain.Repositories;

namespace Source.DI.Installers
{
    public class LevelManagmentsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance("Levels/En/{0}.json").WithId("addressTemplate");

            BindRepositories();
            BindServices();
        }

        private void BindRepositories()
        {
            Container.Bind<ILevelRepository>().To<LevelRepository>().AsSingle();
            Container.Bind<ILevelChainRepository>().To<LevelChainRepository>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IGameProgressService>().To<GameProgressService>().AsSingle();
            Container.Bind<ILevelPreloadingService>().To<LevelPreloadingService>().AsSingle();
            Container.Bind<ILevelLoadingService>().To<LevelLoadingService>().AsSingle();
        }
    }
}