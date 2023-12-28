using UnityEngine;

public class AnimationController
{
    private readonly Animator _animator;
    
    private readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int _isDashingAnim = Animator.StringToHash("IsDashing");
    private readonly int _isRunningAnim = Animator.StringToHash("IsRunning");
    private readonly int _attackSpeed = Animator.StringToHash("AttackSpeed");
    private readonly int _swordAttackAnim1 = Animator.StringToHash("SwordAttackAnimation1");
    private readonly int _swordAttackAnim2 = Animator.StringToHash("SwordAttackAnimation2");
    private readonly int _isDeadAnim = Animator.StringToHash("IsDead");
    private readonly int _stopAttackAnim = Animator.StringToHash("StopAttack");

    public AnimationController(Animator animator)
    {
        _animator = animator;
    }
    
    public void SetAnimatorActive(bool active)
    {
        _animator.enabled = active;
    }
    
    public void DoDashAnimation()
    {
        _animator.SetBool(_isDashingAnim, true);
    }
    
    public void StopDashAnimation()
    {
        _animator.SetBool(_isDashingAnim, false);
    }
    
    public void DoDeathAnimation()
    {
        _animator.SetBool(_isDeadAnim, true);
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

    public void DoAttackAnimation(string animation)
    {
        switch (animation)
        {
            case "SwordAttackAnimation1":
                _animator.SetTrigger(_swordAttackAnim1);
                break;
            case "SwordAttackAnimation2":
                _animator.SetTrigger(_swordAttackAnim2);
                break;
            default:
                Debug.LogError(
                    _animator.gameObject.name + ": Animation not found. Tried to play \"" + animation + "\".");
                break;
        }
    }
    
    public void StopAttackAnimation()
    {
        _animator.SetTrigger(_stopAttackAnim);
    }
}
