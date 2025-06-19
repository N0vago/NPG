using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Root
{
    [RequireComponent(typeof(UIRootBinder))]
    public class UIController : MonoBehaviour
    {
        private UIRootViewModel _uiRootViewModel;
        
        private InputActions _inputActions;
        
        private InputAction _openEquipmentAction;
        
        private EquipmentWindowViewModel _equipmentWindowViewModel = new EquipmentWindowViewModel();
        
        [Inject]
        public void Construct(InputActions inputActions, UIRootViewModel uiRootViewModel)
        {
            _inputActions = inputActions;
            _uiRootViewModel = uiRootViewModel;
        }
        private void OnEnable()
        {
            _openEquipmentAction = _inputActions.UI.OpenEquipment;
            _openEquipmentAction.Enable();
            _openEquipmentAction.performed += OnOpenEquipment;
        }

        private void OnOpenEquipment(InputAction.CallbackContext obj)
        {
            if (!_uiRootViewModel.OpenedWindows.Contains(_equipmentWindowViewModel))
            {
                _uiRootViewModel.OpenWindow(_equipmentWindowViewModel);
                _inputActions.Player.Disable();
            }
            else
            {
                _equipmentWindowViewModel.RequestClose();
                _equipmentWindowViewModel = new EquipmentWindowViewModel();
                _inputActions.Player.Enable();
            }
        }
    }
}