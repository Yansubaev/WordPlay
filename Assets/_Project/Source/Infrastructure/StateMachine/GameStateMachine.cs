using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        public void Inject(
            BootstrapState bootstrap,
            LoadMainSceneState loadMainMenu,
            MainMenuState mainMenuState,
            LoadGameSceneState loadGameSceneState,
            GameState gameState
            )
        {
            _states[typeof(BootstrapState)] = bootstrap;
            _states[typeof(LoadMainSceneState)] = loadMainMenu;
            _states[typeof(MainMenuState)] = mainMenuState;
            _states[typeof(LoadGameSceneState)] = loadGameSceneState;
            _states[typeof(GameState)] = gameState;
        }

        public async UniTask Enter<TState>() where TState : IState
        {
            if (_activeState != null)
            {
#if LOG_FSM_EVENTS
                Debug.Log($"<color=cyan>[FSM] GameStateMachine: Exiting state: {_activeState.GetType().Name}</color>");
#endif                

                await _activeState.Exit();
            }

            _activeState = _states[typeof(TState)];
            
#if LOG_FSM_EVENTS

            Debug.Log($"<color=cyan>[FSM] GameStateMachine: Entering state: {_activeState.GetType().Name}</color>");
#endif

            await _activeState.Enter();
        }
    }

}
