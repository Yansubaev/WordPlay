using Source.Infrastructure.StateMachine.States;
using Source.Signals;
using Source.UI;
using Zenject;

namespace Source.Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        #region public methods

        public override void InstallBindings()
        {
            Container.Bind<MainMenuPresenter>().AsSingle();

            BindSignals();
        }

        #endregion

        #region private methods

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

        #endregion
    }
}