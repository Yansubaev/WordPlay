using System;
using System.Collections.Generic;
using Source.UI.Transitions;

namespace Source.Infrastructure.UI.Transitions
{
    public class TransitionResolver : ITransitionResolver
    {
        private readonly Dictionary<(Type from, Type to), ITransition> _transitions = new();

        public void Register<TFrom, TTo>(ITransition transition) where TFrom : UIScreen where TTo : UIScreen
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
