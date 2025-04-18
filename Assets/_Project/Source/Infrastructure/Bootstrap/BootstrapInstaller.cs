using Source.Infrastructure.SceneManagement;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
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

            Container.Bind<SceneLoaderService>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();

            Container.Bind<BootstrapState>().AsTransient();
            Container.Bind<LoadMainMenuState>().AsTransient();
        }
    }

}
