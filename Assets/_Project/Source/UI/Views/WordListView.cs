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
        [SerializeField] private LayoutGroup _layoutGroup;
        [SerializeField] private ContentSizeFitter _contentSizeFitter;

        public List<string> Words
        {
            get => _words;
            set => SetWords(value);
        }

        public LayoutGroup LayoutGroup => _layoutGroup;

        public ContentSizeFitter ContentSizeFitter => _contentSizeFitter;

#if UNITY_EDITOR
            protected override void OnValidate()
            {
                SetWords(_words);
            }
    
#endif
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
