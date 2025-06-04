using Source.Application.Repositories;
using Source.Application.Services;
using Source.Domain.Repositories;
using Source.Domain.Serivces;
using Zenject;

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

        #region private methods

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

        #endregion
    }
}