using Source.Infrastructure.UI.Transitions;
using Source.UI.Screens;
using Source.UI.Transitions;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers
{
    public class TransitionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] TransitionInstaller.InstallBindings</color>");

            Container.Bind<ITransitionResolver>().To<TransitionResolver>().AsSingle();

            Container.Bind<ITransition>().WithId("Fade").To<TransitionFade>().AsTransient();
        }

        public override void Start()
        {
            var resolver = Container.Resolve<ITransitionResolver>();

            var fade = Container.ResolveId<ITransition>("Fade");

            resolver.Register<MainMenuScreen, SettingsScreen>(fade);
            resolver.Register<SettingsScreen, MainMenuScreen>(fade);
        }

    }
}
