using Source.UI.Screens;
using System;
using UnityEngine;

namespace Source.UI.Transitions
{
    [Serializable]
    public class ScreenTransitionEntry
    {
        #region public fields

        [SerializeReference]
        public UIScreenType From;

        [SerializeReference]
        public UIScreenType To;

        public TransitionType Type;
        #endregion
    }

    [Serializable]
    public abstract class UIScreenType
    {
        #region public methods
        public abstract Type GetScreenType();
        #endregion
    }
}