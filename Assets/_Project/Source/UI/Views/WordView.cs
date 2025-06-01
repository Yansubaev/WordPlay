using Source.UI.Settings;
using UnityEngine;
using Yans.UI.Views;

namespace Source.UI
{
    public class WordView : EnumerableView<LetterView>
    {
        [SerializeField] private string _word;
        [SerializeField] private ColorPalette _colorPalette;

        public string Word
        {
            get => _word;
            set => SetWord(value);
        }

#if UNITYEDITOR
            protected override void OnValidate()
            {
                SetWord(_word);
            }
#endif

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
                letterView.InitialColor = valid ? _colorPalette.ValidCellColor : _colorPalette.DefaultCellColor;
            }
        }
    }
}
