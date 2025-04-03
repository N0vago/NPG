using System;
using Codebase.Infrastructure.Data;
using Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Zenject;

namespace Codebase.Game
{
    public class PlayerController : MonoBehaviour, IDataWriter
    {
        [SerializeField] private float moveSpeed;

        private ProgressDataHandler _progressDataHandler;
        
        private InputActions _inputActions;
        private InputAction _moveAction;
        
        private Rigidbody _rb;
        private Vector2 _moveDirection;

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            _progressDataHandler = progressDataHandler;
            _progressDataHandler.RegisterObserver(this);
        }

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
            _progressDataHandler.SaveProgress(this);
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

        public void Load(GameData data)
        {
            
        }

        public void Save(ref GameData data)
        {
            data.playerData.playerPosition = gameObject.transform.position;
        }
    }
}