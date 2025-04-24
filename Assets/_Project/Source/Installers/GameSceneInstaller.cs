using Source.Infrastructure.Services;
using Source.Infrastructure.UI;
using UnityEngine;
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
            Container.Bind<IScreenService>().To<UIScreenService>().AsSingle();
        }
    }
}
