using System.Collections.Generic;
using Source.Infrastructure.UI.Transitions;
using Source.UI.Transitions;
using UnityEngine;
using Zenject;

namespace Source.Installers
{
    public class TransitionInstaller : MonoInstaller
    {
        [SerializeField] private TransitionConfig _config;

        public override void InstallBindings()
        {
            Debug.Log("<color=green>[ZEN] TransitionInstaller.InstallBindings</color>");

            var registry = new Dictionary<TransitionType, ITransition>
            {
                { TransitionType.Fade, new TransitionFade() },
                { TransitionType.SlideLeft, new TransitionSlideLeft() },
                { TransitionType.SlideRight, new TransitionSlideRight() },
            };

            Container.Bind<ITransitionResolver>().To<ConfigBasedTransitionResolver>().AsSingle()
                .WithArguments(_config, registry);
        }
    }
}
