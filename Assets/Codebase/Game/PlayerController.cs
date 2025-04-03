using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace Codebase.Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        private InputActions _inputActions;
        private InputAction _moveAction;
        
        private Rigidbody _rb;
        private Vector2 _moveDirection;

        private void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            _moveAction = _inputActions.Player.Move;
            _moveAction.Enable();
        }

        private void OnDisable()
        {
            _moveAction.Disable();
        }

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _moveDirection = _moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (_moveDirection != Vector2.zero)
            {
                Move();
                Look();
            }
        }

        private void Move() => _rb.MovePosition(transform.position + transform.forward * (_moveDirection.magnitude * moveSpeed * Time.deltaTime));

        private void Look()
        {
            var direction = new Vector3(_moveDirection.x, 0, _moveDirection.y);
            
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            var skewedInput = matrix.MultiplyPoint3x4(direction);

            var relative = (transform.position + skewedInput) - transform.position;
            
            Quaternion lookRotation = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = lookRotation;
        }
        
    }
}