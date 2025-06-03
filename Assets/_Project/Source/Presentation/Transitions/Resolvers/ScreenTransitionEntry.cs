using System;
using UnityEngine;

namespace Source.Presentation.Transitions
{
    #region public classes

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
        public abstract Type GetScreenType();
    }

    #endregion 
}