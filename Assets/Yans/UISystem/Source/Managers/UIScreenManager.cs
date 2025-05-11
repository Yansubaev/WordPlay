using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yans.UI.Screen;
using Yans.UI.Transitions;
using Yans.ViewModels;

namespace Yans.UI
{
    public class UIScreenManager : IScreenManager, IOrientationChangeListener
    {
        #region private fields
        private ScreenOrientation _currentScreenOrientation;
        private readonly List<UIScreen> _generalStack = new();
        private readonly ITransitionResolver _transitionResolver;
        private readonly UIRoot _uiRoot;
        private readonly IScreenInstantiator _screenInstantiator;
        private readonly IViewModelProvider _viewModelProvider;
        #endregion

        public UIScreenManager(
            UIRoot uIRoot,
            IScreenInstantiator screenInstantiator,
            ITransitionResolver transitionResolver,
            IViewModelProvider viewModelProvider)
        {
            _uiRoot = uIRoot;
            _screenInstantiator = screenInstantiator;
            _transitionResolver = transitionResolver;
            _viewModelProvider = viewModelProvider;
            _currentScreenOrientation = uIRoot.CurrentOrientation;

            _uiRoot.AddOrientationListener(this);
        }

        #region public methods

        public async UniTask<T> OpenScreen<T>() where T : UIScreen
        {
            var newScreen = await _screenInstantiator.InstantiateScreen<T>(_uiRoot.ScreenRoot, _currentScreenOrientation);
            var prevScreen = _generalStack.LastOrDefault();
            _generalStack.Add(newScreen);

            SafePauseLifecycle(prevScreen);
            SafeStartLifecycle(newScreen);

            await HandlePanelsTransition(prevScreen, newScreen);

            if (prevScreen is not UIPopup)
                SafeStopLifecycle(prevScreen);

            SafeResumeLifecycle(newScreen);

            return newScreen;
        }

        public async UniTask CloseScreen(UIScreen screen)
        {
            if (!_generalStack.Contains(screen))
            {
                Debug.LogWarning($"Screen {screen.GetType().Name} is not open.");
                return;
            }

            var isTopScreen = _generalStack.Last() == screen;
            _generalStack.Remove(screen);

            SafePauseLifecycle(screen);

            if (isTopScreen)
            {
                var newTopScreen = _generalStack.LastOrDefault();
                SafeStartLifecycle(newTopScreen);

                await HandlePanelsTransition(screen, newTopScreen);

                SafeResumeLifecycle(newTopScreen);
            }

            SafeStopLifecycle(screen);
            CleanupScreen(screen);
        }

        public UniTask CloseTop() =>
            _generalStack.Count == 0 ? UniTask.CompletedTask : CloseScreen(_generalStack.Last());

        public void OnOrientationChanged(ScreenOrientation newOrientation)
        {
            _currentScreenOrientation = newOrientation;
            RebuildScreens();
        }

        #endregion

        #region private methods

        private void CleanupScreen(UIScreen screen)
        {
            screen.Close();
            _screenInstantiator.CleanUpScreen(screen);
        }

        private async UniTask HandlePanelsTransition(UIScreen fromScreen, UIScreen toScreen)
        {
            if (fromScreen is not UIPanel fomPanel || toScreen is not UIPanel toPanel) return;

            var transition = _transitionResolver.Resolve(fomPanel.GetType(), toPanel.GetType());
            await transition.Play(fomPanel, toPanel);
        }

        private async void RebuildScreens()
        {
            var screensToRebuild = _generalStack.ToList();
            _generalStack.Clear();

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

        private async UniTask RebuildScreen(UIScreen oldScreen)
        {
            var newScreen = await _screenInstantiator.InstantiateScreen(oldScreen.GetType(), _uiRoot.ScreenRoot, _currentScreenOrientation);

            if (oldScreen == newScreen)
            {
                _generalStack.Add(oldScreen);
                return;
            }

            TransferConfiguration(oldScreen, newScreen);
            _generalStack.Add(newScreen);
            CleanupScreen(oldScreen);
        }

        private void TransferConfiguration(UIScreen oldScreen, UIScreen newScreen)
        {
            var isLifecycleStarted = oldScreen.IsLifecycleStarted;
            var isLifecycleResumed = oldScreen.IsLifecycleResumed;

            oldScreen.PauseLifecycle();
            oldScreen.StopLifecycle();

            newScreen.Create(_viewModelProvider, oldScreen.GetInstanceId());

            if (isLifecycleStarted)
            {
                newScreen.StartLifecycle();
                if (isLifecycleResumed)
                {
                    newScreen.ResumeLifecycle();
                }
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

        #endregion
    }
}