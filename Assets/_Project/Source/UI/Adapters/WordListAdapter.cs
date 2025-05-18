using UnityEngine.UI;
using Yans.UI.Adapters;
using Yans.UI.Views;

namespace Source.UI.Adapters
{

    public class WordListAdapter : ListAdapter<WordView>
    {
        private string[,] _words;

        public WordListAdapter(EnumerableView<WordView> view) : base(view)
        {
        }

        public void UpdateDataset(string[,] words)
        {
            _words = words;
            NotifyDatasetChanged(_words.GetLength(0));
            LayoutRebuilder.ForceRebuildLayoutImmediate(View.RectTransform);
        }

        protected override void OnBindView(WordView view, int position)
        {
            // Get all entries from the row at position
            int columns = _words.GetLength(1);
            string[] wordRow = new string[columns];

            for (int i = 0; i < columns; i++)
            {
                wordRow[i] = _words[position, i];
            }

            view.SetWord(wordRow);
        }
    }
}