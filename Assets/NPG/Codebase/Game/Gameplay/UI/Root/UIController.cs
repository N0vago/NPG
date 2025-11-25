using System;
using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.HUD;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Menu;
using NPG.Codebase.Infrastructure.BindingRegistration;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.Services.DataSaving;
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
        private ItemDataBase _itemDataBase;
        
		private MenuWindowViewModel _menuWindowViewModel;


        private ProgressDataHandler _progressDataHandler;

		[Inject]
        public void Construct(InputActions inputActions, UIRootViewModel uiRootViewModel, ItemDataBase itemDataBase, ProgressDataHandler progressDataHandler)
        {
            _inputActions = inputActions;
            _uiRootViewModel = uiRootViewModel;
            _itemDataBase = itemDataBase;
            _progressDataHandler = progressDataHandler;
		}
        

        private void OnEnable()
        {
            switch(SceneManager.GetActiveScene().name)
            {
                case "Hub":

					_uiRootViewModel.OpenScreen(new HUDViewModel(new WeaponSelectorViewModel(_equipmentWindowViewModel)));

					//Equipment window
					_openEquipmentAction = _inputActions.UI.OpenEquipment;
					_openEquipmentAction.Enable();
					_openEquipmentAction.performed += OnOpenEquipment;

                    //Menu window
                    _openMenuWindowAction = _inputActions.UI.OpenMenuWindow;
					_openMenuWindowAction.Enable();
					_openMenuWindowAction.performed += OnOpenMenu;

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

                    _openMenuWindowAction.performed -= OnOpenMenu;
                    _openMenuWindowAction.Disable();

					_uiRootViewModel.CloseWindow(_equipmentWindowViewModel);
					break;
				case "MainMenu":
					return;
			}
        }

		private void OnOpenMenu(InputAction.CallbackContext context)
		{
            if (_menuWindowViewModel != null)
            {
                if (_uiRootViewModel.OpenedWindows.Contains(_menuWindowViewModel))
                {
                    _uiRootViewModel.CloseWindow(_menuWindowViewModel);
                    _inputActions.Player.Enable();
                    return;
                }
            }
            
            _menuWindowViewModel = new MenuWindowViewModel(_progressDataHandler);
			_uiRootViewModel.OpenWindow(_menuWindowViewModel);
			_inputActions.Player.Disable();
			
		}
        private void OnOpenEquipment(InputAction.CallbackContext obj)
        {
            if (_equipmentWindowViewModel != null)
            {
                if (_uiRootViewModel.OpenedWindows.Contains(_equipmentWindowViewModel))
                {
                    _uiRootViewModel.CloseWindow(_equipmentWindowViewModel);
                    _inputActions.Player.Enable();
                    return;
				}
            }

            if (_itemDataBase == null)
            {
	            Debug.LogError("No item data base was found.");
            }
            _equipmentWindowViewModel = new EquipmentWindowViewModel(_itemDataBase, _progressDataHandler);
			_uiRootViewModel.OpenWindow(_equipmentWindowViewModel);
			_inputActions.Player.Disable();
            
        }
    }
}