using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yans.UI.Screen
{
    public abstract class UIScreen : UIBehaviour, ILifecycleOwner
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private RectTransform _transitionRoot;
        [SerializeField] private CanvasGroup _fadeRoot;

        public Canvas Canvas => _canvas;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;
        public RectTransform TransitionRoot => _transitionRoot;
        public CanvasGroup FadeRoot => _fadeRoot;
        public GameObject GameObject => gameObject;

        private List<ILifecycleListener> _lifecycleListeners;

        protected virtual void OnCreated() { }
        protected virtual void OnStarted() { }
        protected virtual void OnResumed() { }
        protected virtual void OnPaused() { }
        protected virtual void OnStopped() { }
        protected virtual void OnClosed() { }

        void ILifecycleOwner.AddLifecycleListener(ILifecycleListener lifecycleListener)
        {
            _lifecycleListeners ??= new List<ILifecycleListener>();
            _lifecycleListeners.Add(lifecycleListener);
        }

        public void Create()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=green>[SCREEN] {name}.Create</color>", gameObject);
#endif
            OnCreated();
        }

        public void StartLifecycle()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=blue>[SCREEN] {name}.StartScreen</color>", gameObject);
#endif
            Canvas.enabled = true;

            OnStarted();
        }

        public void ResumeLifecycle()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=cyan>[SCREEN] {name}.ResumeScreen</color>", gameObject);
#endif

            GraphicRaycaster.enabled = true;

            OnResumed();
        }

        public void PauseLifecycle()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=orange>[SCREEN] {name}.PauseScreen</color>", gameObject);
#endif

            GraphicRaycaster.enabled = false;

            OnPaused();
        }

        public void StopLifecycle()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=yellow>[SCREEN] {name}.StopScreen</color>", gameObject);
#endif
            Canvas.enabled = false;

            OnStopped();
        }

        public void Close()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=magenta>[SCREEN] {name}.Close</color>", gameObject);
#endif

            OnClosed();
        }
    }
}
