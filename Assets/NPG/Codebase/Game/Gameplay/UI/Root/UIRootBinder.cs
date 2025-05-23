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
            _subscriptions.Add(viewModel.OpenedScreen.Subscribe(viewModel.OpenScreen));

            foreach (var openedWindow in viewModel.OpenedWindows)
            {
                _ = _windowsFactory.OpenWindow(openedWindow);
            }
            
            _subscriptions.Add(viewModel.OpenedWindows.ObserveAdd().Subscribe(e =>
            {
                _ = _windowsFactory.OpenWindow(e.Value);
            }));

            _subscriptions.Add(viewModel.OpenedWindows.ObserveRemove().Subscribe(e =>
            {
                _windowsFactory.CloseWindow(e.Value);
            }));
            
            OnBind(viewModel);
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