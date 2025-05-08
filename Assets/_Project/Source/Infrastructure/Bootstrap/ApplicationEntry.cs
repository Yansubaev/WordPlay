using UnityEngine;
using Zenject;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Source.Infrastructure
{
    /// <summary>
    /// This class is responsible for bootstrapping the game. It initializes the game and sets up the necessary components.
    /// </summary>
    public class ApplicationEntry : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        [Inject]
        public void Inject(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private async void Start()
        {
            Debug.Log("ApplicationEntry: Starting the application entry");
            
            var t = await Addressables.InitializeAsync(true);
            
            // Initialize the state machine with the bootstrap state
            _stateMachine.Enter<BootstrapState>().Forget();
        }
    }

}
