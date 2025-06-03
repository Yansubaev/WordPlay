using System;
using System.Collections.Generic;
using Yans.UI.Transitions;

namespace Source.Presentation.Transitions
{
    public class ConfigBasedTransitionResolver : ITransitionResolver
    {
        private readonly Dictionary<(Type from, Type to), ITransition> _map = new();

        public ConfigBasedTransitionResolver(TransitionConfig config, Dictionary<TransitionType, ITransition> registry)
        {
            foreach (var entry in config.Transitions)
            {
                var key = (entry.From.GetScreenType(), entry.To.GetScreenType());
                _map[key] = registry[entry.Type];
            }
        }

        public ITransition Resolve(Type from, Type to)
        {
            var key = (from, to);
            return _map.TryGetValue(key, out var transition)
                ? transition
                : new TransitionEmpty();
        }
    }
}
