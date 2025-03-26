using Codebase.Infrastructure.GameBase.StateMachine;
using Codebase.Infrastructure.Services;
using Zenject;

namespace Codebase.Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStateMachine>().FromNew().AsSingle();

            Container.Bind<SceneLoader>().FromNew().AsSingle();
        }
    }
}