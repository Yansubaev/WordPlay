using Cysharp.Threading.Tasks;
using Source.Infrastructure.Services;
using Source.Infrastructure.UI;
using Source.Signals;
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
            // Container.Bind<UIService>().AsSingle();
            Container.Bind<IUIService>().To<UIService>().AsSingle();
            Container.Bind<MainMenuUIEntryPoint>().AsSingle();

            Container.BindSignal<ShowMainMenuSignal>()
                .ToMethod<MainMenuUIEntryPoint>(x => x.ShowMainMenu)
                .FromResolve();
        }
    }
}
