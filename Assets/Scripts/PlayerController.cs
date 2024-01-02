using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    private Controls _controls;
    
    private Vector3 _movement = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Movement.performed += OnMovementPerformed;
        _controls.Player.Movement.canceled += OnMovementCanceled;
        _controls.Player.Attack.performed += OnAttackPerformed;
        _controls.Player.Dash.performed += OnDashPerformed;
    }
    
    private void OnDisable()
    {
        _controls.Player.Dash.performed -= OnDashPerformed;
        _controls.Player.Attack.performed -= OnAttackPerformed;
        _controls.Player.Movement.canceled -= OnMovementCanceled;
        _controls.Player.Movement.performed -= OnMovementPerformed;
        _controls.Disable();
    }
    
    protected override void MoveUpdate()
    {
        if (_movement == Vector3.zero)
        {
            CharacterManager.CharacterInfo.RealSpeed = 0;
            CharacterManager.CharacterInfo.IsRunning = false;
            CharacterManager.AnimationController.StopRunAnimation();
        }
        else
        {
            CharacterManager.CharacterInfo.IsRunning = true;
            CharacterManager.CharacterInfo.RealSpeed = _movement.normalized.magnitude *
                CharacterManager.CharacterInfo.logicalSpeed;
            CharacterManager.AnimationController.DoRunAnimation();
        }
        TargetToMoveTo.transform.position = gameObject.transform.position +
                                                _movement.ToIsometric() *
                                                (CharacterManager.CharacterInfo.RealSpeed *
                                                 Time.deltaTime);
        CharacterManager.RigidBody.MovePosition(TargetToMoveTo.transform.position);
        CharacterManager.AnimationController.SetMoveSpeed(
            CharacterManager.CharacterInfo.RealSpeed / CharacterManager.CharacterInfo.logicalSpeed);
    }
    
    protected override void LookUpdate()
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
        DoAttack();
    }
    
    private void OnDashPerformed(InputAction.CallbackContext value)
    {
        DoDash(_movement);
    }

    public override void StopMovement()
    {
        _movement = Vector3.zero;
    }

    public override void DoAttack()
    {
        DoGenericAttack();
    }
}
