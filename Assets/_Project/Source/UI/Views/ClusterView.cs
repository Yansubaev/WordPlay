using Source.UI.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yans.UI.Views;

namespace Source.UI
{
    public class ClusterView : WordView
    {
        #region private fields

        [SerializeField]
        private DraggableComponent _draggableComponent;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        #endregion

        public event Action<ClusterView> OnDragBegin;
        public event Action<ClusterView, View> OnDragEnd;
        public event Action<ClusterView, View> OnDragging;
        public event Action<ClusterView, LetterView> OnHoveringOverEnter;
        public event Action<ClusterView, LetterView> OnHoveringOverExit;

        #region public methods

        public void SnapBack()
        {
            _draggableComponent.ResetPosition();
        }

        #endregion

        #region protected methods

        protected override void OnEnable()
        {
            base.OnEnable();
            _draggableComponent.OnDragStarted += HandleDragStarted;
            _draggableComponent.OnDragEnded += HandleDragEnded;
            _draggableComponent.OnDragging += HandleDragging;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _draggableComponent.OnDragStarted -= HandleDragStarted;
            _draggableComponent.OnDragEnded -= HandleDragEnded;
            _draggableComponent.OnDragging -= HandleDragging;
        }

        protected override LetterView CreateNewView()
        {
            var view = base.CreateNewView();
            view.OnCenterEnter += HandleCenterEnter;
            view.OnCenterExit += HandleCenterExit;

            return view;
        }

        protected override void OnDestroyView(LetterView view)
        {
            view.OnCenterEnter -= HandleCenterEnter;
            view.OnCenterExit -= HandleCenterExit;

            base.OnDestroyView(view);
        }
        #endregion

        #region private methods

        private void HandleCenterEnter(LetterView me, View target)
        {
            if (target is LetterView letterView)
            {
                OnHoveringOverEnter?.Invoke(this, letterView);                
            }
        }

        private void HandleCenterExit(LetterView me, View target)
        {
            if (target is LetterView letterView)
            {
                OnHoveringOverExit?.Invoke(this, letterView);
            }
        }

        private void HandleDragStarted(PointerEventData data)
        {
            _canvasGroup.alpha = 0.3f;
            OnDragBegin?.Invoke(this);
        }

        private void HandleDragEnded(PointerEventData data)
        {
            _canvasGroup.alpha = 1f;
            var viewUnder = FindViewsUnder();
            OnDragEnd?.Invoke(this, viewUnder);
        }

        private void HandleDragging(PointerEventData data)
        {
            _canvasGroup.alpha = 1f;
            var viewUnder = FindViewsUnder();
            OnDragging?.Invoke(this, viewUnder);
        }

        private View FindViewsUnder()
        {
            var originalBlocksRaycast = _canvasGroup.blocksRaycasts;
            _canvasGroup.blocksRaycasts = false;

            var viewUnder = this[0].CheckCenterOverlap();

            _canvasGroup.blocksRaycasts = originalBlocksRaycast;

            return viewUnder;
        }

        #endregion
    }
}