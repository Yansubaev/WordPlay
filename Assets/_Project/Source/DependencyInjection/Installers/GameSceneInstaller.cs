using Source.Application.Services;
using Source.Domain.Serivces;
using Source.Application.Controllers;
using Source.Infrastructure.Signals;
using Source.Infrastructure.StateMachine.States;
using Source.Presentation.Presenters;
using Zenject;

namespace Source.DI.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameplayPresenter>().AsSingle();
            Container.Bind<ILevelLoadingService>().To<LevelLoadingService>().AsSingle();
            Container.Bind<IClusterGameService>().To<ClusterGameService>().AsSingle();
            Container.Bind<GameSessionManager>().AsSingle();

            BindSignals();
        }

        private void BindSignals()
        {
            Container.BindSignal<StartGameSignal>()
                .ToMethod<GameplayPresenter>(x => x.ShowGameplayScreen)
                .FromResolve();

            Container.BindSignal<StartGameSignal>()
                .ToMethod<GameSessionManager>(x => x.StartGame)
                .FromResolve();

            Container.BindSignal<PauseGameSignal>()
                .ToMethod<GameplayPresenter>(x => x.ShowPausePopup)
                .FromResolve();

            Container.BindSignal<ResumeGameSignal>()
                .ToMethod<GameplayPresenter>(x => x.ClosePausePopup)
                .FromResolve();

            Container.BindSignal<ExitGameSignal>()
                .ToMethod<GameSessionManager>(x => x.CloseGame)
                .FromResolve();

            Container.BindSignal<ExitGameSignal>()
                .ToMethod<GameplayPresenter>(x => x.CloseGameplayScreen)
                .FromResolve();

            Container.BindSignal<ExitGameSignal>()
                .ToMethod<GameState>(x => x.ReturnToMainMenu)
                .FromResolve();
        }
    }
}