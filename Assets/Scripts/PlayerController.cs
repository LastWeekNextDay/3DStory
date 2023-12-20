using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    private Controls _controls;
    
    private Vector3 _movement = Vector3.zero;
    
    private Vector2 _attackDirection = Vector2.zero;

    private void Awake()
    {
        _controls = new Controls();
        CharacterManager = GetComponent<CharacterManager>();
    }

    private void Update()
    {
        LookUpdate();
    }

    private void FixedUpdate()
    {
        MoveUpdate();
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Movement.performed += OnMovementPerformed;
        _controls.Player.Movement.canceled += OnMovementCanceled;
        _controls.Player.Attack.performed += OnAttackPerformed;
        _controls.Player.MousePosition.performed += OnMousePositionPerformed;
        _controls.Player.RightAnalogPosition.performed += OnRightStickPerformed;
        _controls.Player.Dash.performed += OnDashPerformed;
    }
    
    private void OnDisable()
    {
        _controls.Player.Dash.performed -= OnDashPerformed;
        _controls.Player.RightAnalogPosition.performed -= OnRightStickPerformed;
        _controls.Player.MousePosition.performed -= OnMousePositionPerformed;
        _controls.Player.Attack.performed -= OnAttackPerformed;
        _controls.Player.Movement.canceled -= OnMovementCanceled;
        _controls.Player.Movement.performed -= OnMovementPerformed;
        _controls.Disable();
    }
    
    private void MoveUpdate()
    {
        if (_movement == Vector3.zero)
        {
            CharacterManager.AnimationController.StopRunAnimation();
        }
        else
        {
            CharacterManager.AnimationController.DoRunAnimation();
        }
        CharacterManager.RigidBody.MovePosition(gameObject.transform.position +
                                                _movement.ToIsometric() *
                                                (_movement.magnitude * CharacterManager.CharacterInfo.CurrentSpeed *
                                                 Time.deltaTime));
        CharacterManager.AnimationController.SetMoveSpeed(_movement.magnitude *
            CharacterManager.CharacterInfo.CurrentSpeed / CharacterManager.CharacterInfo.DefaultSpeed);
    }
    
    private void LookUpdate()
    {
        if (_movement == Vector3.zero) return;
        
        var rotation = Quaternion.LookRotation(_movement.ToIsometric(), Vector3.up);
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, rotation,
            TurnSpeed * Time.deltaTime);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector3>();
    }
    
    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _movement = Vector3.zero;
    }
    
    private void OnAttackPerformed(InputAction.CallbackContext value)
    {
        DoAttack(_attackDirection);
    }
    
    private void OnDashPerformed(InputAction.CallbackContext value)
    {
        DoDash(_movement);
    }

    private void OnMousePositionPerformed(InputAction.CallbackContext value)
    {
        var mousePosition = value.ReadValue<Vector2>();
        if (Camera.main == null) return;
        var playerScreenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        var direction = mousePosition - (Vector2)playerScreenPosition;
        _attackDirection = direction.normalized;
    }
    
    private void OnRightStickPerformed(InputAction.CallbackContext value)
    {
        var analog = value.ReadValue<Vector2>();
        _attackDirection = analog.normalized;
    }
}
