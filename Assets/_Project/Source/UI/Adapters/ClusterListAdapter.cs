using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Adapters;
using Yans.UI.Views;

namespace Source.UI.Adapters
{
    public class ClusterListAdapter : ListAdapter<ClusterView>
    {
        #region private fields
        private List<string> _clusters;
        private readonly Transform _dragArea;
        #endregion

        public ClusterListAdapter(EnumerableView<ClusterView> view, Transform dragArea) : base(view)
        {
            _dragArea = dragArea;
        }

        #region public methods

        public void UpdateDataset(IEnumerable<string> clusters)
        {
            _clusters = clusters is List<string> list ? list : new List<string>(clusters);
            NotifyDatasetChanged(_clusters.Count);
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.RectTransform);
        }

        #endregion

        #region protected methods

        protected override void OnBindView(ClusterView view, int position)
        {
            view.OnDragBegin -= HandleDragBegin;
            view.OnDragEnd -= HandleDragEnd;
            // view.OnDragging -= HandleDragging;

            view.SetWord(_clusters[position]);

            view.OnDragBegin += HandleDragBegin;
            view.OnDragEnd += HandleDragEnd;
            // view.OnDragging += HandleDragging;
        }

        #endregion

        #region private methods

        private void HandleDragBegin(ClusterView view)
        {
            view.RectTransform.SetParent(_dragArea);
        }

        private void HandleDragging(ClusterView clusterView, IEnumerable<View> views)
        {
            foreach (var view in views)
            {
                if (view is LetterView letterView)
                {
                    letterView.Color = Color.cyan;
                }
            }
        }

        private void HandleDragEnd(ClusterView clusterView, IEnumerable<View> views)
        {
            Debug.Log($"Drag ended on: {clusterView.name}, {string.Join(',', views.Select(v => v.name))}");

            clusterView.SnapBack();
            clusterView.RectTransform.SetParent(View.ViewsContainer);

            foreach (var view in views)
            {
                if (view is LetterView letterView)
                {
                    letterView.Color = Color.red;
                }
            }
        }

        #endregion
    }
}