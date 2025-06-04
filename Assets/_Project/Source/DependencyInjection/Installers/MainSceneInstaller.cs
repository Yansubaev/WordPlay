using Source.Infrastructure.Signals;
using Source.Infrastructure.StateMachine.States;
using Source.Presentation.Presenters;
using Zenject;

namespace Source.DI.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuPresenter>().AsSingle();

            BindSignals();
        }

        private void BindSignals()
        {
            Container.BindSignal<ShowMainMenuSignal>()
                .ToMethod<MainMenuPresenter>(x => x.ShowMainMenu)
                .FromResolve();

            Container.BindSignal<OpenSettingsSignal>()
                .ToMethod<MainMenuPresenter>(x => x.ShowSettingsScreen)
                .FromResolve();

            Container.BindSignal<CloseSettingsSignal>()
                .ToMethod<MainMenuPresenter>(x => x.CloseSettings)
                .FromResolve();

            Container.BindSignal<OpenGameSignal>()
                .ToMethod<MainMenuState>(x => x.StartGame)
                .FromResolve();
        }
    }
}