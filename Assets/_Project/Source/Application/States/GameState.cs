using Cysharp.Threading.Tasks;
using Source.Signals;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class GameState : IState
    {
        #region private fields
        private SignalBus _signalBus;
        private GameStateMachine _stateMachine;
        #endregion

        #region public methods

        [Inject]
        public void Inject(SignalBus signalBus, GameStateMachine stateMachine)
        {
            _signalBus = signalBus;
            _stateMachine = stateMachine;
        }

        public UniTask Enter()
        {
            _signalBus.Fire<StartGameSignal>();

            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        public async void ReturnToMainMenu()
        {
            await _stateMachine.Enter<LoadMainSceneState>();
        }

        #endregion
    }
}