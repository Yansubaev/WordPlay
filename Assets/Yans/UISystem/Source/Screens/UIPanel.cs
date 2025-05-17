using System;
using UnityEngine;
using Yans.UI.Utils;

namespace Yans.UI.UIScreens
{
    public abstract class UIPanel : UIScreen
    {
        #region public fields
        public RectTransform Wrapper;
        #endregion

        #region private fields

        [SerializeField]
        private ScreenSide _useSafeAreaOn = ScreenSide.Left | ScreenSide.Right | ScreenSide.Top | ScreenSide.Bottom;

        [SerializeField]
        private RectTransform _wrapper;

        #endregion

        public ScreenSide UseSafeAreaOn
        {
            get => _useSafeAreaOn;
            set
            {
                _useSafeAreaOn = value;
                ResizePanelToSafeArea(Screen.safeArea, new Vector2(Screen.width, Screen.height));
            }
        }

        #region protected methods

        protected override void OnStarted()
        {
            base.OnStarted();
            ResizePanelToSafeArea(Screen.safeArea, new Vector2(Screen.width, Screen.height));
        }

        protected virtual void ResizePanelToSafeArea(Rect safeArea, Vector2 screenSize)
        {
            if (_wrapper == null) return;
            var scale = 1 / Canvas.scaleFactor;

            if (_useSafeAreaOn.HasFlag(ScreenSide.Left))
                _wrapper.SetLeft(safeArea.x * scale);

            if (_useSafeAreaOn.HasFlag(ScreenSide.Right))
                _wrapper.SetRight((screenSize.x - safeArea.width - safeArea.x) * scale);

            if (_useSafeAreaOn.HasFlag(ScreenSide.Bottom))
                _wrapper.SetBottom(safeArea.y * scale);

            if (_useSafeAreaOn.HasFlag(ScreenSide.Top))
                _wrapper.SetTop((screenSize.y - safeArea.height - safeArea.y) * scale);
        }

        #endregion

        [Flags]
        public enum ScreenSide
        {
            None = 0,
            Left = 1 << 0,
            Right = 1 << 1,
            Top = 1 << 2,
            Bottom = 1 << 3
        }
    }
}