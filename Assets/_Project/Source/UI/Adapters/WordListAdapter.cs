using Cysharp.Threading.Tasks;
using Source.UI.Settings;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Adapters;

namespace Source.UI.Adapters
{
    public class WordListAdapter : ListAdapter<WordView>
    {
        #region private fields
        private readonly ColorPalette _colorPalette;
        private readonly LayoutGroup _layoutGroup;
        private readonly ContentSizeFitter _contentSizeFitter;
        private string[,] _words;
        private int[] _validatedWords;
        #endregion

        public event Action<string, int, int> OnClusterPlaced;

        public WordListAdapter(WordListView wordListView, ColorPalette colorPallet) : base(wordListView)
        {
            _layoutGroup = wordListView.LayoutGroup;
            _contentSizeFitter = wordListView.ContentSizeFitter;
            _colorPalette = colorPallet;
        }

        #region public methods

        public void UpdateDataset(string[,] words, int[] validatedWords = null)
        {
            _words = words;
            _validatedWords = validatedWords ?? Array.Empty<int>();
            NotifyDatasetChanged(_words.GetLength(0));
        }

        public void HandleHoveringOverEnter(ClusterView clusterView, LetterView letterView)
        {
            PaintViewsFromLeadingView(clusterView, letterView, _colorPalette.HoverCellColor);
        }

        public void HandleHoveringOverExit(ClusterView clusterView, LetterView letterView)
        {
            PaintViewsFromLeadingView(clusterView, letterView, _colorPalette.DefaultCellColor);
        }

        public void HandleReleasedCluster(ClusterView clusterView, LetterView letterView)
        {
            var wordIndex = letterView.ParentEnumerableView.RectTransform.GetSiblingIndex();
            var letterIndex = letterView.AdapterPosition;

            OnClusterPlaced?.Invoke(clusterView.Word, wordIndex, letterIndex);
        }

        #endregion

        #region protected methods

        protected override void OnBeforeBinding()
        {
            _layoutGroup.enabled = true;
            _contentSizeFitter.enabled = true;
        }

        protected override void OnBindViewHolder(WordView view, int position)
        {
            int columns = _words.GetLength(1);
            string[] wordRow = new string[columns];

            for (int i = 0; i < columns; i++)
            {
                wordRow[i] = _words[position, i];
            }

            view.SetWord(wordRow, _validatedWords.Contains(position));
        }

        protected async override void OnAfterBinding()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(EnumerableView.RectTransform);

            await UniTask.Yield();

            _layoutGroup.enabled = false;
            _contentSizeFitter.enabled = false;
        }

        #endregion

        #region private methods

        private void PaintViewsFromLeadingView(ClusterView clusterView, LetterView letterView, Color color)
        {
            var clusterSize = clusterView.Count;
            var letterPos = letterView.AdapterPosition;
            var letterParent = letterView.ParentEnumerableView;
            var rangeEnd = Mathf.Min(letterPos + clusterSize, letterParent.Count);

            for (int i = letterPos; i < rangeEnd; i++)
            {
                var letter = letterParent[i];
                letter.Color = color;
            }

        }

        #endregion
    }
}