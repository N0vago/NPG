using NPG.Codebase.Game.Gameplay.UI.Factories;
using NPG.Codebase.Game.Gameplay.UI.Menu;
using NPG.Codebase.Infrastructure.IDs;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Infrastructure.ScriptableObjects.StaticData;
using NPG.Codebase.Infrastructure.Services;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using R3;
using UnityEngine;
using Zenject;
using PrefabProvider = NPG.Codebase.Infrastructure.Services.PrefabProviding.PrefabProvider;

namespace NPG.Codebase.Infrastructure.GameBase.StateMachine.GameStates
{
	public class MenuState : IPayloadState<UserProfileData>, IDataReader
	{
		DiContainer _container;
		UIRootFactory _uiRootFactory;
		ProgressDataHandler _progressDataHandler;

		MenuObjects _menuObjects;
		UserProfileData _userData;
		
		SceneLoader _sceneLoader;
		GameStateMachine _stateMachine;
		public MenuState(SceneLoader sceneLoader, GameStateMachine stateMachine)
		{
			_sceneLoader = sceneLoader;
			_stateMachine = stateMachine;
		}
		public void Enter(UserProfileData payload)
		{
			_container = UnityEngine.Object.FindObjectOfType<SceneContext>().Container;
			_menuObjects = _container.Resolve<MenuObjects>();
			_progressDataHandler = _container.Resolve<ProgressDataHandler>();
			_progressDataHandler.RegisterObserver(this);
			InitMenu();
		}

		private void InitMenu()
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
						MenuViewModel mvm = new MenuViewModel(_userData);
						mvm.StartGame += async () =>
						{
							await _sceneLoader.LoadSceneAsync((int)SceneIDs.Hub, OnSceneLoaded);
						};
						_uiRootFactory.UIRootViewModel.OpenScreen(mvm);
						_uiRootFactory.UIRootBinder.AttachScreenBinder(binder);
						break;
				}
			}
			_uiRootFactory.UIRootBinder.uiController.EnableActions("MainMenu");
		}

		private void OnSceneLoaded()
		{
			_stateMachine.Enter<HubState>();
		}


		public void Exit()
		{
			_uiRootFactory.UIRootBinder.uiController.DisableActions("MainMenu");
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
