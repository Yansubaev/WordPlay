using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Views;

namespace Source.UI
{
    public class ClusterListView : EnumerableView<ClusterView>
    {
        [SerializeField] private List<string> _clusters;

        protected override void OnValidate()
        {
            SetClusters(_clusters);
        }

        public void SetClusters(IEnumerable<string> clusters)
        {
            _clusters = clusters is List<string> clusterList ? clusterList : clusters.ToList();
            ReleaseAllViews();
            for (int i = 0; i < clusters.Count(); i++)
            {
                var clusterView = this[i];
                clusterView.Word = clusters.ElementAt(i);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}
