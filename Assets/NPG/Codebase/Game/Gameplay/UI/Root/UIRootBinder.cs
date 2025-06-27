using NPG.Codebase.Game.Gameplay.UI.Factories;
using NPG.Codebase.Game.Gameplay.UI.HUD;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment;
using ObservableCollections;
using R3;
using UnityEngine;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Root
{
    public class UIRootBinder : MonoBehaviour
    {
        [SerializeField] private HUDBinder hudBinder;
        
        private WindowsFactory _windowsFactory;
        
        private UIRootViewModel _viewModel;
        
        private readonly CompositeDisposable _subscriptions = new();
        
        //Remove this field next time you change the code !!!IMPORTANT!!!
        private EquipmentWindowViewModel _equipmentWindowViewModel;
        
        [Inject]
        public void Construct(WindowsFactory windowsFactory, EquipmentWindowViewModel equipmentWindowViewModel)
        {
            _windowsFactory = windowsFactory;
            _equipmentWindowViewModel = equipmentWindowViewModel;
        }
        
        public void Bind(UIRootViewModel viewModel)
        {
            _viewModel = viewModel;
            
            _subscriptions.Add(_viewModel.OpenedScreen.Subscribe(hudViewModel =>
            {
                hudBinder.Bind(hudViewModel);
            }));
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