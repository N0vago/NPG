using NPG.Codebase.Game.Gameplay.UI.Factories;
using ObservableCollections;
using R3;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Root
{
    public class UIRootBinder : MonoBehaviour
    {
        private WindowsFactory _windowsFactory;
        
        private UIRootViewModel _viewModel;
        
        private readonly CompositeDisposable _subscriptions = new();
        
        [Inject]
        public void Construct(WindowsFactory windowsFactory)
        {
            _windowsFactory = windowsFactory;
        }
        
        public void Bind(UIRootViewModel viewModel)
        {
            _viewModel = viewModel;
            
            _subscriptions.Add(_viewModel.OpenedScreen.Subscribe(_viewModel.OpenScreen));

            foreach (var openedWindow in _viewModel.OpenedWindows)
            {
                _ = _windowsFactory.OpenWindow(openedWindow, transform);
            }
            
            _subscriptions.Add(_viewModel.OpenedWindows.ObserveAdd().Subscribe(e =>
            {
                _ = _windowsFactory.OpenWindow(e.Value, transform);
            }));

            _subscriptions.Add(_viewModel.OpenedWindows.ObserveRemove().Subscribe(e =>
            {
                _windowsFactory.CloseWindow(e.Value);
            }));
            
            OnBind(_viewModel);
        }

        private void OnBind(UIRootViewModel viewModel)
        {
            
        }

        private void OnDestroy()
        {
            _subscriptions.Dispose();
        }
    }
}