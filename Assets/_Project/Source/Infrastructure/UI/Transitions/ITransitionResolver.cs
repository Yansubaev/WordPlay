using System;

namespace Source.Infrastructure.UI.Transitions
{
    public interface ITransitionResolver
    {
        void Register<TFrom, TTo>(ITransition transition) where TFrom : UIScreen where TTo : UIScreen;
        ITransition Resolve(Type from, Type to);
    }
}
