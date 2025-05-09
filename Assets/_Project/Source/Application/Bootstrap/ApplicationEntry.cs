using Cysharp.Threading.Tasks;
using Source.Infrastructure.StateMachine;
using Source.Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Source.Infrastructure
{
    /// <summary>
    /// This class is responsible for bootstrapping the game. It initializes the game and sets up the necessary components.
    /// </summary>
    public class ApplicationEntry : MonoBehaviour
    {
        #region private fields
        private GameStateMachine _stateMachine;
        #endregion

        #region public methods

        [Inject]
        public void Inject(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        #endregion

        #region private methods

        private async void Start()
        {
            Debug.Log("ApplicationEntry: Starting the application entry");

            var t = await Addressables.InitializeAsync(true);

            // Initialize the state machine with the bootstrap state
            _stateMachine.Enter<BootstrapState>().Forget();
        }

        #endregion
    }
}