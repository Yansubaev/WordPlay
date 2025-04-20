using System;

namespace Source.Infrastructure.UI.Transitions
{
    public interface ITransitionResolver
    {
        ITransition Resolve(Type from, Type to);
    }
}
