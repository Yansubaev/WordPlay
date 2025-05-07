using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Infrastructure.UI.Views
{
    public class View : UIBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform = _rectTransform != null
            ? _rectTransform
            : (RectTransform)transform;

    }
}