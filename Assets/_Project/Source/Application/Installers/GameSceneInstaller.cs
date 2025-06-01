using Source.Game.Controllers;
using Source.Game.Core;
using Source.Infrastructure.StateMachine.States;
using Source.Signals;
using Source.UI;
using UnityEngine;
using Zenject;

namespace Source.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        #region public methods

        public override void InstallBindings()
        {
            Container.Bind<GameplayPresenter>().AsSingle();
            Container.Bind<ILevelLoaderService>()
                .To<LevelLoaderService>()
                .FromMethod(() =>
                {
                    var addressTemplate = "Levels/En/{0}.json";
                    return new LevelLoaderService(addressTemplate);
                })
                .AsSingle();
            Container.Bind<IClusterGameService>().To<ClusterGameService>().AsSingle();
            Container.Bind<IGameProgressService>().To<GameProgressService>().AsSingle();
            Container.Bind<GameSessionManager>().AsSingle();

            BindSignals();
        }

        #endregion

        #region private methods

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

        #endregion
    }
}