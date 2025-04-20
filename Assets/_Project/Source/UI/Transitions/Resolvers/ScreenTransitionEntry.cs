using System;

namespace Source.UI.Transitions
{
    [Serializable]
    public class ScreenTransitionEntry
    {
        public string From;
        public string To;
        public TransitionType Type;
    }
}
