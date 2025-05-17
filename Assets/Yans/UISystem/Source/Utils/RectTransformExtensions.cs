using UnityEngine;

namespace Yans.UI.Utils
{
    public static class RectTransformExtensions
    {
        public static void SetWidth(this RectTransform rt, float width)
        {
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public static void SetHeight(this RectTransform rt, float width)
        {
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width);
        }

        public static void SetWidthHeight(this RectTransform rt, float width, float height)
        {
            rt.SetWidth(width);
            rt.SetHeight(height);
        }

        public static void SetWidthHeight(this RectTransform rt, Vector2 size)
        {
            rt.SetWidth(size.x);
            rt.SetHeight(size.y);
        }

        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }
 
        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }
 
        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }
 
        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
    }
}