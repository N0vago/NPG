using NPG.Codebase.Infrastructure.JsonData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NPG.Codebase.Game.Gameplay.UI.Menu
{
    public class ProfileButton : MonoBehaviour
    {
        [HideInInspector] public bool isSelected;
        public TMP_Text profileText;
        public Button button;

        private Color _defaultColor;
        private Color _selectedColor;
        
        public UserProfileData UserData { get; set; }

        void Awake()
        {
            _defaultColor = new Color(
                252f / 255f,
                83f  / 255f,
                41f  / 255f,
                1f
            );

            _selectedColor = new Color(
                179f / 255f,
                46f  / 255f,
                14f  / 255f,
                1f
            );
        }
        public void SetProfileName(string profileName)
        {
            profileText.text = profileName;
        }
        
        public void SetSelected(bool value)
        {
            button.image.color = value ? _selectedColor : _defaultColor;
            isSelected = value;
        }
        
    }
}