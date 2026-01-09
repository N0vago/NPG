using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using NPG.Codebase.Infrastructure.JsonData;
using NPG.Codebase.Infrastructure.ScriptableObjects;
using NPG.Codebase.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
    public class CreateProfileWindowBinder : WindowBinder
    {
        [SerializeField] private TMP_InputField profileNameInput;
        [SerializeField] private Button[]  profileIcons;
        [SerializeField] private Button createProfileButton;
        [SerializeField] private Button exitButton;

        private UIRootViewModel _uiRootViewModel;
        private Image _selectedIcon;
        private CreateProfileWindowViewModel _viewModel;

        [Inject]
        public void Construct(UIRootViewModel uiRootViewModel)
        {
            _uiRootViewModel = uiRootViewModel;
        }
        
        protected override void OnBind(WindowViewModel viewModel)
        {
            base.OnBind(viewModel);
            
            _viewModel = (CreateProfileWindowViewModel)viewModel;
            
            createProfileButton.onClick.AddListener(TryCreateProfile);
            
            exitButton.onClick.AddListener(() => _uiRootViewModel.CloseWindow(_viewModel));
        }

        private void TryCreateProfile()
        {
            UserProfileData profile = new UserProfileData()
                { 
                    userName = profileNameInput.text,
                    userAvatar = _selectedIcon.gameObject.name,
                    userId = IDGenerator.GenerateProfileID(),
                    hasSession = false,
                    isCurrentUser = false,
                    achievementsIDs = new(),
                    playerData = new PlayerData()
                };
            if (!_viewModel.TryCreateProfile(profile))
            {
                TryCreateProfile();
            }
            _uiRootViewModel.CloseWindow(_viewModel);
        }

        protected override void OnEnabling()
        {
            foreach (var profileIcon in profileIcons)
            {
                var capturedIcon = profileIcon;
                profileIcon.onClick.AddListener(() =>
                {
                    SelectIcon(capturedIcon);
                });
            }
        }

        protected override void OnDisabling()
        {
            foreach (var profileIcon in profileIcons)
            {
                profileIcon.onClick.RemoveAllListeners();
            }
            createProfileButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }

        private void SelectIcon(Button clickedButton)
        {
            var clickedImage = clickedButton.GetComponent<Image>();

            foreach (var icon in profileIcons)
            {
                var image = icon.GetComponent<Image>();
                
                bool isSelected = image == clickedImage;
                image.color = isSelected ? Color.gray : Color.white;
            }

            _selectedIcon = clickedImage;
        }

        
    }
}