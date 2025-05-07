using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Yans.UI;
using Yans.UI.Screen;
using Yans.UI.Transitions;
using Zenject;

namespace Source.Infrastructure.UI
{

    /// <summary>
    /// This class is responsible for managing the UI panels in the game.
    /// </summary>
    public class UIScreenService : IScreenService
    {
        private const string ScreenAddressTemplate = "UI/Screens/{0}.prefab";
        private const string PopupAddressTemplate = "UI/Popups/{0}.prefab";

        private readonly Dictionary<string, UIPanel> _screens = new();
        private readonly List<UIPanel> _screenStack = new();
        private readonly List<UIPopup> _popupStack = new();
        private readonly List<UIScreen> _generalStack = new();
        private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _popupHandles = new();
        private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _screenHandles = new();

        private DiContainer _container;
        private UIRoot _uiRoot;
        private ITransitionResolver _transitionResolver;

        [Inject]
        public void Inject(
            DiContainer container,
            UIRoot uIRoot,
            ITransitionResolver transitionResolver)
        {
            _container = container;
            _uiRoot = uIRoot;
            _transitionResolver = transitionResolver;
        }

        public async UniTask<T> OpenScreen<T>() where T : UIPanel
        {
            string address = string.Format(ScreenAddressTemplate, typeof(T).Name);

            var prefabHandle = Addressables.LoadAssetAsync<GameObject>(address);
            _screenHandles[typeof(T).Name] = prefabHandle;

            var prefab = (await prefabHandle).GetComponent<T>();
            var newScreen = _container.InstantiatePrefabForComponent<T>(prefab);

            newScreen.transform.SetParent(_uiRoot.ScreenRoot, false);
            newScreen.transform.localPosition = Vector3.zero;
            newScreen.transform.localScale = Vector3.one;
            newScreen.Create();

            UIPanel previous = _screenStack.Count > 0 ? _screenStack.LastOrDefault() : null;

            if (previous != null)
                previous.PauseLifecycle();

            var transition = _transitionResolver.Resolve(previous?.GetType(), typeof(T));
            await transition.Play(previous, newScreen);

            if (previous != null)
                previous.StopLifecycle();

            _screenStack.Add(newScreen);
            _screens[typeof(T).Name] = newScreen;

            newScreen.Canvas.enabled = true;
            newScreen.StartLifecycle();

            _generalStack.Add(newScreen);
            return newScreen;
        }

        public UniTask CloseTop()
        {
            if (_generalStack.Count == 0)
                return UniTask.CompletedTask;

            var topScreen = _generalStack.LastOrDefault();

            if (topScreen is UIPanel screen)
            {
                return CloseScreen(screen);
            }
            else if (topScreen is UIPopup popup)
            {
                return ClosePopup(popup);
            }

            throw new System.Exception("Unknown screen type in stack.");
        }

        public async UniTask CloseScreen(UIPanel screen)
        {
            if (!_screens.TryGetValue(screen.GetType().Name, out var existingScreen))
            {
                Debug.LogWarning($"Screen {screen.GetType().Name} is not open.");
                return;
            }

            bool isTop = _screenStack.Last() == screen;

            UIPanel newTop = null;
            _screenStack.Remove(screen);

            newTop = _screenStack.Count > 0 ? _screenStack.LastOrDefault() : null;

            _screens.Remove(screen.GetType().Name);

            screen.PauseLifecycle();

            if (newTop != null)
            {
                newTop.StartLifecycle();
            }

            if (isTop)
            {
                newTop.Canvas.enabled = true;
                var transition = _transitionResolver.Resolve(screen.GetType(), newTop?.GetType());
                await transition.Play(screen, newTop);
            }

            screen.StopLifecycle();
            screen.Close();

            _generalStack.Remove(screen);

            string screenName = screen.GetType().Name;
            if (_screenHandles.TryGetValue(screenName, out var handle))
            {
                Addressables.Release(handle);
                _screenHandles.Remove(screenName);
            }
            Addressables.ReleaseInstance(screen.gameObject);

            if (isTop && newTop != null)
            {
                newTop.ResumeLifecycle();
            }
        }

        public async UniTask<T> OpenPopup<T>() where T : UIPopup
        {
            string address = string.Format(PopupAddressTemplate, typeof(T).Name);

            var prefabHandle = Addressables.LoadAssetAsync<GameObject>(address);
            _popupHandles[typeof(T).Name] = prefabHandle;
            var prefab = await prefabHandle;
            var newPopup = _container.InstantiatePrefabForComponent<T>(prefab);

            newPopup.transform.SetParent(_uiRoot.PopupRoot, false);
            newPopup.transform.localPosition = Vector3.zero;
            newPopup.transform.localScale = Vector3.one;
            newPopup.Create();
            newPopup.StartLifecycle();

            var topScreen = _screenStack.Count > 0 ? _screenStack.LastOrDefault() : null;
            if (topScreen != null)
            {
                topScreen.PauseLifecycle();
            }

            newPopup.ResumeLifecycle();

            _popupStack.Add(newPopup);
            _generalStack.Add(newPopup);

            return newPopup;
        }

        public UniTask ClosePopup(UIPopup popup)
        {
            if (!_popupStack.Contains(popup))
            {
                Debug.LogWarning($"Popup {popup.GetType().Name} is not open.");
                return UniTask.CompletedTask;
            }

            popup.PauseLifecycle();

            var newTop = _screenStack.Count > 0 ? _screenStack.LastOrDefault() : null;
            if (newTop != null)
            {
                newTop.ResumeLifecycle();
            }

            _generalStack.Remove(popup);
            _popupStack.Remove(popup);

            popup.StopLifecycle();
            popup.Close();

            string popupName = popup.GetType().Name;
            if (_popupHandles.TryGetValue(popupName, out var handle))
            {
                Addressables.Release(handle);
                _popupHandles.Remove(popupName);
            }
            Addressables.ReleaseInstance(popup.gameObject);

            return UniTask.CompletedTask;
        }
    }
}
