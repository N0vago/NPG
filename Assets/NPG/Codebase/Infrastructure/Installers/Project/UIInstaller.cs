using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Project
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIRootViewModel>().FromNew().AsSingle();
            
            Container.Bind<EquipmentWindowBinder>().FromComponentInHierarchy().AsSingle();

            Container.Bind<ItemContainerBinder>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<SlotContainerBinder>().FromComponentInHierarchy().AsSingle();
            
        }
        
    }
}