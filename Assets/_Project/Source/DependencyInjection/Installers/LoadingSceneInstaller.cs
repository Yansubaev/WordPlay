using Source.Infrastructure.Signals;
using Source.Infrastructure.StateMachine.States;
using Source.Presentation.Presenters;
using Zenject;

namespace Source.DI.Installers
{
    public class LoadingSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadingProgressPresenter>().AsSingle();

            Container.BindSignal<RetryLoadingSignal>()
                .ToMethod<LoadMainSceneState>(x => x.RetryLoading)
                .FromResolve();
        }

        public override void Start()
        {
            base.Start();
            Container.Resolve<LoadingProgressPresenter>().ShowLoadingProgress();
        }
    }
}