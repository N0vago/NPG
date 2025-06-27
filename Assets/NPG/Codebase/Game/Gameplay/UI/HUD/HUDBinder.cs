using NPG.Codebase.Game.Gameplay.UI.Root;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.HUD
{
    public class HUDBinder : Binder<HUDViewModel>
    {
        [SerializeField] private WeaponSelectorBinder _weaponSelectorBinder;
        
        protected override void OnBind(HUDViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            _weaponSelectorBinder.Bind(viewModel.WeaponSelector.CurrentValue);
        }
    }
}