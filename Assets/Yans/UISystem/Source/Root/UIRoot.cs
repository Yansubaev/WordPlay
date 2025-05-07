using UnityEngine;
using UnityEngine.UI;

namespace Yans.UI
{
    [DisallowMultipleComponent]
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private RectTransform _screenRoot;
        [SerializeField] private RectTransform _popupRoot;
        [SerializeField] private RectTransform _hudRoot;

        public Canvas Canvas => _canvas;
        public CanvasScaler CanvasScaler => _canvasScaler;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;

        public RectTransform ScreenRoot => _screenRoot;
        public RectTransform PopupRoot => _popupRoot;
        public RectTransform HudRoot => _hudRoot;

        
    }
}
