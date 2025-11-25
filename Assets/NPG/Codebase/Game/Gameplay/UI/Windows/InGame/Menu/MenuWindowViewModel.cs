using System.Collections.Generic;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.Services.DataSaving;
using R3;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Menu
{
	public class MenuWindowViewModel : WindowViewModel, IDataWriter
	{

		private readonly ReactiveProperty<(int width, int height)> resolutionChange;

		private (int width, int height) currentResolution = (1920, 1080);

		private List<(int width, int height)> availableResolutions = new List<(int width, int height)>
		{
			(800, 600),
			(1024, 768),
			(1280, 720),
			(1920, 1080),
		};

		private ProgressDataHandler _progressDataHandler;

		public ReadOnlyReactiveProperty<(int width, int height)> ResolutionProperty => resolutionChange;

		public (int width, int height) CurrentResolution => currentResolution;

		public override string Id => "MenuWindow";

		public MenuWindowViewModel(ProgressDataHandler progressDataHandler)
		{
			_progressDataHandler = progressDataHandler;
			resolutionChange = new ReactiveProperty<(int width, int height)>((1920, 1080));
		}

		public void Init()
		{
			_progressDataHandler.RegisterObserver(this);
		}

		public void ChangeResolution(ResolutionChangeDirection direction)
		{
			var currentResolution = resolutionChange.Value;
			int currentIndex = availableResolutions.IndexOf(currentResolution);
			if (direction == ResolutionChangeDirection.Next)
			{
				int nextIndex = (currentIndex + 1) % availableResolutions.Count;
				resolutionChange.Value = availableResolutions[nextIndex];
			}
			else if (direction == ResolutionChangeDirection.Previous)
			{
				int previousIndex = (currentIndex - 1 + availableResolutions.Count) % availableResolutions.Count;
				resolutionChange.Value = availableResolutions[previousIndex];
			}
		}

		public void ApplySettings()
		{
			currentResolution = resolutionChange.Value;
			Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
		}

		public void Save(ref GameData data)
		{
			data.gamePreferences.resolutionWidth = currentResolution.width;
			data.gamePreferences.resolutionHeight = currentResolution.height;
		}

		public void Load(GameData data)
		{
			currentResolution = (data.gamePreferences.resolutionWidth, data.gamePreferences.resolutionHeight);
			resolutionChange.Value = currentResolution;
		}

		public override void Dispose()
		{
			_progressDataHandler.SaveProgress(this);
			base.Dispose();
		}
	}

	public enum ResolutionChangeDirection
	{
		Next,
		Previous
	}
}
