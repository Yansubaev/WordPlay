using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.Infrastructure.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Source.Infrastructure.Services
{

    /// <summary>
    /// This class is responsible for managing the UI elements in the game.
    /// </summary>
    public class UIService : IUIService
    {
        private IDictionary<string, UIScreen> _screens = new Dictionary<string, UIScreen>();
        private string _addressTemplate = "UI/Screens/{0}.prefab";
        private UIRoot _uiRoot;

        [Inject]
        public void Inject(UIRoot uIRoot)
        {
            _uiRoot = uIRoot;
            Debug.Log($"<color=magenta>UIService.Inject(UIRoot: {uIRoot.name})</color>");
        }

        // public UIService(UIRoot uIRoot)
        // {
        //     Debug.Log($"<color=magenta>UIService.Constructor(UIRoot: {uIRoot.name})</color>");

        //     _uiRoot = uIRoot;
        // }

        public async UniTask<T> OpenScreen<T>() where T : UIScreen
        {
            Debug.Log($"<color=magenta>UIService.OpenScreen({typeof(T).Name})</color>");
            
            var address = string.Format(_addressTemplate, typeof(T).Name);
            var handle = Addressables.InstantiateAsync(address);
            var screenObject = await handle.ToUniTask();
            var screen = screenObject.GetComponent<T>();

            screen.transform.SetParent(_uiRoot.ScreensRoot, false);
            screen.transform.localPosition = Vector3.zero;
            screen.transform.localScale = Vector3.one;

            _screens.Add(typeof(T).Name, screen);

            return screen;
        }

        public UniTask CloseScreen(UIScreen screen)
        {
            if (_screens.TryGetValue(screen.GetType().Name, out var existingScreen))
            {
                _screens.Remove(existingScreen.GetType().Name);
                Addressables.Release(screen.gameObject);
                return UniTask.CompletedTask;
            }
            else
            {
                Debug.LogWarning($"Screen {screen.GetType().Name} is not open.");
                return UniTask.CompletedTask;
            }
        }
    }
}
