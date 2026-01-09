using System.Collections.Generic;
using System.Linq;
using NPG.Codebase.Game.Gameplay.UI.Root;
using NPG.Codebase.Game.Gameplay.UI.Windows;
using ObservableCollections;
using UnityEngine;
using UnityEngine.UI;
using R3;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
    public class ChangeProfileWindowBinder : WindowBinder
    {
        [SerializeField] private Button createProfileButton;
        [SerializeField] private Button selectProfileButton;

        [SerializeField] private Button exitButton;

        [SerializeField] private Transform profilesField;

        [SerializeField] private ProfileButton prefab;

        private UIRootViewModel _uiRootViewModel;
        
        private ChangeProfileWindowViewModel _viewModel;

        private List<ProfileButton> _profileButtons = new();

        [Inject]
        public void Construct(UIRootViewModel uiRootViewModel)
        {
            _uiRootViewModel = uiRootViewModel;
        }

        protected override void OnBind(WindowViewModel viewModel)
        {
            base.OnBind(viewModel);
            _viewModel = viewModel as ChangeProfileWindowViewModel;
            
            foreach (var user in _viewModel.Users)
            {
                ProfileButton newButton = Instantiate(prefab, profilesField);
                newButton.UserData = user;
                newButton.SetProfileName(user.userName);
                _profileButtons.Add(newButton);
                newButton.button.onClick.AddListener(() => SelectButton(newButton));
            }
            _disposables.Add(_viewModel.Users.ObserveAdd().Subscribe(newUser =>
            {
                ProfileButton newButton = Instantiate(prefab, profilesField);
                newButton.UserData = newUser.Value;
                newButton.SetProfileName(newUser.Value.userName);
                _profileButtons.Add(newButton);
                newButton.button.onClick.AddListener(() => SelectButton(newButton));
            }));
            selectProfileButton.onClick.AddListener((() =>
            {
                foreach (var profileButton in _profileButtons)
                {
                    if (profileButton.isSelected)
                    {
                        _viewModel.SetUser(profileButton.UserData);
                    }
                }
                _uiRootViewModel.CloseWindow(_viewModel);
            }));
            
            createProfileButton.onClick.AddListener(OpenCreateProfileWindow);

            exitButton.onClick.AddListener((() => _uiRootViewModel.CloseWindow(_viewModel)));
        }


        private void OpenCreateProfileWindow()
        {
            CreateProfileWindowViewModel viewModel = new CreateProfileWindowViewModel(_viewModel.Users.ToArray());
            _disposables.Add(viewModel.NewProfile.Subscribe(newProfile =>
            {
                _viewModel.AddUser(newProfile);
            }));
            
            _uiRootViewModel.OpenWindow(viewModel);
        }


        protected override void OnEnabling()
        {
            foreach (var profileButton in _profileButtons)
            {
                var capturedButton = profileButton;
                profileButton.button.onClick.AddListener(() =>
                {
                    SelectButton(capturedButton);
                });
            }
        }

        protected override void OnDisabling()
        {
            foreach (var profileButton in _profileButtons)
            {
                profileButton.button.onClick.RemoveAllListeners();
            }
        }
        private void SelectButton(ProfileButton selected)
        {
            foreach (var profileButton in _profileButtons)
            {
                bool isActive = profileButton == selected;
                profileButton.SetSelected(isActive);
            }
        }
    }
}