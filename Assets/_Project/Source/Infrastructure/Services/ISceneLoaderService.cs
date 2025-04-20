using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Source.Infrastructure.SceneManagement
{
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(string sceneKey, LoadSceneMode mode = LoadSceneMode.Single, CancellationToken cancellationToken = default);
        UniTask UnloadScene(string sceneName, CancellationToken cancellationToken = default);
    }
}
