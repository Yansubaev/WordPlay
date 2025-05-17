using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Yans.UI.UIScreens;

namespace Yans.UI
{
    public class AddressablesScreenInstantiator : ScreenInstantiator
    {
        #region private fields

        [SerializeField]
        private List<AssetReference> _screenAssetReference;


        #endregion

        #region public methods

        public override async UniTask<T> InstantiateScreen<T>(Transform parent, ScreenOrientation screenOrientation)
        {
            var reference = _screenAssetReference.FirstOrDefault(x => x.AssetGUID.Contains(typeof(T).Name));
            if (reference == null)
                throw new ArgumentException($"Screen of type {typeof(T).Name} not found in references");

            var instance = await reference.InstantiateAsync(parent);
            var screen = instance.GetComponent<T>();
            if (screen == null)
                throw new ArgumentException($"Instantiated object does not contain component of type {typeof(T).Name}");

            return screen;
        }
        public override void CleanUpScreen(UIScreen uiScreen)
        {
            Addressables.ReleaseInstance(uiScreen.gameObject);
        }

        public override UniTask<UIScreen> InstantiateScreen(Type type, Transform parent, ScreenOrientation screenOrientation)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}