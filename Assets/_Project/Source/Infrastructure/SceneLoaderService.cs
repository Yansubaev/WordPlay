using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Source.Infrastructure.SceneManagement
{

    /// <summary>
    /// This class is responsible for loading scenes and managing scene transitions.
    /// </summary>

    public class SceneLoaderService
    {
        SceneInstance _currentScene;

        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public async UniTask LoadSceneAsync(string sceneKey, LoadSceneMode mode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
        {
            if (mode == LoadSceneMode.Single && _currentScene.Scene.IsValid())
            {
                await Addressables.UnloadSceneAsync(_currentScene, true);
            }

            _currentScene = await Addressables.LoadSceneAsync(sceneKey, mode).ToUniTask(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// Unloads a scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to unload.</param>
        public void UnloadScene(string sceneName)
        {
            // Implement scene unloading logic here
            // For example, using Unity's SceneManager.UnloadSceneAsync
        }
    }
}
