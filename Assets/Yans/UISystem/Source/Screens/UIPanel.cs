using UnityEngine;

namespace Yans.UI.Screen
{

    public abstract class UIPanel : UIScreen
    {
        [SerializeField] private RectTransform _wrapper;

        public RectTransform Wrapper;


    }
}
