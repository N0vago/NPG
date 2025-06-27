using System;
using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.HUD;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment;
using NPG.Codebase.Infrastructure.BindingRegistration;
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
        
        private EquipmentWindowViewModel _equipmentWindowViewModel;
        
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

            _equipmentWindowViewModel = new EquipmentWindowViewModel();
            
            _uiRootViewModel.OpenHUD(new HUDViewModel(new WeaponSelectorViewModel(_equipmentWindowViewModel)));
            _uiRootViewModel.OpenWindow(_equipmentWindowViewModel);
            _equipmentWindowViewModel.RequestHide();
        }
        private void OnDisable()
        {
            _openEquipmentAction.performed -= OnOpenEquipment;
            _openEquipmentAction.Disable();
            _equipmentWindowViewModel.RequestClose();
        }

        private void OnOpenEquipment(InputAction.CallbackContext obj)
        {
            if (!_equipmentWindowViewModel.IsVisible)
            {
                _equipmentWindowViewModel.RequestShow();
                _inputActions.Player.Disable();
            }
            else
            {
                _equipmentWindowViewModel.RequestHide();
                _inputActions.Player.Enable();
            }
        }
    }
}