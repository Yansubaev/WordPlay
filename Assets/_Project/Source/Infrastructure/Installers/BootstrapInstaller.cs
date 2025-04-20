using Source.Infrastructure.SceneManagement;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
using Source.Signals;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] BootstrapInstaller.InstallBindings</color>");

            SignalBusInstaller.Install(Container);

            DeclareSignals();

            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();

            BindStates();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<ShowMainMenuSignal>();
            Container.DeclareSignal<OpenSettingsSignal>();
            Container.DeclareSignal<CloseSettingsSignal>();
            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<ExitGameSignal>();
        }

        private void BindStates()
        {
            Container.Bind<BootstrapState>().AsTransient();
            Container.Bind<LoadMainSceneState>().AsTransient();
            Container.Bind<MainMenuState>().AsTransient();

        }
    }

}
