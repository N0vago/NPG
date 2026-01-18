using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace NPG.Codebase.Infrastructure.Services
{
    public class SceneLoader 
    {
        public bool IsLoading { get; private set; }
        public async UniTask LoadSceneAsync(int sceneIndex, Action onComplete = null)
        {
            if (sceneIndex == SceneManager.GetActiveScene().buildIndex)
                return;

            if (IsLoading) 
                return;

            IsLoading = true;
            try
            {
                await SceneManager.LoadSceneAsync(sceneIndex).ToUniTask();
                onComplete?.Invoke();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}