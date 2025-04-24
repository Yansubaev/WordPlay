using Cysharp.Threading.Tasks;

namespace Source.Infrastructure.StateMachine.States
{
    public interface IState
    {
        public UniTask Enter();
        public UniTask Exit();
    }

}
