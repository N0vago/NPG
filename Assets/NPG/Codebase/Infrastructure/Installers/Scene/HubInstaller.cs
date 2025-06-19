using NPG.Codebase.Game.Gameplay.Player;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Item;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components.Slot;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Scene
{
    public class HubInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<UIRootBinder>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<HubObjects>().FromScriptableObjectResource("HubObjects").AsSingle();
            
            Container.Bind<ItemContainerBinder>().FromComponentInHierarchy().AsTransient();

            Container.Bind<SlotContainerBinder>().FromComponentInHierarchy().AsTransient();

            Container.Bind<ItemFactory>().FromNew().AsSingle();
            
            Container.Bind<SlotFactory>().FromNew().AsSingle();
        }
    }
}