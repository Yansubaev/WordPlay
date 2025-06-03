using Source.Application.Services;
using Source.Game.Core;
using Source.Infrastructure.SceneManagement;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
using Source.Signals;
using UnityEngine;
using Zenject;

namespace Source.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] BootstrapInstaller.InstallBindings</color>");

            SignalBusInstaller.Install(Container);

            DeclareSignals();

            Container.BindInstance("Levels/En/{0}.json").WithId("addressTemplate");

            Container.Bind<ILevelChainRepository>().To<LevelChainRepository>().AsTransient();
            Container.Bind<ILevelPreloadingService>().To<LevelPreloadingService>().AsSingle();
            Container.Bind<IGameProgressService>().To<GameProgressService>().AsSingle();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();

            BindStates();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<ShowMainMenuSignal>();
            Container.DeclareSignal<OpenSettingsSignal>();
            Container.DeclareSignal<CloseSettingsSignal>();
            Container.DeclareSignal<OpenGameSignal>();
            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<ExitGameSignal>();
            Container.DeclareSignal<ShowGameplayScreenSignal>();
            Container.DeclareSignal<PauseGameSignal>();
            Container.DeclareSignal<ResumeGameSignal>();
        }

        private void BindStates()
        {
            Container.Bind<BootstrapState>().AsTransient();
            Container.Bind<LoadMainSceneState>().AsTransient();
            Container.Bind<MainMenuState>().AsTransient();
            Container.Bind<LoadGameSceneState>().AsTransient();
            Container.Bind<GameState>().AsTransient();
        }
    }

}
