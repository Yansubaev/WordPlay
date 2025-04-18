using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private GameStateMachine _stateMachine;

        [Inject]
        public void Inject(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.Enter<LoadMainMenuState>();
        }

        public void Exit()
        {
        }
    }
}
