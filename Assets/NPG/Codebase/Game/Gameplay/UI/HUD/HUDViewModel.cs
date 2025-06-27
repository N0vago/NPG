using NPG.Codebase.Game.Gameplay.UI.Root;
using R3;

namespace NPG.Codebase.Game.Gameplay.UI.HUD
{
    public class HUDViewModel : ViewModel
    {
        private readonly ReactiveProperty<WeaponSelectorViewModel> _weaponSelector = new();
        
        public ReadOnlyReactiveProperty<WeaponSelectorViewModel> WeaponSelector => _weaponSelector;
        
        public HUDViewModel(WeaponSelectorViewModel weaponSelectorViewModel)
        {
            _weaponSelector.Value = weaponSelectorViewModel;
        }
    }
}