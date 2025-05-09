using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Views;

namespace Source.UI
{
    public class WordListView : EnumerableView<WordView>
    {
        [SerializeField] private List<string> _words;

        protected override void OnValidate()
        {
            SetWords(_words);
        }

        public void SetWords(IEnumerable<string> words)
        {
            _words = words is List<string> wordList ? wordList : words.ToList();
            ReleaseAllViews();
            for (int i = 0; i < words.Count(); i++)
            {
                var wordView = this[i];
                wordView.Word = words.ElementAt(i);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}
