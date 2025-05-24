using System.Collections.Generic;
using Source.UI.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yans.UI.Views;

namespace Source.UI
{
    public class LetterView : View
    {
        public delegate void CenterOverlapEvent(LetterView me, View target);

        #region private fields

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private EnumerableView<LetterView> _parentEnumerableView;

        [SerializeField]
        private ColorPalette _colorPalette;

        private View _currentObjectUnderCenter;
        private bool _isHovered;
        private Color _initialColor;

        #endregion

        #region public properties

        public string Letter
        {
            get => _text.text;
            set => _text.text = value;
        }

        public char Character
        {
            get => _text.text[0];
            set => _text.text = value.ToString();
        }

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public Color InitialColor
        {
            get => _initialColor;
            set
            {
                _initialColor = value;
                _image.color = value;
            }
        }

        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                if (_isHovered != value)
                {
                    _isHovered = value;
                    _image.color = value ? _colorPalette.HoverCellColor : _initialColor;
                }
            }
        }

        public EnumerableView<LetterView> ParentEnumerableView
        {
            get
            {
                if (_parentEnumerableView == null)
                {
                    _parentEnumerableView = GetComponentInParent<EnumerableView<LetterView>>();
                    if (_parentEnumerableView == null)
                    {
                        Debug.LogWarning($"No EnumerableView found for {gameObject.name}");
                    }
                }
                return _parentEnumerableView;
            }
        }

        public int AdapterPosition
        {
            get
            {
                if (ParentEnumerableView == null)
                    return -1;

                return RectTransform.GetSiblingIndex();
            }
        }

        #endregion

        // Events for center enter/exit
        public event CenterOverlapEvent OnCenterEnter;

        public event CenterOverlapEvent OnCenterExit;

        #region public methods

        /// <summary>
        /// Check if the center of this object is over a new object or has exited a previous object
        /// </summary>
        /// <returns>The current object under the center</returns>
        public View CheckCenterOverlap()
        {
            View viewUnderCenter = FindObjectUnderCenter();

            // Check if we exited the previous object
            if (_currentObjectUnderCenter != null && viewUnderCenter != _currentObjectUnderCenter)
            {
                OnCenterExit?.Invoke(this, _currentObjectUnderCenter);
            }

            // Check if we entered a new object
            if (viewUnderCenter != null && viewUnderCenter != _currentObjectUnderCenter)
            {
                OnCenterEnter?.Invoke(this, viewUnderCenter);
            }

            // Update current object reference
            _currentObjectUnderCenter = viewUnderCenter;

            return viewUnderCenter;
        }

        /// <summary>
        /// Get the current object under the center without updating events
        /// </summary>
        public View GetCurrentObjectUnderCenter()
        {
            return _currentObjectUnderCenter;
        }

        /// <summary>
        /// Manually clear the current object under center (e.g., when object is disabled)
        /// </summary>
        public void ClearCurrentObjectUnderCenter()
        {
            if (_currentObjectUnderCenter != null)
            {
                OnCenterExit?.Invoke(this, _currentObjectUnderCenter);
                _currentObjectUnderCenter = null;
            }
        }

        /// <summary>
        /// Find what object is currently under the center of our draggable object
        /// </summary>
        public View FindObjectUnderCenter()
        {
            // Create a new PointerEventData with the center position of our object
            PointerEventData centerPointerData = new PointerEventData(EventSystem.current);

            // Convert rect transform center to screen position
            Vector3 worldCenter = RectTransform.TransformPoint(RectTransform.rect.center);
            Vector2 screenCenter;

            if (ParentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                screenCenter = worldCenter;
            }
            else
            {
                Camera cam = ParentCanvas.worldCamera != null ? ParentCanvas.worldCamera : Camera.main;
                screenCenter = RectTransformUtility.WorldToScreenPoint(cam, worldCenter);
            }

            centerPointerData.position = screenCenter;

            // Temporarily enable raycast blocking to prevent finding this object
            bool originalBlocksRaycast = _canvasGroup.blocksRaycasts;
            _canvasGroup.blocksRaycasts = false;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(centerPointerData, results);

            // Restore original setting
            _canvasGroup.blocksRaycasts = originalBlocksRaycast;

            // Return the first hit object that isn't this one
            foreach (var result in results)
            {
                if (result.gameObject != gameObject)
                {
                    return result.gameObject.TryGetComponent<View>(out var view) ? view : null;
                }
            }

            return null;
        }

        #endregion

        #region protected methods

        protected override void OnDisable()
        {
            base.OnDisable();
            ClearCurrentObjectUnderCenter();
        }

        #endregion
    }
}