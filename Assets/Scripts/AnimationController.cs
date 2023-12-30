using UnityEngine;

public enum AnimationState
{
    SwordAttackAnimation1,
    SwordAttackAnimation2
}

public class AnimationController
{
    private readonly Animator _animator;
    
    private readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int _isDashingAnim = Animator.StringToHash("IsDashing");
    private readonly int _isRunningAnim = Animator.StringToHash("IsRunning");
    private readonly int _attackSpeed = Animator.StringToHash("AttackSpeed");
    private readonly int _isDeadAnim = Animator.StringToHash("IsDead");
    private readonly int _stopAttackAnim = Animator.StringToHash("StopAttack");
    private readonly int _hurtAnim = Animator.StringToHash("Hurt");

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

    public void DoHurtAnimation()
    {
        _animator.SetTrigger(_hurtAnim);
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

    public void DoAttackAnimation(AnimationState animation)
    {
        _animator.Play(animation.ToString());
    }
    
    public void StopAttackAnimation()
    {
        _animator.SetTrigger(_stopAttackAnim);
    }
}
