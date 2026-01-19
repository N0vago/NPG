using System;
using UnityEngine;

namespace NPG.Codebase.Game.Gameplay.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int IsFire = Animator.StringToHash("IsFire");
        private Animator _animator;
        private PlayerController _playerController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
           _animator.SetFloat(Velocity, _playerController.Velocity);
           _animator.SetBool(IsFire, _playerController.IsFire);
        }
    }
}