using Source.UI.Components;
using System;
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

        #endregion

        public event Action<ClusterView> OnDragBegin;
        public event Action<ClusterView, View> OnDragEnd;

        #region protected methods

        protected override void Awake()
        {
            base.Awake();
            _draggableComponent.OnDragStarted += HandleDragStarted;
            _draggableComponent.OnDragEnded += HandleDragEnded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _draggableComponent.OnDragStarted -= HandleDragStarted;
            _draggableComponent.OnDragEnded -= HandleDragEnded;
        }

        #endregion

        #region private methods

        private void HandleDragStarted(PointerEventData data, GameObject @object)
        {
            OnDragBegin?.Invoke(this);
        }

        private void HandleDragEnded(PointerEventData data, GameObject @object)
        {
            var targetView = @object != null ? @object.GetComponent<View>() : null;
            OnDragEnd?.Invoke(this, targetView);
        }

        public void SnapBack()
        {
            _draggableComponent.ResetPosition();
        }

        #endregion
    }
}