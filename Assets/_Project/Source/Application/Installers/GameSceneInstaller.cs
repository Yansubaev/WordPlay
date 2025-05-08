using Source.Game.Controllers;
using Source.Game.Core;
using Source.Infrastructure.StateMachine.States;
using Source.Infrastructure.UI;
using Source.Signals;
using Source.UI;
using Source.UI.ViewModels;
using UnityEngine;
using Yans.UI;
using Yans.ViewModels;
using Zenject;

namespace Source.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRoot;

        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] GameSceneInstaller.InstallBindings</color>");

            Container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle();
            Container.Bind<IViewModelProvider>().To<ViewModelProvider>().AsSingle();
            Container.Bind<IScreenManager>().To<ZenjectUIScreenManager>().AsSingle();
            Container.Bind<GameplayPresenter>().AsSingle();
            Container.Bind<ILevelLoaderService>().To<LevelLoaderService>().AsSingle();
            Container.Bind<IClusterGameService>().To<ClusterGameService>().AsSingle();
            Container.Bind<IGameProgressService>().To<GameProgressService>().AsSingle();
            Container.Bind<GameController>().AsSingle();

            BindSignals();
        }

        private void BindSignals()
        {
            Container.BindSignal<ShowGameplayScreenSignal>()
                .ToMethod<GameplayPresenter>(x => x.ShowGameplayScreen)
                .FromResolve();

            Container.BindSignal<StartGameSignal>()
                .ToMethod<GameController>(x => x.StartGame)
                .FromResolve();

            Container.BindSignal<PauseGameSignal>()
                .ToMethod<GameplayPresenter>(x => x.ShowPausePopup)
                .FromResolve();
            
            Container.BindSignal<ResumeGameSignal>()
                .ToMethod<GameplayPresenter>(x => x.ClosePausePopup)
                .FromResolve();

            Container.BindSignal<ExitGameSignal>()
                .ToMethod<GameController>(x => x.CloseGame)
                .FromResolve();

            Container.BindSignal<ExitGameSignal>()
                .ToMethod<GameState>(x => x.ReturnToMainMenu)
                .FromResolve();
        }
    }
}
