using NPG.Codebase.Game.Gameplay.UI.Menu;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using Zenject;

namespace NPG.Codebase.Infrastructure.Installers.Scene
{
	public class MenuInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<MenuBinder>().FromComponentInHierarchy().AsSingle();
			Container.Bind<MenuObjects>().FromScriptableObjectResource("InGameData/MenuObjects").AsSingle();
		}
	}
}
