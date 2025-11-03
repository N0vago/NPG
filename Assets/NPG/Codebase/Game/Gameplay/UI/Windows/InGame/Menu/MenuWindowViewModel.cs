using NPG.Codebase.Game.Gameplay.UI.Windows;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Menu
{
	public class MenuWindowViewModel : WindowViewModel
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

		public ReadOnlyReactiveProperty<(int width, int height)> ResolutionProperty => resolutionChange;

		public (int width, int height) CurrentResolution => currentResolution;

		public override string Id => "MenuWindow";

		public MenuWindowViewModel()
		{
			resolutionChange = new ReactiveProperty<(int width, int height)>(currentResolution);
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
			Debug.Log($"Resolution changed to: {resolutionChange.Value.width} x {resolutionChange.Value.height}");
		}

		public void ApplySettings()
		{
			currentResolution = resolutionChange.Value;
			Debug.Log($"Applying resolution: {currentResolution.width} x {currentResolution.height}");
			Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
		}

	}

	public enum ResolutionChangeDirection
	{
		Next,
		Previous
	}
}
