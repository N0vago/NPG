using Cysharp.Threading.Tasks;
using NPG.Codebase.Game.Gameplay.UI.Root;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace NPG.Codebase.Game.Gameplay.UI.Factories
{
    public class UIRootFactory
    {
        private UIRootBinder _uiRootBinder;
        private UIRootViewModel _uiRootViewModel = new UIRootViewModel();

        private DiContainer _container;

        public UIRootFactory(DiContainer container)
        {
            _container = container;
        }

        public async UniTaskVoid CreateUIRoot(string addressableName)
        {
            DestroyUIRoot();
            
            var prefab = Addressables.LoadAssetAsync<UIRootBinder>(addressableName);
            await prefab.Task;
            var instance = Object.Instantiate(prefab.Result);
            
            _container.Inject(instance);
            
            instance.Bind(_uiRootViewModel);
        }

        public void DestroyUIRoot()
        {
            if (_uiRootBinder != null)
            {
                Object.Destroy(_uiRootBinder.gameObject);
                _uiRootViewModel.Dispose();
            }
        }
    }
}