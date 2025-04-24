using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Source.Infrastructure.SceneManagement
{

    /// <summary>
    /// This class is responsible for loading scenes and managing scene transitions.
    /// </summary>

    public class SceneLoaderService : ISceneLoaderService
    {
        private const string SceneKeyTemplate = "Scenes/{0}";

        private IDictionary<string, SceneInstance> _loadedScenes = new Dictionary<string, SceneInstance>();

        SceneInstance _currentScene;

        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, CancellationToken cancellationToken = default)
        {
            if (mode == LoadSceneMode.Single && _currentScene.Scene.IsValid())
            {
                await Addressables.UnloadSceneAsync(_currentScene, true);
                _loadedScenes.Remove(_currentScene.Scene.name);
            }

            _currentScene = await Addressables
                .LoadSceneAsync(string.Format(SceneKeyTemplate, sceneName), mode)
                .ToUniTask(cancellationToken: cancellationToken);
                
            _loadedScenes[sceneName] = _currentScene;
        }

        /// <summary>
        /// Unloads a scene asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to unload.</param>
        public async UniTask UnloadScene(string sceneName, CancellationToken cancellationToken = default)
        {
            if (_loadedScenes.TryGetValue(sceneName, out SceneInstance sceneInstance))
            {
                _loadedScenes.Remove(sceneName);
                await Addressables.UnloadSceneAsync(sceneInstance, true).ToUniTask(cancellationToken: cancellationToken);
            }
            else
            {
                Debug.LogWarning($"Scene {sceneName} is not loaded.");
            }
        }
    }
}
