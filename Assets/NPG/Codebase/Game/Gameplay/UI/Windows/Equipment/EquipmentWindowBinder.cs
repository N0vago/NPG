using System;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using UnityEngine;
using R3;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.Equipment
{
    public class EquipmentWindowBinder : WindowBinder
    {
        [SerializeField] SlotContainerBinder slotContainerBinder;
        [SerializeField] ItemContainerBinder itemContainerBinder;
        
        private ItemFactory _itemFactory;
        
        private EquipmentWindowViewModel _equipmentWindowViewModel;
        
        [Inject]
        public void Construct(ItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }
        protected override void OnBind(WindowViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            if (viewModel is not EquipmentWindowViewModel equipmentWindowViewModel)
            {
                Debug.LogError("EquipmentWindowBinder: ViewModel is not of type EquipmentWindowViewModel");
                return;
            }
            _equipmentWindowViewModel = equipmentWindowViewModel;
            
           _equipmentWindowViewModel.ItemContainerViewModel
                .Subscribe(itemContainerViewModel => itemContainerBinder.Bind(itemContainerViewModel));
            
           
           _equipmentWindowViewModel.SlotContainer.Subscribe(
                slotContainerViewModel => slotContainerBinder.Bind(slotContainerViewModel));
           
           _equipmentWindowViewModel.ItemContainerViewModel.CurrentValue.AddItem(new ItemViewModel());
        }

       
    }
}