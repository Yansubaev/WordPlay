using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Yans.UI.Screen;
using Yans.UI.Transitions;
using Yans.ViewModels;

namespace Yans.UI
{
    /// <summary>
    /// This class is responsible for managing the UI panels in the game.
    /// </summary>
    public class UIScreenManager : IScreenManager, IOrientationChangeListener
    {
        #region private fields
        private readonly List<string> _addressesCache;
        private ScreenOrientation _currentScreenOrientation;
        private readonly List<UIScreen> _generalStack = new();
        private ScreenOrientation _oldScreenOrientation;
        private readonly List<UIPopup> _popupStack = new();
        private readonly Dictionary<ScreenOrientation, string> _screenOrientationSuffix;
        private readonly Dictionary<string, UIPanel> _screens = new();
        private readonly List<UIPanel> _screenStack = new();
        private readonly ITransitionResolver _transitionResolver;
        private readonly UIRoot _uiRoot;
        private readonly IViewModelProvider _viewModelProvider;
        #endregion

        #region protected properties
        protected virtual string PanelAddressTemplate => "UI/Panels/{0}";
        protected virtual string PopupAddressTemplate => "UI/Popups/{0}";
        #endregion

        public UIScreenManager(
            UIRoot uIRoot,
            ITransitionResolver transitionResolver,
            IViewModelProvider viewModelProvider)
        {
            _uiRoot = uIRoot;
            _transitionResolver = transitionResolver;
            _viewModelProvider = viewModelProvider;

            _uiRoot.AddOrientationListener(this);

            _oldScreenOrientation = _uiRoot.CurrentOrientation;
            _currentScreenOrientation = _uiRoot.CurrentOrientation;
            _screenOrientationSuffix = new Dictionary<ScreenOrientation, string>
            {
                {ScreenOrientation.Portrait, "/Portrait"},
                {ScreenOrientation.PortraitUpsideDown, "/PortraitUpsideDown"},
                {ScreenOrientation.LandscapeLeft, "/Landscape"},
                {ScreenOrientation.LandscapeRight, "/LandscapeRight"},
                {ScreenOrientation.AutoRotation, ""},
            };

            _addressesCache = GetAddressesFromGroup();
        }

        #region public methods

        public async UniTask ClosePanel(UIPanel screen)
        {
            if (!_screens.ContainsKey(screen.GetType().Name))
            {
                Debug.LogWarning($"Screen {screen.GetType().Name} is not open.");
                return;
            }

            bool isTopScreen = _screenStack.Last() == screen;
            _screenStack.Remove(screen);
            _screens.Remove(screen.GetType().Name);
            _generalStack.Remove(screen);

            SafePauseLifecycle(screen);

            if (isTopScreen)
            {
                var newTopScreen = _screenStack.LastOrDefault();

                SafeStartLifecycle(newTopScreen);

                await HandlePanelsTransition(screen, newTopScreen);

                SafeResumeLifecycle(newTopScreen);
            }

            SafeStopLifecycle(screen);

            CleanupScreen(screen);
        }

        public UniTask ClosePopup(UIPopup popup)
        {
            if (!_popupStack.Contains(popup))
            {
                Debug.LogWarning($"Popup {popup.GetType().Name} is not open.");
                return UniTask.CompletedTask;
            }

            SafePauseLifecycle(popup);
            _popupStack.Remove(popup);
            _generalStack.Remove(popup);

            SafeResumeLifecycle(_screenStack.LastOrDefault());

            SafeStopLifecycle(popup);

            CleanupScreen(popup);
            return UniTask.CompletedTask;
        }

        public UniTask CloseTop()
        {
            return _generalStack.Count == 0 ? UniTask.CompletedTask :
            _generalStack.Last() switch
            {
                UIPanel screen => ClosePanel(screen),
                UIPopup popup => ClosePopup(popup),
                _ => throw new System.Exception("Unknown screen type in stack.")
            };
        }

        public List<string> GetAddressesFromGroup()
        {
            var addresses = new List<string>();

            foreach (var locator in Addressables.ResourceLocators)
            {
                foreach (var key in locator.Keys)
                {
                    string address = key.ToString();
                    if (address.Contains("Popups", System.StringComparison.InvariantCulture)
                        || address.Contains("Panels", System.StringComparison.InvariantCulture))
                    {
                        addresses.Add(address);
                    }
                }
            }

            return addresses;
        }

        public void OnOrientationChanged(ScreenOrientation newOrientation)
        {
            _oldScreenOrientation = _currentScreenOrientation;
            _currentScreenOrientation = newOrientation;

            RebuildScreens();
        }

        public async UniTask<T> OpenPanel<T>() where T : UIPanel
        {
            var newScreen = await InstantiateUI<T>();
            newScreen.Create(_viewModelProvider);

            var previousScreen = _screenStack.LastOrDefault();

            SafePauseLifecycle(previousScreen);

            newScreen.StartLifecycle();

            await HandlePanelsTransition(previousScreen, newScreen);

            _screenStack.Add(newScreen);
            _screens[typeof(T).Name] = newScreen;
            _generalStack.Add(newScreen);

            SafeStopLifecycle(previousScreen);

            newScreen.ResumeLifecycle();

            return newScreen;
        }

        public async UniTask<T> OpenPopup<T>() where T : UIPopup
        {
            var newPopup = await InstantiateUI<T>();
            newPopup.Create(_viewModelProvider);
            newPopup.StartLifecycle();

            SafePauseLifecycle(_screenStack.LastOrDefault());

            _popupStack.Add(newPopup);
            _generalStack.Add(newPopup);

            return newPopup;
        }

        #endregion

        #region protected methods

        protected virtual async UniTask<T> InstantiateScreenPrefab<T>(GameObject prefab, Transform parent) where T : UIScreen
        {
            var instances = await Object.InstantiateAsync(prefab, parent);
            return instances[0].GetComponent<T>();
        }

        #endregion

        #region private methods

        private string BuildAddressForOrientation<T>(ScreenOrientation orientation, string addressTemplate)
        {
            return BuildAddressForOrientationType(typeof(T), orientation, addressTemplate);
        }

        private string BuildAddressForOrientationType(System.Type type, ScreenOrientation orientation, string addressTemplate)
        {
            var suffix = $"{type.Name}{_screenOrientationSuffix[orientation]}";
            return string.Format(addressTemplate, suffix);
        }

        private void CleanupScreen(UIScreen screen)
        {
            screen.Close();
            Addressables.ReleaseInstance(screen.gameObject);
            GameObject.Destroy(screen.gameObject);
        }

        private string GetAddress<T>(ScreenOrientation screenOrientation) where T : UIScreen
        {
            return GetAddress(typeof(T), screenOrientation);
        }

        private string GetAddress(System.Type type, ScreenOrientation screenOrientation)
        {
            var addressTemplate = type.IsSubclassOf(typeof(UIPanel)) ? PanelAddressTemplate : PopupAddressTemplate;

            var orientation = screenOrientation;
            var address = BuildAddressForOrientationType(type, orientation, addressTemplate);

            while (!_addressesCache.Contains(address))
            {
                if (orientation == ScreenOrientation.AutoRotation)
                    throw new System.Exception($"No {type.Name} prefab for address {address}");

                orientation = GetFallbackOrientation(orientation);
                address = BuildAddressForOrientationType(type, orientation, addressTemplate);
            }

            return address;
        }

        private ScreenOrientation GetFallbackOrientation(ScreenOrientation orientation)
        {
            return orientation switch
            {
                ScreenOrientation.Portrait => ScreenOrientation.AutoRotation,
                ScreenOrientation.PortraitUpsideDown => ScreenOrientation.Portrait,
                ScreenOrientation.LandscapeLeft => ScreenOrientation.AutoRotation,
                ScreenOrientation.LandscapeRight => ScreenOrientation.LandscapeLeft,
                ScreenOrientation.AutoRotation => ScreenOrientation.AutoRotation,
                _ => ScreenOrientation.AutoRotation,
            };
        }

        private async UniTask HandlePanelsTransition(UIPanel fromScreen, UIPanel toScreen)
        {
            if (fromScreen == null || toScreen == null) return;

            var transition = _transitionResolver.Resolve(fromScreen.GetType(), toScreen.GetType());
            await transition.Play(fromScreen, toScreen);
        }

        private async UniTask<T> InstantiateUI<T>() where T : UIScreen
        {
            string address = GetAddress<T>(_currentScreenOrientation);
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>(address);

            var prefab = await prefabHandle.ToUniTask();

            var parent = typeof(T).IsSubclassOf(typeof(UIPanel)) ? _uiRoot.ScreenRoot : _uiRoot.PopupRoot;
            var instance = await InstantiateScreenPrefab<T>(prefab, parent);

            SetupTransform(instance, parent);
            Addressables.Release(prefabHandle);

            return instance;
        }

        private async UniTask RebuildScreen(UIScreen oldScreen)
        {
            var screenType = oldScreen.GetType();
            var oldAddress = GetAddress(screenType, _oldScreenOrientation);
            var newAddress = GetAddress(screenType, _currentScreenOrientation);

            if (oldAddress.Equals(newAddress))
            {
                RestoreScreenToStacks(oldScreen);
                return;
            }

            var parent = oldScreen is UIPanel ? _uiRoot.ScreenRoot : _uiRoot.PopupRoot;
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>(newAddress);
            var prefab = await prefabHandle.ToUniTask();

            var newScreenComponent = await InstantiateScreenPrefab<UIScreen>(prefab, parent);

            TransferConfiguration(oldScreen, newScreenComponent);
            SetupTransform(newScreenComponent, parent);

            RestoreScreenToStacks(newScreenComponent);

            CleanupScreen(oldScreen);
            Addressables.Release(prefabHandle);
        }

        private async void RebuildScreens()
        {
            var screensToRebuild = _generalStack.ToList();
            _generalStack.Clear();
            _screenStack.Clear();
            _popupStack.Clear();
            _screens.Clear();

            foreach (var screen in screensToRebuild)
            {
                try
                {
                    await RebuildScreen(screen);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to rebuild screen {screen.GetType().Name}: {e}");
                }
            }
        }

        private void RestoreScreenToStacks(UIScreen screen)
        {
            _generalStack.Add(screen);

            if (screen is UIPanel panel)
            {
                _screenStack.Add(panel);
                _screens[panel.GetType().Name] = panel;
            }
            else if (screen is UIPopup popup)
            {
                _popupStack.Add(popup);
            }
        }

        private void SafePauseLifecycle(UIScreen screen)
        {
            if (screen != null)
                screen.PauseLifecycle();
        }

        private void SafeResumeLifecycle(UIScreen screen)
        {
            if (screen != null)
                screen.ResumeLifecycle();
        }

        private void SafeStartLifecycle(UIScreen screen)
        {
            if (screen != null)
                screen.StartLifecycle();
        }

        private void SafeStopLifecycle(UIScreen screen)
        {
            if (screen != null)
                screen.StopLifecycle();
        }

        private void SetupTransform(UIScreen screen, Transform parent)
        {
            screen.transform.localPosition = Vector3.zero;
            screen.transform.localScale = Vector3.one;
        }

        private void TransferConfiguration(UIScreen oldScreen, UIScreen newScreen)
        {
            var IsLifecycleStarted = oldScreen.IsLifecycleStarted;
            var IsLifecyclePaused = oldScreen.IsLifecyclePaused;

            oldScreen.PauseLifecycle();
            oldScreen.StopLifecycle();

            newScreen.Create(_viewModelProvider, oldScreen.GetInstanceId());
            if (IsLifecycleStarted)
            {
                newScreen.StartLifecycle();

                if (IsLifecyclePaused)
                {
                    newScreen.PauseLifecycle();
                }
            }
        }

        #endregion
    }
}