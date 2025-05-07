using Cysharp.Threading.Tasks;
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

        public UniTask Enter()
        {
            return _stateMachine.Enter<LoadMainSceneState>();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
