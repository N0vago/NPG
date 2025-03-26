using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Codebase.Infrastructure.Services
{
    public class SceneLoader 
    {
        public UniTask LoadSceneAsync(int sceneIndex)
        {
            if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
            {
                UniTask asyncOperation = SceneManager.LoadSceneAsync(sceneIndex).ToUniTask();

                return asyncOperation;
            }

            return UniTask.CompletedTask;
        }
    }
}