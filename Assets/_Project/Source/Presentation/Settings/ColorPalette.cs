using UnityEngine;

namespace Source.Presentation.Settings
{
    
    [CreateAssetMenu(menuName = "WordPlay/ColorPallet", fileName = "ColorPallet", order = 0)]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private Color _defaultCellColor = Color.white;
        [SerializeField] private Color _hoverCellColor = Color.cyan;
        [SerializeField] private Color _validCellColor = Color.green;
        [SerializeField] private Color _invalidCellColor = Color.red;

        public Color DefaultCellColor => _defaultCellColor;
        public Color HoverCellColor => _hoverCellColor;
        public Color ValidCellColor => _validCellColor;
        public Color InvalidCellColor => _invalidCellColor;
    }
}
