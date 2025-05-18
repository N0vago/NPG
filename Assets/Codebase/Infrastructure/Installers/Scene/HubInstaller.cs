using Codebase.Game.Player;
using Codebase.Infrastructure.Services;
using Zenject;

namespace Codebase.Infrastructure.Installers.Scene
{
    public class HubInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<HubInitializer>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        }
    }
}