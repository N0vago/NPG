using NPG.Codebase.Game.Gameplay.UI.HUD;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows.Equipment;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Project
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EquipmentWindowViewModel>().FromNew().AsSingle();
            
            Container.Bind<UIRootBinder>().FromComponentSibling().AsSingle();
            
            Container.Bind<UIRootViewModel>().FromNew().AsSingle();
            
            Container.Bind<HUDBinder>().FromComponentSibling().AsSingle();

            Container.Bind<WeaponSelectorBinder>().FromComponentSibling().AsSingle();
            
        }
        
    }
}