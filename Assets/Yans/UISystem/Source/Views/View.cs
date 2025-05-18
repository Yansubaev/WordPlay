using UnityEngine;
using UnityEngine.EventSystems;

namespace Yans.UI.Views
{
    public class View : UIBehaviour
    {
        #region private fields

        [SerializeField]
        private RectTransform _rectTransform;

        private Canvas _parentCanvas;
        #endregion

        #region public properties

        public RectTransform RectTransform => _rectTransform = _rectTransform != null
            ? _rectTransform
            : (RectTransform)transform;

        public Canvas ParentCanvas
        {
            get
            {
                if (_parentCanvas == null)
                {
                    _parentCanvas = GetComponentInParent<Canvas>();
                    if (_parentCanvas == null)
                        Debug.LogWarning($"No Canvas found for {gameObject.name}");
                }
                return _parentCanvas;
            }
        }

        #endregion
    }
}