using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace NPG.Codebase.Game.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform topPart;
        [SerializeField] private Transform rootTransform;
        
        private Quaternion _topPartBaseLocalRot;
        
        private InputActions _inputActions;
        
        private InputAction _moveAction;
        private InputAction _fireAction;
        private InputAction _aimAction;
        private InputAction _reloadAction;

        private Camera _camera;
        private Rigidbody _rb;
        private Vector3 _prevRbPos;
        
        private Vector2 _mousePosition;
        private Vector2 _moveDirection;
        
        public Vector3 MousePosition => GetMouseWorldPosition();
        
        public float Velocity { get; private set; }
        public bool IsFire { get; private set; }

        public event Action StartFireAction;
        public event Action StopFireAction;
        public event Action ReloadAction;

        [Inject]
        public void Construct(InputActions inputActions)
        {
            _inputActions = inputActions;
        }

        private void Awake()
        {
            _topPartBaseLocalRot = topPart.localRotation;
        }

        private void OnEnable()
        {
            _moveAction = _inputActions.Player.Move;
            _moveAction.Enable();

            _aimAction = _inputActions.Player.Aim;
            _aimAction.Enable();

            _fireAction = _inputActions.Player.Attack;
            _fireAction.Enable();
            _fireAction.started += OnStartFire;
            _fireAction.canceled += OnStopFire;

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
            
            _fireAction.started -= OnStartFire;
            _fireAction.canceled -= OnStopFire;

            _reloadAction.performed -= ReloadWeapon;
        }
        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _camera = Camera.main;
            _prevRbPos = _rb.position;
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
            }
            Velocity = ((_rb.position - _prevRbPos) / Time.fixedDeltaTime).magnitude;
            _prevRbPos = _rb.position;
        }

        private void LateUpdate()
        {
            if (_moveDirection != Vector2.zero)
            { 
                BottomPartLook();
            }
            TopPartLook();
        }

        private void Move() =>
            _rb.MovePosition(rootTransform.position +
                             rootTransform.forward * ((int)_moveDirection.magnitude * moveSpeed * Time.deltaTime));

        private void BottomPartLook()
        {
            var direction = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            var skewedInput = matrix.MultiplyPoint3x4(direction);

            var relative = (rootTransform.position + skewedInput) - rootTransform.position;
            
            Quaternion lookRotation = Quaternion.LookRotation(relative, Vector3.up);

            rootTransform.rotation = lookRotation;
        }

        private void TopPartLook()
        {
            Ray ray = _camera.ScreenPointToRay(_mousePosition);
            Plane groundPlane = new Plane(Vector3.up, topPart.position);

            if (!groundPlane.Raycast(ray, out float enter))
                return;

            Vector3 worldCursorPos = ray.GetPoint(enter);

            Vector3 dir = worldCursorPos - topPart.position;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            Vector3 localDir = rootTransform.InverseTransformDirection(dir);
            
            float angle = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;

            
            topPart.localRotation = _topPartBaseLocalRot * Quaternion.AngleAxis(-angle, Vector3.right);
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = _camera.ScreenPointToRay(_mousePosition);
            Plane groundPlane = new Plane(Vector3.up, topPart.position);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldCursorPos = ray.GetPoint(enter);
                return worldCursorPos;
            }

            return Vector3.zero;
        }

        private void OnStartFire(InputAction.CallbackContext obj)
        {
            IsFire = true;
            StartFireAction?.Invoke();
        }

        private void OnStopFire(InputAction.CallbackContext obj)
        {
            IsFire = false;
            StopFireAction?.Invoke();
        }

        private void ReloadWeapon(InputAction.CallbackContext obj) => ReloadAction?.Invoke();
    }
}