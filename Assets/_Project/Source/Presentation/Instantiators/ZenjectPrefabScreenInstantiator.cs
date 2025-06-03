using UnityEngine;
using Yans.UI;
using Yans.UI.UIScreens;
using Zenject;

namespace Source.Presentation.Instantiators
{
    public class ZenjectPrefabScreenInstantiator : PrefabScreenInstantiator
    {
        private DiContainer _container;

        [Inject]
        private void Inject(DiContainer container)
        {
            _container = container;
        }

        protected override UIScreen CreateScreenInstance(UIScreen prefab, Transform parent)
        {
            return _container.InstantiatePrefabForComponent<UIScreen>(prefab, parent);
        }
    }
}
