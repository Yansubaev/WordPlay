using UnityEngine;
using Zenject;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;

namespace Source.Infrastructure
{
    /// <summary>
    /// This class is responsible for bootstrapping the game. It initializes the game and sets up the necessary components.
    /// </summary>
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        [Inject]
        public void Inject(GameStateMachine stateMachine)
        {
            Debug.Log("GameBootstrapper: Injecting GameStateMachine");
            _stateMachine = stateMachine;
        }

        private void Start()
        {
            Debug.Log("GameBootstrapper: Starting the game bootstrapper");
            // Initialize the state machine with the bootstrap state
            _stateMachine.Enter<BootstrapState>();
        }
    }

}
