using UnityEngine;
using UnityEngine.UI;

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

        public Canvas Canvas => _canvas;
        public CanvasScaler CanvasScaler => _canvasScaler;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;

        public RectTransform ScreenRoot => _screenRoot;
        public RectTransform PopupRoot => _popupRoot;
    }
}
