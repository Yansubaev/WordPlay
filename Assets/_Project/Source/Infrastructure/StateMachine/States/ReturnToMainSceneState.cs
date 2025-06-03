using Cysharp.Threading.Tasks;
using Source.Infrastructure.SceneManagement;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class ReturnToMainSceneState : IState
    {
        private GameStateMachine _stateMachine;
        private ISceneLoaderService _sceneLoader;

        [Inject]
        public void Inject(GameStateMachine stateMachine, ISceneLoaderService sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Enter()
        {
            await _sceneLoader.LoadSceneAsync("Main");
            await _stateMachine.Enter<MainMenuState>();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}