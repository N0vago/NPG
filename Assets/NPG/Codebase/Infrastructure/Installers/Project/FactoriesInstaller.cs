using NPG.Codebase.Game.Gameplay.UI.Factories;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment.Components;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Project
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {

            Container.Bind<UIRootFactory>().FromNew().AsSingle();

            Container.Bind<WindowsFactory>().FromNew().AsSingle();
            
            Container.Bind<ItemFactory>().FromNew().AsSingle();
            
            Container.Bind<SlotFactory>().FromNew().AsSingle();
        }
    }
}