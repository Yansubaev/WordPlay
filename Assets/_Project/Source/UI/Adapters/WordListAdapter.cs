using System.Collections.Generic;
using UnityEngine.UI;
using Yans.UI.Adapters;
using Yans.UI.Views;

namespace Source.UI.Adapters
{
    public class WordListAdapter : ListAdapter<WordView>
    {
        private List<string> _words;

        public WordListAdapter(EnumerableView<WordView> view) : base(view)
        {
        }

        public void UpdateDataset(IEnumerable<string> words)
        {
            _words = words is List<string> list ? list : new List<string>(words);
            NotifyDatasetChanged(_words.Count);
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.RectTransform);
        }

        protected override void OnBindView(WordView view, int position)
        {
            view.SetWord(_words[position]);
        }
    }
}