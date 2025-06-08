using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Source.Infrastructure.SceneManagement
{
    /// <summary>
    /// This class is responsible for loading scenes and managing scene transitions.
    /// </summary>

    public class SceneLoaderService : ISceneLoaderService
    {
        #region private fields
        private readonly ZenjectSceneLoader _sceneLaoder;
        string _currentScene;
        #endregion

        public SceneLoaderService(ZenjectSceneLoader sceneLoader)
        {
            _sceneLaoder = sceneLoader;
            _currentScene = default;
        }

        #region public methods

        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public async UniTask LoadSceneAsync(
            string sceneName,
            LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default)
        {
            var oldScene = _currentScene;

            var op = _sceneLaoder.LoadSceneAsync(sceneName, mode);
            op.allowSceneActivation = true;

            await op.ToUniTask(cancellationToken: cancellationToken);
            _currentScene = sceneName;
        }

        /// <summary>
        /// Unloads a scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to unload.</param>
        public async UniTask UnloadScene(string sceneName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new System.ArgumentException("Scene name cannot be null or empty.", nameof(sceneName));
            }

            var op = SceneManager.UnloadSceneAsync(sceneName);
            
            await op.ToUniTask(cancellationToken: cancellationToken);

            if (_currentScene == sceneName)
            {
                _currentScene = default;
            }
        }

        #endregion
    }
}