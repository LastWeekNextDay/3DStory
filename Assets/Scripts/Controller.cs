using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected CharacterManager CharacterManager;
    
    protected const float TurnSpeed = 1080f;

    protected void DoAttack()
    {
        if (CharacterManager.CharacterInfo.IsAttacking) return;
        if (CharacterManager.CharacterInfo.CurrentAttackCooldown > 0) return;
        if (CharacterManager.CharacterInfo.EquippedWeapon == null) return;
        switch (CharacterManager.CharacterInfo.EquippedWeapon.AttackType)
        {
            case AttackType.Melee:
                PerformMeleeAttack();
                break;
            case AttackType.Ranged:
                PerformRangedAttack();
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
        StartCoroutine(nameof(Dashing));
    }

    private void PerformMeleeAttack()
    {
        if (CharacterManager.CharacterInfo.IsAttacking) return;
        StartCoroutine(nameof(MeleeAttack));
    }
    
    private void PerformRangedAttack()
    {
        if (CharacterManager.CharacterInfo.IsAttacking) return;
        Debug.Log("Ranged Attack");
    }

    private IEnumerator MeleeAttack()
    {
        CharacterManager.CharacterInfo.IsAttacking = true;
        CharacterManager.CharacterInfo.CurrentAttackCooldown = CharacterManager.CharacterInfo.EquippedWeapon.AttackCooldown;
        CharacterManager.CharacterInfo.EquippedWeapon.AllowDamageCollision();
        CharacterManager.AnimationController.SetAttackSpeed(1/CharacterManager.CharacterInfo.EquippedWeapon.AttackSpeed);
        int animIndex;
        if (CharacterManager.CharacterInfo.EquippedWeapon.useCombos)
        {
            animIndex = CharacterManager.CharacterInfo.EquippedWeapon.ComboNumber-1;
            CharacterManager.CharacterInfo.EquippedWeapon.IterateCombo();
        }
        else
        {
            animIndex = 0;
        }
        var anim = CharacterManager.CharacterInfo.EquippedWeapon.AttackAnimation[animIndex];
        CharacterManager.AnimationController.DoAttackAnimation(anim);
        yield return new WaitForSeconds(CharacterManager.CharacterInfo.EquippedWeapon.AttackSpeed);
        CharacterManager.CharacterInfo.EquippedWeapon.DisallowDamageCollision();
        CharacterManager.CharacterInfo.IsAttacking = false;
    }
    
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    private IEnumerator Dashing()
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
