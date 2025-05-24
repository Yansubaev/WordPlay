using UnityEngine;
using Yans.UI.Views;

namespace Source.UI
{
    public class WordView : EnumerableView<LetterView>
    {
        [SerializeField] private string _word;

        public string Word
        {
            get => _word;
            set => SetWord(value);
        }

        protected override void OnValidate()
        {
            SetWord(_word);
        }

        public void SetWord(string word)
        {
            _word = word;
            ReleaseAllViews();
            for (int i = 0; i < word.Length; i++)
            {
                var letterView = this[i];
                letterView.Character = word[i];
            }
        }

        public void SetWord(string[] word, bool valid = false)
        {
            _word = string.Join("", word);
            ReleaseAllViews();
            for (int i = 0; i < word.Length; i++)
            {
                var letterView = this[i];
                letterView.Letter = word[i];
                letterView.Color = valid ? Color.green : Color.white;
            }
        }
    }
}
