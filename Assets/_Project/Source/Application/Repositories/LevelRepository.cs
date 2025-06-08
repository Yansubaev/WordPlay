using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Source.Domain.Entities;
using Source.Domain.Repositories;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using Zenject;

namespace Source.Application.Repositories
{
    public class LevelRepository : BaseRepository, ILevelRepository
    {
        private readonly string _addressTemplate;

        public LevelRepository([Inject(Id = "addressTemplate")] string addressTemplate)
        {
            _addressTemplate = addressTemplate;
        }

        public async UniTask<bool> IsLevelAvaialble(string levelId, CancellationToken ct = default)
        {
            try
            {
                string address = string.Format(_addressTemplate, levelId);

                // Check if the address exists in Addressables
                var locationsHandle = Addressables.LoadResourceLocationsAsync(address);
                await locationsHandle.ToUniTask(cancellationToken: ct);

                if (locationsHandle.Status != AsyncOperationStatus.Succeeded || locationsHandle.Result.Count == 0)
                {
                    Addressables.Release(locationsHandle);
                    return false; // Not found in Addressables
                }
                Addressables.Release(locationsHandle);

                // Check if the asset is cached and up-to-date (download size == 0 means cached and no update needed)
                var sizeHandle = Addressables.GetDownloadSizeAsync(address);
                await sizeHandle.ToUniTask(cancellationToken: ct);
                bool isCached = sizeHandle.Status == AsyncOperationStatus.Succeeded && sizeHandle.Result == 0;
                Addressables.Release(sizeHandle);

                return isCached;
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        public async UniTask<AddressableResult<LevelData>> LoadLevel(string levelId, Action<float> onProgress = null, CancellationToken ct = default)
        {
            AsyncOperationHandle<TextAsset> handle = default;
            try
            {
                handle = Addressables.LoadAssetAsync<TextAsset>(string.Format(_addressTemplate, levelId));

                while (!handle.IsDone)
                {
                    onProgress?.Invoke(handle.PercentComplete);
                    await UniTask.Yield();
                }

                if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
                    throw new Exception($"Failed to load level: {levelId} from Addressables.");

                var json = handle.Result.text;
                var obj = JsonConvert.DeserializeObject<LevelData>(json);
                return new AddressableResult<LevelData>(obj);
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogException(ex);
                return new AddressableResult<LevelData>(AddressableStatus.Cancelled);
            }
            catch (InvalidKeyException ex)
            {
                Debug.LogException(ex);
                return new AddressableResult<LevelData>(AddressableStatus.InvalidKey);
            }
            catch (RemoteProviderException ex)
            {
                Debug.LogException(ex);
                return new AddressableResult<LevelData>(AddressableStatus.NetworkError);
            }
            catch (FormatException ex)
            {
                Debug.LogException(ex);
                return new AddressableResult<LevelData>(AddressableStatus.InvalidKey);
            }
            catch (JsonException ex)
            {
                Debug.LogException(ex);
                return new AddressableResult<LevelData>(AddressableStatus.Failed);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return new AddressableResult<LevelData>(AddressableStatus.Failed);
            }
            finally
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
        }
    }
}