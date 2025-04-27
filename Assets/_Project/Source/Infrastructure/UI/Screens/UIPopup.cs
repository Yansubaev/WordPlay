using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Source.Infrastructure.UI
{
    public abstract class UIPopup : UIBehaviour, ILifecycleOwner
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] GraphicRaycaster _graphicRaycaster;
        [SerializeField] private RectTransform _transitionRoot;
        [SerializeField] private CanvasGroup _fadeRoot;

        private SignalBus _signalBus;

        public Canvas Canvas => _canvas;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;
        public RectTransform TransitionRoot => _transitionRoot;
        public CanvasGroup FadeRoot => _fadeRoot;
        public GameObject GameObject => gameObject;

        protected SignalBus SignalBus => _signalBus;

        protected virtual void OnCreated() { }
        protected virtual void OnStarted() { }
        protected virtual void OnResumed() { }
        protected virtual void OnPaused() { }
        protected virtual void OnStopped() { }
        protected virtual void OnClosed() { }

        public void Create(SignalBus signalBus)
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=green>[SCREEN] {name}.Create</color>", gameObject);
#endif
            _signalBus = signalBus;

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
            Debug.Log($"<color=cyan>[SCREEN] {name}.Resume</color>", gameObject);
#endif

            GraphicRaycaster.enabled = true;

            OnResumed();
        }

        public void PauseLifecycle()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=orange>[SCREEN] {name}.Pause</color>", gameObject);
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
