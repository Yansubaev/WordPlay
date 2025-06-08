using Source.Presentation.Presenters;
using Zenject;

namespace Source.DI.Installers
{
    public class LoadingSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadingProgressPresenter>().AsSingle();
        }

        public override void Start()
        {
            base.Start();
            Container.Resolve<LoadingProgressPresenter>().ShowLoadingProgress();
        }
    }
}