using NPG.Codebase.Game.Gameplay.Player;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Scene
{
    public class HubInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<HubObjects>().FromScriptableObjectResource("HubObjects").AsSingle();
        }
    }
}