using Codebase.Infrastructure.GameBase.StateMachine;
using Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using Codebase.Infrastructure.Services;
using Codebase.Infrastructure.Services.DataSaving;
using Zenject;

namespace Codebase.Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStateMachine>().FromNew().AsSingle();

            Container.Bind<HubState>().FromNew().AsSingle();
            
            Container.Bind<SceneLoader>().FromNew().AsSingle();

            Container.Bind<ProgressDataHandler>().FromNew().AsSingle();
        }
    }
}