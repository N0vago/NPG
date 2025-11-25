using NPG.Codebase.Infrastructure.BindingRegistration;
using NPG.Codebase.Infrastructure.GameBase.StateMachine;
using NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.Services;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Project
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStateMachine>().FromNew().AsSingle();

            Container.Bind<HubState>().FromNew().AsSingle();

            Container.Bind<MenuState>().FromNew().AsSingle();

			Container.Bind<SceneLoader>().FromNew().AsSingle();

            Container.Bind<ProgressDataHandler>().FromNew().AsSingle();

            Container.Bind<InputActions>().FromNew().AsSingle();

            Container.Bind<IBinderRegistry>().To<BinderRegistry>().FromNew().AsSingle();
            
            Container.Bind<ItemDataBase>().FromScriptableObjectResource("InGameData/ItemDataBase").AsSingle();
        }
    }
}