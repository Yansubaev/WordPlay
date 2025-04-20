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
            Debug.Log($"<color=green>[SCREEN] {name}.Create</color>", gameObject);

            _signalBus = signalBus;
            
            OnCreated();
        }

        public void StartScreen()
        {
            Debug.Log($"<color=green>[SCREEN] {name}.StartScreen</color>", gameObject);
            OnStarted();
        }

        public void StopScreen()
        {
            Debug.Log($"<color=green>[SCREEN] {name}.StopScreen</color>", gameObject);
            OnStopped();
        }

        public void Close()
        {
            Debug.Log($"<color=green>[SCREEN] {name}.Close</color>", gameObject);
            OnClosed();
        }

        protected virtual void OnCreated() { }
        protected virtual void OnStarted() { }
        protected virtual void OnStopped() { }
        protected virtual void OnClosed() { }
    }
}
