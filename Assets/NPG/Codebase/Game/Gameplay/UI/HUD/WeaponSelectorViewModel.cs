using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment;
using NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.Weapon;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.HUD
{
    public class WeaponSelectorViewModel : ViewModel
    {
        private readonly Subject<WeaponBase> _selectedWeapon = new();
        
        private EquipmentWindowViewModel _equipmentWindowViewModel;
        
        public Observable<WeaponBase> SelectedWeapon => _selectedWeapon;
        
        public WeaponSelectorViewModel(EquipmentWindowViewModel equipmentWindowViewModel)
        {
            _equipmentWindowViewModel = equipmentWindowViewModel;
        }
        public void SelectWeapon(ItemType itemType)
        {
            if (_equipmentWindowViewModel == null)
            {
                Debug.LogError("No EquipmentWindowViewModel");
            }
            ItemContainerViewModel containerViewModel = _equipmentWindowViewModel.GetItemContainer(itemType).CurrentValue;
            
            if (containerViewModel == null) return;
            if (containerViewModel.Items.Count == 0) return;
            
            ItemViewModel item = containerViewModel.Items.FirstOrDefault(item => item.ItemType == itemType);
            
            _selectedWeapon.OnNext(item?.WeaponPrefab);
        }
    }
}