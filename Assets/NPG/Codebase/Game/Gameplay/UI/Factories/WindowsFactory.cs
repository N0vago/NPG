﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Game.Gameplay.UI.Factories
{
    public class WindowsFactory
    {
        private DiContainer _container;
        
        private Dictionary<WindowViewModel, WindowBinder> _windows = new();
        public Dictionary<WindowViewModel, WindowBinder> Windows => _windows;
        
        public WindowsFactory(DiContainer container)
        {
            _container = container;
        }
        public async UniTaskVoid OpenWindow(WindowViewModel window, Transform parent)
        {
            if (_windows.ContainsKey(window))
            {
                Debug.LogWarning($"Window {window.Id} is already open.");
                return;
            }
            
            var asyncHandler = PrefabProvider.LoadPrefabAsync(window.Id);

            var prefab = await asyncHandler;
            
            var instance = _container.InstantiatePrefab(prefab, parent);
            
            var binder = instance.GetComponent<WindowBinder>();
            
            binder.Bind(window);
            
            _windows.Add(window, binder);
        }

        public void CloseWindow(WindowViewModel window)
        {
            if (!_windows.TryGetValue(window, out var binder))
            {
                Debug.LogWarning($"Window {window.Id} is not open, cannot close it.");
                return;
            }

            binder?.Close();
            _windows.Remove(window);
        }
        
    }
    
}