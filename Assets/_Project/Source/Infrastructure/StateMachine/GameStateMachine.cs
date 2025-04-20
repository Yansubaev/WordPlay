using System;
using System.Collections.Generic;
using Source.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private IState _activeState;

        [Inject]
        public void Inject(BootstrapState bootstrap, LoadMainSceneState loadMainMenu, MainMenuState mainMenuState)
        {
            _states[typeof(BootstrapState)] = bootstrap;
            _states[typeof(LoadMainSceneState)] = loadMainMenu;
            _states[typeof(MainMenuState)] = mainMenuState;
        }

        public void Enter<TState>() where TState : IState
        {
            _activeState?.Exit();
            _activeState = _states[typeof(TState)];
            Debug.Log($"GameStateMachine: Entering state: {_activeState.GetType().Name}");
            _activeState.Enter();
        }
    }

}
