using Source.Infrastructure.Services;
using Source.Signals;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{

    public class MainMenuState : IState
    {
        private SignalBus _signalBus;

        [Inject]
        public void Inject(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _signalBus.Fire<ShowMainMenuSignal>();
        }

        public void Exit()
        {
        }
    }
}
