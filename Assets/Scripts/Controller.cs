using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected CharacterManager CharacterManager;
    
    protected const float TurnSpeed = 1080f;

    protected void DoAttack(Vector2 attackDirection)
    {
        if (CharacterManager.CharacterInfo.IsAttacking) return;
        if (CharacterManager.CharacterInfo.CurrentAttackCooldown > 0) return;
        if (CharacterManager.CharacterInfo.EquippedWeapon == null) return;
        switch (CharacterManager.CharacterInfo.EquippedWeapon.AttackType)
        {
            case AttackType.Melee:
                PerformMeleeAttack(attackDirection);
                break;
            case AttackType.Ranged:
                PerformRangedAttack(attackDirection);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected void DoDash(Vector3 movement)
    {
        if (movement == Vector3.zero) return;
        if (CharacterManager.CharacterInfo.CurrentDashCooldown > 0) return;
        if (CharacterManager.CharacterInfo.IsDashing) return;
        StartCoroutine(nameof(Dashing), movement);
    }

    private void PerformMeleeAttack(Vector2 attackDirection)
    {
        if (CharacterManager.CharacterInfo.IsAttacking) return;
        StartCoroutine(nameof(MeleeAttack), attackDirection);
    }
    
    private void PerformRangedAttack(Vector2 attackDirection)
    {
        Debug.Log(attackDirection);
        if (CharacterManager.CharacterInfo.IsAttacking) return;
        Debug.Log("Ranged Attack");
    }

    private IEnumerator MeleeAttack(Vector2 attackDirection)
    {
        CharacterManager.CharacterInfo.IsAttacking = true;
        CharacterManager.CharacterInfo.CurrentAttackCooldown = CharacterManager.CharacterInfo.EquippedWeapon.AttackCooldown;
        CharacterManager.CharacterInfo.EquippedWeapon.AllowDamageCollision();
        CharacterManager.AnimationController.SetAttackSpeed(1/CharacterManager.CharacterInfo.EquippedWeapon.AttackSpeed);
        CharacterManager.AnimationController.SetRotationDirection(attackDirection);
        CharacterManager.AnimationController.DoAttackAnimation(CharacterManager.CharacterInfo.EquippedWeapon.AttackAnimation);
        yield return new WaitForSeconds(CharacterManager.CharacterInfo.EquippedWeapon.AttackSpeed);
        CharacterManager.CharacterInfo.EquippedWeapon.DisallowDamageCollision();
        CharacterManager.CharacterInfo.IsAttacking = false;
    }
    
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    private IEnumerator Dashing(Vector3 movement)
    {
        CharacterManager.CharacterInfo.IsDashing = true;
        var beforeSpeed = CharacterManager.CharacterInfo.CurrentSpeed;
        CharacterManager.CharacterInfo.CurrentSpeed = CharacterManager.CharacterInfo.DashSpeed;
        CharacterManager.CharacterInfo.CurrentDashCooldown = CharacterManager.CharacterInfo.DashCooldown;
        CharacterManager.AnimationController.DoDashAnimation();
        yield return new WaitForSeconds(CharacterManager.CharacterInfo.DashDuration);
        CharacterManager.CharacterInfo.CurrentSpeed = beforeSpeed;
        CharacterManager.AnimationController.StopDashAnimation();
        CharacterManager.CharacterInfo.IsDashing = false;
    }
}
