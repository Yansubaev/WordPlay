using System;
using System.Collections.Generic;
using Source.Presentation.Views;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Adapters;
using Yans.UI.Views;

namespace Source.Presentation.Adapters
{
    public class ClusterListAdapter : ListAdapter<ClusterView>
    {
        #region private fields
        private List<string> _clusters;
        private readonly Transform _dragArea;
        #endregion

        public event Action<ClusterView, LetterView> OnHoveringOverEnter;
        public event Action<ClusterView, LetterView> OnHoveringOverExit;
        public event Action<ClusterView, LetterView> OnReleasedCluster;

        public ClusterListAdapter(EnumerableView<ClusterView> view, Transform dragArea) : base(view)
        {
            _dragArea = dragArea;
        }

        #region public methods

        public void UpdateDataset(IEnumerable<string> clusters)
        {
            _clusters = clusters is List<string> list ? list : new List<string>(clusters);
            NotifyDatasetChanged(_clusters.Count);
            LayoutRebuilder.ForceRebuildLayoutImmediate(EnumerableView.RectTransform);
        }

        #endregion

        #region protected methods

        protected override void OnViewHolderCreated(ClusterView view)
        {
            view.OnDragBegin += HandleDragBegin;
            view.OnDragEnd += HandleDragEnd;
            view.OnHoveringOverEnter += HandleHoveringOverEnter;
            view.OnHoveringOverExit += HandleHoveringOverExit;
        }

        protected override void OnBeforeBinding()
        {
            EnumerableView.ReleaseAllViews();
        }
        
        protected override void OnBindViewHolder(ClusterView view, int position)
        {
            view.SetWord(_clusters[position]);
        }

        protected override void OnViewHolderDestroyed(ClusterView view)
        {
            view.OnDragBegin -= HandleDragBegin;
            view.OnDragEnd -= HandleDragEnd;
            view.OnHoveringOverEnter -= HandleHoveringOverEnter;
            view.OnHoveringOverExit -= HandleHoveringOverExit;
        }

        #endregion

        #region private methods

        private void HandleDragBegin(ClusterView view)
        {
            view.RectTransform.SetParent(_dragArea);
        }

        private void HandleHoveringOverEnter(ClusterView clusterView, LetterView letterView)
        {
            OnHoveringOverEnter?.Invoke(clusterView, letterView);
        }

        private void HandleHoveringOverExit(ClusterView clusterView, LetterView letterView)
        {
            OnHoveringOverExit?.Invoke(clusterView, letterView);
        }

        private void HandleDragEnd(ClusterView clusterView, View view)
        {
            clusterView.SnapBack();
            clusterView.RectTransform.SetParent(EnumerableView.ViewsContainer);

            if (view is LetterView letterView)
            {
                OnReleasedCluster?.Invoke(clusterView, letterView);
            }
        }

        #endregion
    }
}