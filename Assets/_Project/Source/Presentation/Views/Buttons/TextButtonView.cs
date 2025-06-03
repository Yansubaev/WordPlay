using TMPro;
using UnityEngine;

namespace Source.Presentation.Views
{
    public class TextButtonView : ButtonView
    {
        [SerializeField] private TextMeshProUGUI _text;

        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
    }
}
