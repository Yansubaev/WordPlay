
using Cysharp.Threading.Tasks;

namespace Source.Infrastructure.StateMachine.States
{
    public class GameState : IState
    {
        public UniTask Enter()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
