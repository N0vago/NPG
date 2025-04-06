using System;
using Codebase.Game.Weapon;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Codebase.Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private Pistol pistol;
        
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerController.OnFire += Fire;
        }

        private void OnDisable()
        {
            _playerController.OnFire -= Fire;
        }

        private void Fire(InputAction.CallbackContext obj)
        {
            pistol.Shoot();
        }
    }
}