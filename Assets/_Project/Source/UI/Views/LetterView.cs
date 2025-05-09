using TMPro;
using UnityEngine;
using Yans.UI.Views;

namespace Source.UI
{

    public class LetterView : View
    {
        [SerializeField] private TextMeshProUGUI _text;

        public string Letter
        {
            get => _text.text;
            set => _text.text = value;
        }

        public char Character
        {
            get => _text.text[0];
            set => _text.text = value.ToString();
        }
    }
}
