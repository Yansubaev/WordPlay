using System.Collections.Generic;
using UnityEngine;

namespace Source.UI.Transitions
{
    [CreateAssetMenu(menuName = "UI/TransitionConfig")]
    public class TransitionConfig : ScriptableObject
    {
        public List<ScreenTransitionEntry> Transitions;
    }
}
