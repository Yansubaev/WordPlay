
using Cysharp.Threading.Tasks;
using Source.Infrastructure.SceneManagement;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class LoadMainSceneState : IState
    {
        private ISceneLoaderService _sceneLoader;
        private GameStateMachine _stateMachine;

        [Inject]
        public void Inject(ISceneLoaderService sceneLoader, GameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
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
