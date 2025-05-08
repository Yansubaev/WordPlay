using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yans.ViewModels;

namespace Yans.UI.Screen
{
    public abstract class UIScreen : UIBehaviour, IViewModelOwner
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private RectTransform _transitionRoot;
        [SerializeField] private CanvasGroup _fadeRoot;

        private IViewModelProvider _viewModelProvider;
        private string _instanceId;

        public Canvas Canvas => _canvas;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;
        public RectTransform TransitionRoot => _transitionRoot;
        public CanvasGroup FadeRoot => _fadeRoot;
        protected IViewModelProvider ViewModelProvider => _viewModelProvider;

        public bool IsLifecycleStarted { get; private set; }
        public bool IsLifecyclePaused { get; private set; }

        protected virtual void OnCreated() { }
        protected virtual void OnStarted() { }
        protected virtual void OnResumed() { }
        protected virtual void OnPaused() { }
        protected virtual void OnStopped() { }
        protected virtual void OnClosed() { }


        public string GetInstanceId()
        {
            return _instanceId;
        }

        public void Create(IViewModelProvider viewModelProvider = null, string instanceId = null)
        {
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=green>[SCREEN] {name}.Create</color>", gameObject);
#endif
            _viewModelProvider = viewModelProvider;
            _instanceId = instanceId ?? $"{GetType().Name}-{System.Guid.NewGuid()}";

            CreateLifecycle();
        }

        public void CreateLifecycle()
        {

            OnCreated();
        }

        public void StartLifecycle()
        {
            if (IsLifecycleStarted) return;

#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=blue>[SCREEN] {name}.StartScreen</color>", gameObject);
#endif
            Canvas.enabled = true;

            OnStarted();
            IsLifecycleStarted = true;
        }

        public void ResumeLifecycle()
        {
            if (!IsLifecyclePaused) return;

#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=cyan>[SCREEN] {name}.ResumeScreen</color>", gameObject);
#endif

            GraphicRaycaster.enabled = true;

            OnResumed();
            IsLifecyclePaused = false;
        }

        public void PauseLifecycle()
        {
            if(IsLifecyclePaused) return;
            
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=orange>[SCREEN] {name}.PauseScreen</color>", gameObject);
#endif

            GraphicRaycaster.enabled = false;

            OnPaused();
            IsLifecyclePaused = true;
        }

        public void StopLifecycle()
        {
            if(!IsLifecycleStarted) return;
            
#if LOG_SCREEN_LIFECYCLE
            Debug.Log($"<color=yellow>[SCREEN] {name}.StopScreen</color>", gameObject);
#endif
            Canvas.enabled = false;

            OnStopped();
            IsLifecycleStarted = false;
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
