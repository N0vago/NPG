using NPG.Codebase.Game.Gameplay.Player;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item;
using R3;
using UnityEngine.InputSystem;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.HUD
{
    public class WeaponSelectorBinder : Binder<WeaponSelectorViewModel>
    {
        private InputActions _inputActions;
        
        private InputAction _selectWeaponAction;
        
        private PlayerCharacter _playerCharacter;
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        [Inject]
        public void Construct(InputActions inputActions)
        {
            _inputActions = inputActions;
            _playerCharacter = FindObjectOfType<SceneContext>().Container.Resolve<PlayerCharacter>();
        }
        
        protected override void OnBind(WeaponSelectorViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            _disposable.Add(ViewModel.SelectedWeapon.Subscribe(weapon =>
            {
                if (weapon == null)
                {
                    _playerCharacter.SetWeapon(null);
                }
                else
                {
                    _playerCharacter.SetWeapon(weapon);
                }
            }));
        }

        private void OnEnable()
        {
            _selectWeaponAction = _inputActions.UI.SelectWeapot;
            _selectWeaponAction.Enable();
            _selectWeaponAction.performed += OnSelectWeapon;
        }

        private void OnSelectWeapon(InputAction.CallbackContext context)
        {
            string controlPath = context.control.path;
            if (controlPath.Contains("1"))
            {
                ViewModel.SelectWeapon(ItemType.Pistol);
            }
            else if (controlPath.Contains("2"))
            {
                ViewModel.SelectWeapon(ItemType.Shotgun);
            }
            else if (controlPath.Contains("3"))
            {
                ViewModel.SelectWeapon(ItemType.Rifle);
            }
            else if (controlPath.Contains("4"))
            {
                ViewModel.SelectWeapon(ItemType.GrenadeLauncher);
            }
            else if (controlPath.Contains("5"))
            {
                ViewModel.SelectWeapon(ItemType.EnergyWeapon);
            }
            
        }
    }
}