using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace NPG.Codebase.Game.Gameplay.UI.Factories
{
    public class WindowsFactory
    {
        private readonly UIRootBinder _uiRootBinder;
        
        private Dictionary<WindowViewModel, WindowBinder> _windows = new();
        
        public WindowsFactory(UIRootBinder uiRootBinder)
        {
            _uiRootBinder = uiRootBinder;
        }
        public async UniTaskVoid OpenWindow(WindowViewModel window)
        {
            var prefab = Addressables.LoadAssetAsync<WindowBinder>(window.Id);
            await prefab.Task;
            var instance = Object.Instantiate(prefab.Result, _uiRootBinder.transform);
            instance.Bind(window);
            _windows.Add(window, instance);
        }

        public void CloseWindow(WindowViewModel window)
        {
            var binder = _windows[window];
            binder?.Close();
            _windows.Remove(window);
        }
    }
    
}