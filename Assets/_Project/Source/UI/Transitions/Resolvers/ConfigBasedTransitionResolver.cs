using System;
using System.Collections.Generic;
using Source.Infrastructure.UI.Transitions;

namespace Source.UI.Transitions
{
    public class ConfigBasedTransitionResolver : ITransitionResolver
    {
        private readonly Dictionary<(string from, string to), ITransition> _map = new();

        public ConfigBasedTransitionResolver(TransitionConfig config, Dictionary<TransitionType, ITransition> registry)
        {
            foreach (var entry in config.Transitions)
            {
                var key = (entry.From, entry.To);
                _map[key] = registry[entry.Type];
            }
        }

        public ITransition Resolve(Type from, Type to)
        {
            var key = (from?.Name ?? "null", to?.Name ?? "null");
            return _map.TryGetValue(key, out var transition)
                ? transition
                : _map.TryGetValue(("null", to?.Name ?? "null"), out var fallback)
                    ? fallback
                    : new TransitionEmpty();
        }
    }
}
