
using Source.Infrastructure.SceneManagement;
using Zenject;

namespace Source.Infrastructure.StateMachine.States
{
    public class LoadMainMenuState : IState
    {
        private SceneLoaderService _sceneLoader;

        [Inject]
        public void Inject(SceneLoaderService sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async void Enter()
        {
            // Load the main menu scene here
            await _sceneLoader.LoadSceneAsync("Main");
        }

        public void Exit()
        {
        }
    }
}
