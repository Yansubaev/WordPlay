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
        private const string AddressTemplate = "UI/Screens/{0}.prefab";

        private readonly Dictionary<string, UIScreen> _screens = new();
        private readonly Stack<UIScreen> _screenStack = new();

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
            string address = string.Format(AddressTemplate, typeof(T).Name);
            var handle = Addressables.InstantiateAsync(address);
            var screenObject = await handle.ToUniTask();
            var newScreen = screenObject.GetComponent<T>();

            newScreen.transform.SetParent(_uiRoot.ScreensRoot, false);
            newScreen.transform.localPosition = Vector3.zero;
            newScreen.transform.localScale = Vector3.one;
            newScreen.Create(_signalBus);

            UIScreen previous = _screenStack.Count > 0 ? _screenStack.Peek() : null;

            if (previous != null)
                previous.StopScreen();

            var transition = _transitionResolver.Resolve(previous?.GetType(), typeof(T));
            await transition.Play(previous, newScreen);

            if (previous != null)
            {
                previous.Canvas.enabled = false;
                previous.transform.localPosition = Vector3.zero;
                previous.transform.localScale = Vector3.one;
            }

            _screenStack.Push(newScreen);
            _screens[typeof(T).Name] = newScreen;

            newScreen.Canvas.enabled = true;
            newScreen.StartScreen();
            return newScreen;
        }

        public UniTask CloseTop()
        {
            if (_screenStack.Count == 0)
                return UniTask.CompletedTask;

            return CloseScreen(_screenStack.Peek());
        }

        public async UniTask CloseScreen(UIScreen screen)
        {
            if (!_screens.TryGetValue(screen.GetType().Name, out var existingScreen))
            {
                Debug.LogWarning($"Screen {screen.GetType().Name} is not open.");
                return;
            }

            var tempStack = new Stack<UIScreen>(_screenStack.Reverse());
            _screenStack.Clear();

            bool isTop = tempStack.Peek() == screen;

            UIScreen newTop = null;
            foreach (var scr in tempStack)
            {
                if (scr == screen) continue;
                _screenStack.Push(scr);
            }
            newTop = _screenStack.Count > 0 ? _screenStack.Peek() : null;

            _screens.Remove(screen.GetType().Name);

            screen.StopScreen();

            if (isTop)
            {
                newTop.Canvas.enabled = true;
                var transition = _transitionResolver.Resolve(screen.GetType(), newTop?.GetType());
                await transition.Play(screen, newTop);
            }

            screen.Close();
            Addressables.Release(screen.gameObject);
            Object.Destroy(screen.gameObject);

            if (isTop && newTop != null)
            {
                newTop.StartScreen();
            }
        }
    }
}
