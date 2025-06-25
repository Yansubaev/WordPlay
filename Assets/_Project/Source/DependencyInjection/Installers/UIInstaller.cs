using Source.Presentation.Instantiators;
using Source.Presentation.Transitions;
using Source.Presentation.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using Yans.UI;
using Yans.UI.Transitions;
using Yans.ViewModels;
using Zenject;

namespace Source.DI.Installers
{
    public class UIInstaller : MonoInstaller
    {
        #region private fields

        [SerializeField]
        private UIRoot _uiRoot;

        [SerializeField]
        private ZenjectPrefabScreenInstantiator _screenInstantiator;

        [SerializeField]
        private TransitionConfig _config;

        #endregion

        public override void InstallBindings()
        {
            BindTransitions();

            Container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle();
            Container.Bind<IScreenInstantiator>().To<ZenjectPrefabScreenInstantiator>()
                .FromInstance(_screenInstantiator).AsSingle();
            Container.Bind<IViewModelProvider>().To<ZenjectViewModelProvider>().AsSingle();
            Container.Bind<IScreenManager>().To<UIScreenManager>().AsSingle();

        }

        private void BindTransitions()
        {
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