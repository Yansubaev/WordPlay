using Cysharp.Threading.Tasks;
using UnityEngine;
using Yans.UI;
using Yans.UI.Transitions;
using Yans.ViewModels;
using Zenject;

namespace Source.Infrastructure.UI
{
    public class ZenjectUIScreenManager : UIScreenManager
    {
        private DiContainer _container;

        public ZenjectUIScreenManager(
            UIRoot uIRoot,
            ITransitionResolver transitionResolver,
            IViewModelProvider viewModelProvider,
            DiContainer diContainer) : base(
                uIRoot,
                transitionResolver,
                viewModelProvider)
        {
            _container = diContainer;
        }

        protected override async UniTask<T> InstantiateScreenPrefab<T>(GameObject prefab, Transform parent)
        {
            var instance = (await Object.InstantiateAsync(prefab, parent))[0];
            await UniTask.Yield();
            var component = _container.InjectGameObjectForComponent<T>(instance.gameObject);
            return component;
        }

    }
}
