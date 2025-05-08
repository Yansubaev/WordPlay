using System;
using System.Collections.Generic;
using Yans.UI.Screen;
using Yans.UI.Transitions;

namespace Source.UI.Transitions
{
    public class TransitionResolver : ITransitionResolver
    {
        private readonly Dictionary<(Type from, Type to), ITransition> _transitions = new();

        public void Register<TFrom, TTo>(ITransition transition) where TFrom : UIPanel where TTo : UIPanel
        {
            _transitions[(typeof(TFrom), typeof(TTo))] = transition;
        }

        public ITransition Resolve(Type from, Type to)
        {
            return _transitions.TryGetValue((from, to), out var transition)
                ? transition
                : new TransitionFade(); // fallback to default transition
        }
    }
}
