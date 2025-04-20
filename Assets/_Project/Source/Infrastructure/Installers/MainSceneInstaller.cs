using Cysharp.Threading.Tasks;
using Source.Infrastructure.Services;
using Source.Infrastructure.UI;
using Source.Signals;
using Source.UI;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers
{

    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRoot;

        public static UniTaskCompletionSource SceneLoaded { get; private set; }

        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] MainSceneInstaller.InstallBindings</color>");

            SceneLoaded = new UniTaskCompletionSource();

            Container.Bind<UIRootLoader>().AsSingle();
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
        }
    }
}
