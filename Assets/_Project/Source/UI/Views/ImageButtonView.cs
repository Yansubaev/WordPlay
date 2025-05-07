using Source.Infrastructure.UI.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Views
{
    public class ImageButtonView : ButtonView
    {
        [SerializeField] private Image _image;

        public Sprite Image
        {
            get => _image.sprite;
            set => _image.sprite = value;
        }
    }
}
