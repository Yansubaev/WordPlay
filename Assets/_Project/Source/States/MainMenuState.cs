using Cysharp.Threading.Tasks;
using Source.Signals;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{

    public class MainMenuState : IState
    {
        private SignalBus _signalBus;
        private GameStateMachine _stateMachine;

        [Inject]
        public void Inject(SignalBus signalBus, GameStateMachine stateMachine)
        {
            _signalBus = signalBus;
            _stateMachine = stateMachine;
        }

        public UniTask Enter()
        {
            _signalBus.Fire<ShowMainMenuSignal>();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        public void StartGame()
        {
            _stateMachine.Enter<LoadGameSceneState>().Forget();
        }
    }
}
