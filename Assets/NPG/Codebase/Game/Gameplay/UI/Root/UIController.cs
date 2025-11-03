using System;
using System.Linq;
using Assets.NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Menu;
using NPG.Codebase.Game.Gameplay.UI.HUD;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment;
using NPG.Codebase.Infrastructure.BindingRegistration;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Root
{
    [RequireComponent(typeof(UIRootBinder))]
    public class UIController : MonoBehaviour
    {
        private UIRootViewModel _uiRootViewModel;
        
        private InputActions _inputActions;
        
        private InputAction _openEquipmentAction;

        private InputAction _openMenuWindowAction;
        
        private EquipmentWindowViewModel _equipmentWindowViewModel;
        private MenuWindowViewModel _menuWindowViewModel;

        [Inject]
        public void Construct(InputActions inputActions, UIRootViewModel uiRootViewModel)
        {
            _inputActions = inputActions;
            _uiRootViewModel = uiRootViewModel;
        }
        

        private void OnEnable()
        {
            switch(SceneManager.GetActiveScene().name)
            {
                case "Hub":

					//Equipment window
					_openEquipmentAction = _inputActions.UI.OpenEquipment;
					_openEquipmentAction.Enable();
					_openEquipmentAction.performed += OnOpenEquipment;

					_equipmentWindowViewModel = new EquipmentWindowViewModel();

					_uiRootViewModel.OpenScreen(new HUDViewModel(new WeaponSelectorViewModel(_equipmentWindowViewModel)));
					_uiRootViewModel.OpenWindow(_equipmentWindowViewModel, _equipmentWindowViewModel.RequestHide);

                    //Menu window
                    _openMenuWindowAction = _inputActions.UI.OpenMenuWindow;
					_openMenuWindowAction.Enable();
					_openMenuWindowAction.performed += OnOpenMenu;

                    _menuWindowViewModel = new MenuWindowViewModel();
                    _uiRootViewModel.OpenWindow(_menuWindowViewModel, _menuWindowViewModel.RequestHide);

					break;
				case "MainMenu":
                    return;
			}
			
        }


		private void OnDisable()
        {
            switch(SceneManager.GetActiveScene().name)
            {
                case "Hub":
					_openEquipmentAction.performed -= OnOpenEquipment;
					_openEquipmentAction.Disable();
					_equipmentWindowViewModel.RequestClose();
					break;
				case "MainMenu":
					return;
			}
        }

		private void OnOpenMenu(InputAction.CallbackContext context)
		{
			if(!_menuWindowViewModel.IsVisible)
            {
                _menuWindowViewModel.RequestShow();
                _inputActions.Player.Disable();
            }
            else
            {
                _menuWindowViewModel.RequestHide();
				_inputActions.Player.Enable();
			}
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
                _equipmentWindowViewModel.Dispose();
                _inputActions.Player.Enable();
            }
        }
    }
}