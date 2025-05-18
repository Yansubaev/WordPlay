using System.Collections.Generic;
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
            view.SetWord(_clusters[position]);
            view.OnDragBegin += HandleDragBegin;
            view.OnDragEnd += HandleDragEnd;
        }

        #endregion

        #region private methods

        private void HandleDragBegin(ClusterView view)
        {
            view.RectTransform.SetParent(_dragArea);
            Debug.Log($"Drag started: {view.name}");
        }

        private void HandleDragEnd(ClusterView view1, View view2)
        {
            Debug.Log($"Drag ended on: {view1.name}, {view2?.name ?? "null"}");

                view1.SnapBack();
                view1.RectTransform.SetParent(View.ViewsContainer);
            if (view2 == null)
            {
            }
            else
            {
                // view1.RectTransform.SetParent(View.ViewsContainer);
            }
        }

        #endregion
    }
}