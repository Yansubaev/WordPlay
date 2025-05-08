using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Yans.UI
{
    /// <summary>
    /// Root object for UI screens such as panels and popups. Should be placed somewhere on scene.
    /// Use it in screen service to attach screens to specified containers
    /// </summary>
    [DisallowMultipleComponent]
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private RectTransform _screenRoot;
        [SerializeField] private RectTransform _popupRoot;

        private ScreenOrientation _currentOrientation;
        private readonly List<IOrientationChangeListener> _orientationListeners = new();

        public Canvas Canvas => _canvas;
        public CanvasScaler CanvasScaler => _canvasScaler;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;
        public RectTransform ScreenRoot => _screenRoot;
        public RectTransform PopupRoot => _popupRoot;
        public ScreenOrientation CurrentOrientation => _currentOrientation;

        private void Awake()
        {
            _currentOrientation = UnityEngine.Screen.orientation;
        }

        private void Update()
        {
            CheckOrientation();
        }

        private void CheckOrientation()
        {
            if (_currentOrientation != UnityEngine.Screen.orientation)
            {
                _currentOrientation = UnityEngine.Screen.orientation;
                NotifyOrientationListeners();
            }
        }

        public void AddOrientationListener(IOrientationChangeListener listener)
        {
            if (!_orientationListeners.Contains(listener))
            {
                _orientationListeners.Add(listener);
            }
        }

        public void RemoveOrientationListener(IOrientationChangeListener listener)
        {
            _orientationListeners.Remove(listener);
        }

        private void NotifyOrientationListeners()
        {
            foreach (var listener in _orientationListeners)
            {
                listener.OnOrientationChanged(_currentOrientation);
            }
        }
    }
}
