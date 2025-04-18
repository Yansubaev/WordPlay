using System;
using System.Collections.Generic;
using Source.Infrastructure.StateMachine.States;
using Zenject;

namespace Source.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private IState _activeState;

        [Inject]
        public void Inject(BootstrapState bootstrap, LoadMainMenuState loadMainMenu)
        {
            _states[typeof(BootstrapState)] = bootstrap;
            _states[typeof(LoadMainMenuState)] = loadMainMenu;
        }

        public void Enter<TState>() where TState : IState
        {
            _activeState?.Exit();
            _activeState = _states[typeof(TState)];
            _activeState.Enter();
        }
    }

}
