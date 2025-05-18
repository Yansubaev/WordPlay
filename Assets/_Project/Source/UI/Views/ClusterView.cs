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
        public event Action<ClusterView, IEnumerable<View>> OnDragEnd;
        public event Action<ClusterView, IEnumerable<View>> OnDragging;

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
            Debug.Log($"Center Enter: {me.name} -> {target.name}");
            if (target is LetterView letterView)
            {
                letterView.Color = Color.cyan;
            }
        }

        private void HandleCenterExit(LetterView me, View target)
        {
            Debug.Log($"Center Exit: {me.name} -> {target.name}");
            if (target is LetterView letterView)
            {
                letterView.Color = Color.white;
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
            var viewsUnder = FindViewsUnder();
            OnDragEnd?.Invoke(this, viewsUnder);
        }

        private void HandleDragging(PointerEventData data)
        {
            _canvasGroup.alpha = 1f;
            var viewsUnder = FindViewsUnder();
            OnDragging?.Invoke(this, viewsUnder);
        }

        private IEnumerable<View> FindViewsUnder()
        {
            var originalBlocksRaycast = _canvasGroup.blocksRaycasts;
            _canvasGroup.blocksRaycasts = false;

            var viewsList = new List<View>();
            foreach (var element in this)
            {
                var viewUnderCenter = element.CheckCenterOverlap();
                if (viewUnderCenter != null)
                {
                    viewsList.Add(viewUnderCenter);
                }
            }
            var views = viewsList;

            _canvasGroup.blocksRaycasts = originalBlocksRaycast;

            return views;
        }

        #endregion
    }
}