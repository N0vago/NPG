using NPG.Codebase.Game.Gameplay.UI.Factories;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Project
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<UIRootFactory>().FromNew().AsSingle();

            Container.Bind<WindowsFactory>().FromNew().AsSingle();
        }
    }
}