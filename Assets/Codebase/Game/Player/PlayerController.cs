using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Codebase.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject topPart;
        [SerializeField] private GameObject bottomPart;
        
        private InputActions _inputActions;
        
        private InputAction _moveAction;
        private InputAction _fireAction;
        private InputAction _aimAction;
        private InputAction _reloadAction;

        private Camera _camera;
        private Rigidbody _rb;
        
        private Vector2 _moveDirection;
        private Vector2 _mousePosition;
        public event Action<InputAction.CallbackContext> OnFire;
        public event Action<InputAction.CallbackContext> OnReload;

        public Vector3 ToMousePosition(Vector3 origin)
        {
            Ray ray = _camera.ScreenPointToRay(_mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 worldCursorPos = ray.GetPoint(hit.distance);
                
                Vector3 direction = worldCursorPos - origin;
                
                return direction;
            }

            return Vector3.up;
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
            _fireAction.canceled += Fire;

            _reloadAction = _inputActions.Player.Reload;
            _reloadAction.Enable();
            _reloadAction.performed += ReloadWeapon;
        }


        private void OnDisable()
        {
            _moveAction.Disable();
            _fireAction.Disable();
            _aimAction.Disable();
            _reloadAction.Disable();
            
            _fireAction.started -= Fire;
            _fireAction.canceled -= Fire;

            _reloadAction.performed -= ReloadWeapon;
        }
        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _camera = Camera.main;
        }

        private void Update()
        {
            _moveDirection = _moveAction.ReadValue<Vector2>();
            _mousePosition = _aimAction.ReadValue<Vector2>();
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
            Ray ray = _camera.ScreenPointToRay(_mousePosition);
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

        private void ReloadWeapon(InputAction.CallbackContext obj) => OnReload?.Invoke(obj);
    }
}