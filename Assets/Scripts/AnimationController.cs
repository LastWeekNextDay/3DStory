using UnityEngine;

public class AnimationController
{
    private readonly Animator _animator;
    
    private readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int _isDashingAnim = Animator.StringToHash("IsDashing");
    private readonly int _isRunningAnim = Animator.StringToHash("IsRunning");
    private readonly int _attackSpeed = Animator.StringToHash("AttackSpeed");
    private readonly int _swordAttackAnim1 = Animator.StringToHash("SwordAttackAnimation1");
    private readonly int _x = Animator.StringToHash("X");
    private readonly int _y = Animator.StringToHash("Y");

    public AnimationController(Animator animator)
    {
        _animator = animator;
    }
    
    public void DoDashAnimation()
    {
        // TODO: Implement dash animation
        _animator.SetBool(_isDashingAnim, true);
    }
    
    public void StopDashAnimation()
    {
        _animator.SetBool(_isDashingAnim, false);
    }
    
    public void DoRunAnimation()
    {
        _animator.SetBool(_isRunningAnim, true);
    }
    
    public void StopRunAnimation()
    {
        _animator.SetBool(_isRunningAnim, false);
    }
    
    public void SetMoveSpeed(float speed)
    {
        _animator.SetFloat(_moveSpeed, speed);
    }

    public void SetAttackSpeed(float speed)
    {
        _animator.SetFloat(_attackSpeed, speed);
    }

    public void SetRotationDirection(Vector2 vector2)
    {
        _animator.SetFloat(_x, vector2.x);
        _animator.SetFloat(_y, vector2.y);
    }

    public void DoAttackAnimation(string animation)
    {
        switch (animation)
        {
            case "SwordAttackAnimation1":
                _animator.SetTrigger(_swordAttackAnim1);
                break;
            default:
                Debug.LogError(
                    _animator.gameObject.name + ": Animation not found. Tried to play \"" + animation + "\".");
                break;
        }
    }
    
    public void StopAttackAnimation()
    {
        Debug.Log("Stop Attack");
    }
}
