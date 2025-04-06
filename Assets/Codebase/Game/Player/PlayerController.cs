using System;
using Codebase.Infrastructure.Data;
using Codebase.Infrastructure.Services.DataSaving;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Codebase.Game.Player
{
    public class PlayerController : MonoBehaviour, IDataWriter
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject topPart;
        [SerializeField] private GameObject bottomPart;

        private ProgressDataHandler _progressDataHandler;
        
        private InputActions _inputActions;
        
        private InputAction _moveAction;
        private InputAction _fireAction;
        private InputAction _aimAction;

        private Camera _camera;
        private Rigidbody _rb;
        private Vector2 _moveDirection;

        public event Action<InputAction.CallbackContext> OnFire;

        [Inject]
        public void Construct(ProgressDataHandler progressDataHandler)
        {
            _progressDataHandler = progressDataHandler;
            _progressDataHandler.RegisterObserver(this);
        }

        public void Load(GameData data)
        {
            
        }

        public void Save(ref GameData data)
        {
            data.playerData.playerPosition = gameObject.transform.position;
        }

        private void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            _moveAction = _inputActions.Player.Move;
            _moveAction.Enable();

            _aimAction = _inputActions.Player.Aim;
            _aimAction.Enable();

            _fireAction = _inputActions.Player.Attack;
            _fireAction.Enable();
            _fireAction.started += Fire;
        }


        private void OnDisable()
        {
            _moveAction.Disable();
            _fireAction.Disable();
            _aimAction.Disable();
            _fireAction.started -= Fire;
            
            _progressDataHandler.SaveProgress(this);
        }

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _camera = Camera.main;
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
                BottomPartLook();
            }
            
            TopPartLook();
        }

        private void Move() => _rb.MovePosition(bottomPart.transform.position + bottomPart.transform.forward * (_moveDirection.magnitude * moveSpeed * Time.deltaTime));

        private void BottomPartLook()
        {
            var direction = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            var skewedInput = matrix.MultiplyPoint3x4(direction);

            var relative = (bottomPart.transform.position + skewedInput) - bottomPart.transform.position;
            
            Quaternion lookRotation = Quaternion.LookRotation(relative, Vector3.up);

            bottomPart.transform.rotation = lookRotation;
        }

        private void TopPartLook()
        {
            Ray ray = _camera.ScreenPointToRay(_aimAction.ReadValue<Vector2>());
            Plane groundPlane = new Plane(Vector3.up, topPart.transform.position);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldCursorPos = ray.GetPoint(enter);
                
                Vector3 direction = worldCursorPos - topPart.transform.position;
                direction.y = 0f;

                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    topPart.transform.rotation = rotation;
                }
            }
        }

        private void Fire(InputAction.CallbackContext obj) => OnFire?.Invoke(obj);
    }
}