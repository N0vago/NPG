using Codebase.Game;
using Codebase.Game.Player;
using Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using Codebase.Infrastructure.Services;
using Zenject;

namespace Codebase.Infrastructure.Installers
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