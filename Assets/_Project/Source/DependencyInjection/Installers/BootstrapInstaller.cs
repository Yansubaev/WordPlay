using Source.Infrastructure.SceneManagement;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Source.DI.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] BootstrapInstaller.InstallBindings</color>");


            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();

            BindStateMachine();
        }

        #region private methods


        private void BindStateMachine()
        {
            Container.Bind<GameStateMachine>().AsSingle();

            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadMainSceneState>().AsSingle();
            Container.Bind<ReturnToMainSceneState>().AsSingle();
            Container.Bind<MainMenuState>().AsSingle();
            Container.Bind<LoadGameSceneState>().AsSingle();
            Container.Bind<GameState>().AsSingle();
        }

        #endregion
    }
}