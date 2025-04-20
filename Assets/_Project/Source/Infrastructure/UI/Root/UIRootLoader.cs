using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Infrastructure.UI
{

    /// <summary>
    /// Loads the UI root from the addressable assets.
    /// </summary>
    public class UIRootLoader
    {
        private readonly string _uiRootAddress = "UI/Root/UIRoot.prefab";

        public async UniTask<GameObject> LoadUIRoot(Transform parent = null)
        {
            Debug.Log("UIRootLoader.LoadUIRoot");
            var handle = Addressables.InstantiateAsync(_uiRootAddress, parent);
            return await handle.ToUniTask();
        }
    }
}