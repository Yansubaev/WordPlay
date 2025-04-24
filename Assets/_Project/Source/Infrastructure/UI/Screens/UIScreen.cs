using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Source.Infrastructure.UI
{
    public abstract class UIScreen : UIBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _transitionRoot;
        [SerializeField] private CanvasGroup _fadeRoot;
        [SerializeField] private RectTransform _wrapper;

        private SignalBus _signalBus;

        public Canvas Canvas => _canvas;
        public RectTransform TransitionRoot => _transitionRoot;
        public CanvasGroup FadeRoot => _fadeRoot;
        protected SignalBus SignalBus => _signalBus;

        public void Create(SignalBus signalBus)
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=green>[SCREEN] {name}.Create</color>", gameObject);
#endif
            _signalBus = signalBus;

            OnCreated();
        }

        public void StartScreen()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=blue>[SCREEN] {name}.StartScreen</color>", gameObject);
#endif
            OnStarted();
        }

        public void StopScreen()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=yellow>[SCREEN] {name}.StopScreen</color>", gameObject);
#endif

            OnStopped();
        }

        public void Close()
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=magenta>[SCREEN] {name}.Close</color>", gameObject);
#endif

            OnClosed();
        }

        protected virtual void OnCreated() { }
        protected virtual void OnStarted() { }
        protected virtual void OnStopped() { }
        protected virtual void OnClosed() { }
    }
}
