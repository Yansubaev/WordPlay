using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Source.Infrastructure.Services;
using Source.Infrastructure.UI.Transitions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Source.Infrastructure.UI
{

    /// <summary>
    /// This class is responsible for managing the UI elements in the game.
    /// </summary>
    public class UIScreenService : IScreenService
    {
        private const string ScreenAddressTemplate = "UI/Screens/{0}.prefab";
        private const string PopupAddressTemplate = "UI/Popups/{0}.prefab";

        private readonly Dictionary<string, UIScreen> _screens = new();
        private readonly List<UIScreen> _screenStack = new();
        private readonly List<UIPopup> _popupStack = new();
        private readonly List<ILifecycleOwner> _generalStack = new();

        private UIRoot _uiRoot;
        private ITransitionResolver _transitionResolver;
        private SignalBus _signalBus;

        [Inject]
        public void Inject(UIRoot uIRoot, ITransitionResolver transitionResolver, SignalBus signalBus)
        {
            _uiRoot = uIRoot;
            _transitionResolver = transitionResolver;
            _signalBus = signalBus;
        }

        public async UniTask<T> OpenScreen<T>() where T : UIScreen
        {
            string address = string.Format(ScreenAddressTemplate, typeof(T).Name);
            var handle = Addressables.InstantiateAsync(address);
            var screenObject = await handle.ToUniTask();
            var newScreen = screenObject.GetComponent<T>();

            newScreen.transform.SetParent(_uiRoot.ScreenRoot, false);
            newScreen.transform.localPosition = Vector3.zero;
            newScreen.transform.localScale = Vector3.one;
            newScreen.Create(_signalBus);

            UIScreen previous = _screenStack.Count > 0 ? _screenStack.LastOrDefault() : null;

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

            if (topScreen is UIScreen screen)
            {
                return CloseScreen(screen);
            }
            else if (topScreen is UIPopup popup)
            {
                return ClosePopup(popup);
            }

            throw new System.Exception("Unknown screen type in stack.");
        }

        public async UniTask CloseScreen(UIScreen screen)
        {
            if (!_screens.TryGetValue(screen.GetType().Name, out var existingScreen))
            {
                Debug.LogWarning($"Screen {screen.GetType().Name} is not open.");
                return;
            }

            bool isTop = _screenStack.Last() == screen;

            UIScreen newTop = null;
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

            Addressables.ReleaseInstance(screen.gameObject);
            // Object.Destroy(screen.gameObject);

            if (isTop && newTop != null)
            {
                newTop.ResumeLifecycle();
            }
        }

        public async UniTask<T> OpenPopup<T>() where T : UIPopup
        {
            string address = string.Format(PopupAddressTemplate, typeof(T).Name);
            var handle = Addressables.InstantiateAsync(address);
            var popupObject = await handle.ToUniTask();

            var newPopup = popupObject.GetComponent<T>();

            newPopup.transform.SetParent(_uiRoot.PopupRoot, false);
            newPopup.transform.localPosition = Vector3.zero;
            newPopup.transform.localScale = Vector3.one;
            newPopup.Create(_signalBus);
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

            popup.StopLifecycle();
            popup.Close();

            Addressables.ReleaseInstance(popup.gameObject);
            return UniTask.CompletedTask;
        }
    }
}
