using NPG.Codebase.Game.Gameplay.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace Assets.NPG.Codebase.Game.Gameplay.UI.Windows.InGame.Menu
{
	public class MenuWindowBinder : WindowBinder
	{
		[Header("Resolution")]
		[SerializeField] private Button resolutionLeftButton;
		[SerializeField] private Button resolutionRightButton;
		[SerializeField] private TMP_Text resolutionValueText;

		[Header("Music")]
		[SerializeField] private Button musicVolumeLeftButton;
		[SerializeField] private Button musicVolumeRightButton;
		[SerializeField] private Slider musicVolumeSlider;

		[Header("SFX")]
		[SerializeField] private Button sfxVolumeLeftButton;
		[SerializeField] private Button sfxVolumeRightButton;
		[SerializeField] private Slider sfxVolumeSlider;

		[Header("General")]
		[SerializeField] private Button applyButton;
		[SerializeField] private Button quitToMainMenuButton;

		private MenuWindowViewModel menuViewModel;
	

		protected override void OnBind(WindowViewModel viewModel)
		{
			base.OnBind(viewModel);
			menuViewModel = (MenuWindowViewModel)viewModel;
			_disposables.Add(menuViewModel.ResolutionProperty.Subscribe(resolution =>
			{
				resolutionValueText.text = $"{resolution.width} x {resolution.height}";
			}));

			menuViewModel.Init();

		}

		private void OnEnable()
		{
			resolutionLeftButton.onClick.AddListener(() => { menuViewModel.ChangeResolution(ResolutionChangeDirection.Previous); });
			resolutionRightButton.onClick.AddListener(() => { menuViewModel.ChangeResolution(ResolutionChangeDirection.Next); });

			applyButton.onClick.AddListener(() => { menuViewModel.ApplySettings(); });
		}
		private void OnDisable()
		{
			resolutionLeftButton.onClick.RemoveAllListeners();
			resolutionRightButton.onClick.RemoveAllListeners();

			applyButton.onClick.RemoveAllListeners();
		}
	}
}
