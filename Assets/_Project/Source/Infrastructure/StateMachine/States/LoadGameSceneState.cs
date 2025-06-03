using Cysharp.Threading.Tasks;
using Source.Infrastructure.SceneManagement;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class LoadGameSceneState : IState
    {
        #region private fields
        private ISceneLoaderService _sceneLoader;
        private GameStateMachine _stateMachine;
        #endregion

        [Inject]
        public void Inject(ISceneLoaderService sceneLoader, GameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _stateMachine = stateMachine;
        }

        #region public methods

        public async UniTask Enter()
        {
            await _sceneLoader.LoadSceneAsync("Game");

            await _stateMachine.Enter<GameState>();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        #endregion
    }
}