using NPG.Codebase.Game.Gameplay.UI.Root;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

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

        public void CreateUIRoot(AssetReference uiRootReference)
        {
            DestroyUIRoot();
            
            var prefab = PrefabProvider.LoadPrefab(uiRootReference);
            
            var instance = _container.InstantiatePrefab(prefab);

             _uiRootBinder = instance.GetComponent<UIRootBinder>();
            
            _uiRootBinder.Bind(_uiRootViewModel);
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