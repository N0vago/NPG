using Assets.NPG.Codebase.Game.Gameplay.UI.Menu;
using Assets.NPG.Codebase.Infrastructure.JsonData;
using Assets.NPG.Codebase.Infrastructure.ScriptableObjects;
using Assets.NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using NPG.Codebase.Game.Gameplay.Player;
using NPG.Codebase.Game.Gameplay.UI.Factories;
using NPG.Codebase.Infrastructure.GameBase.StateMachine;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace Assets.NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates
{
	public class MenuState : IPayloadState<UserProfileData>, IDataReader
	{
		DiContainer _container;
		UIRootFactory _uiRootFactory;
		ProgressDataHandler _progressDataHandler;

		MenuObjects _menuObjects;
		UserProfileData _userData;
		public void Enter(UserProfileData payload)
		{
			_container = UnityEngine.Object.FindObjectOfType<SceneContext>().Container;
			_menuObjects = _container.Resolve<MenuObjects>();
			_progressDataHandler = _container.Resolve<ProgressDataHandler>();
			_progressDataHandler.RegisterObserver(this);
			InitHub();
		}

		private void InitHub()
		{
			foreach (var menuObjects in _menuObjects.Objects)
			{
				GameObject prefab;
				GameObject instance;

				switch (menuObjects.menuID)
				{
					case MenuIDs.UIRoot:
						_uiRootFactory = _container.Resolve<UIRootFactory>();
						_uiRootFactory.CreateUIRoot(menuObjects.addressableName);
						break;
					case MenuIDs.MenuCanvas:
						prefab = PrefabProvider.LoadPrefab(menuObjects.addressableName);
						instance = _container.InstantiatePrefab(prefab, _uiRootFactory.UIRootBinder.transform);
						MenuBinder binder = instance.GetComponent<MenuBinder>();
						_uiRootFactory.UIRootViewModel.OpenScreen(new MenuViewModel(_userData));
						_uiRootFactory.UIRootBinder.AttachScreenBinder(binder);
						break;
				}
			}
		}
    

		public void Exit()
		{
			
		}

		public void Load(GameData data)
		{
			if (data.userData[data.GetCurrentUserIndex()] != null)
			{
				_userData = data.userData[data.GetCurrentUserIndex()];
			}

		}
	}
}
