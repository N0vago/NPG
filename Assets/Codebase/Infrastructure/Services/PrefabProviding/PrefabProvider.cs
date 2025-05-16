using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Codebase.Infrastructure.Services.PrefabProviding
{
    public static class PrefabProvider
    {
        public static T GetPrefab<T>(string addressableName) where T : MonoBehaviour
        {
            var handle =  Addressables.LoadAssetAsync<T>(addressableName);
            
            handle.WaitForCompletion();
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {addressableName}");
                return null;
            }
        }
        
        
        public static GameObject GetPrefab(string addressableName)
        {
            var handle =  Addressables.LoadAssetAsync<GameObject>(addressableName);
            
            handle.WaitForCompletion();
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {addressableName}");
                return null;
            }
        }
        public static T GetPrefab<T>(AssetReference reference) where T : MonoBehaviour
        {
            var handle =  Addressables.LoadAssetAsync<T>(reference);
            
            handle.WaitForCompletion();
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {reference}");
                return null;
            }
        }
        
        
        public static GameObject GetPrefab(AssetReference reference)
        {
            var handle =  Addressables.LoadAssetAsync<GameObject>(reference);
            
            handle.WaitForCompletion();
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {reference}");
                return null;
            }
        }

        public static async UniTask<T> GetPrefabAsync<T>(string addressableName) where T : MonoBehaviour
        {
            var handle = Addressables.LoadAssetAsync<T>(addressableName);
            
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {addressableName}");
                return null;
            }
        }

        public static async UniTask<GameObject> GetPrefabAsync(string addressableName)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(addressableName);
            
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {addressableName}");
                return null;
            }
        }
        
        public static async UniTask<T> GetPrefabAsync<T>(AssetReference reference) where T : MonoBehaviour
        {
            var handle = Addressables.LoadAssetAsync<T>(reference);
            
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {reference}");
                return null;
            }
        }

        public static async UniTask<GameObject> GetPrefabAsync(AssetReference reference)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(reference);
            
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load prefab at {reference}");
                return null;
            }
        }

        public static void ReleasePrefab<T>(T prefab) where T : MonoBehaviour
        {
            Addressables.Release(prefab);
        }
    }
}