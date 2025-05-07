using Source.Infrastructure.StateMachine.States;
using Source.Infrastructure.UI;
using Source.Signals;
using Source.UI;
using UnityEngine;
using Yans.UI;
using Zenject;

namespace Source.Installers
{

    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRoot;

        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] MainSceneInstaller.InstallBindings</color>");

            // Container.Bind<UIRootLoader>().AsSingle();
            Container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle();
            Container.Bind<IScreenService>().To<UIScreenService>().AsSingle();
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
