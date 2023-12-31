using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] private CharacterManager characterManager;
    public CharacterManager CharacterManager => characterManager;
    [NonSerialized] public GameObject TargetToMoveTo;
    public GameObject TargetToAttack;
    
    protected const float TurnSpeed = 1080f;

    protected virtual void Awake()
    {
        TargetToMoveTo = new GameObject(gameObject.name + " MoveTarget");
        TargetToMoveTo.transform.position = gameObject.transform.position;
    }

    protected virtual void Update()
    {
        LookUpdate();
    }

    protected virtual void FixedUpdate()
    {
        MoveUpdate();
    }

    public abstract void StopMovement();

    public void StopAttack()
    {
        if (characterManager.CharacterInfo.IsAttacking == false) return;
        characterManager.CharacterInfo.IsAttacking = false;
        characterManager.CharacterInfo.EquippedWeapon.DisallowDamageCollision();
        characterManager.AnimationController.StopAttackAnimation();
        characterManager.CharacterInfo.CurrentAttackCooldown = characterManager.CharacterInfo.EquippedWeapon.AttackCooldown;
        switch (characterManager.CharacterInfo.EquippedWeapon.AttackType)
        {
            case AttackType.Melee:
                StopCoroutine(nameof(MeleeAttack));
                break;
            case AttackType.Ranged:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public abstract void DoAttack();
    
    protected void DoGenericAttack()
    {
        if (characterManager.CharacterInfo.IsAttacking) return;
        if (characterManager.CharacterInfo.CurrentAttackCooldown > 0) return;
        switch (characterManager.CharacterInfo.EquippedWeapon.AttackType)
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

    public void DoDash(Vector3 movement)
    {
        if (movement == Vector3.zero) return;
        if (characterManager.CharacterInfo.CurrentDashCooldown > 0) return;
        if (characterManager.CharacterInfo.IsDashing) return;
        StartCoroutine(nameof(Dashing));
    }

    public void PerformMeleeAttack()
    {
        if (characterManager.CharacterInfo.IsAttacking) return;
        StartCoroutine(nameof(MeleeAttack));
    }
    
    public void PerformRangedAttack()
    {
        if (characterManager.CharacterInfo.IsAttacking) return;
        Debug.Log("Ranged Attack");
    }

    private IEnumerator MeleeAttack()
    {
        characterManager.CharacterInfo.IsAttacking = true;
        characterManager.CharacterInfo.CurrentAttackCooldown = characterManager.CharacterInfo.EquippedWeapon.AttackCooldown;
        characterManager.CharacterInfo.EquippedWeapon.AllowDamageCollision();
        characterManager.CharacterInfo.EquippedWeapon.AudioSource.PlayOneShot(
            characterManager.CharacterInfo.EquippedWeapon.WeaponInitialAttackSound);
        characterManager.AnimationController.SetAttackSpeed(1/characterManager.CharacterInfo.EquippedWeapon.AttackTime);
        int animIndex;
        if (characterManager.CharacterInfo.CanCombo)
        {
            animIndex = characterManager.CharacterInfo.EquippedWeapon.CurrentComboNumber-1;
            characterManager.CharacterInfo.EquippedWeapon.IterateCombo();
        }
        else
        {
            animIndex = 0;
        }
        var anim = characterManager.CharacterInfo.EquippedWeapon.AttackAnimations[animIndex];
        characterManager.AnimationController.DoAttackAnimation(anim);
        yield return new WaitForSeconds(characterManager.CharacterInfo.EquippedWeapon.AttackTime);
        characterManager.CharacterInfo.EquippedWeapon.DisallowDamageCollision();
        characterManager.CharacterInfo.IsAttacking = false;
    }
    
    private IEnumerator Dashing()
    {
        characterManager.CharacterInfo.IsDashing = true;
        var beforeSpeed = characterManager.CharacterInfo.logicalSpeed;
        characterManager.CharacterInfo.logicalSpeed = characterManager.CharacterInfo.dashSpeed;
        characterManager.CharacterInfo.CurrentDashCooldown = characterManager.CharacterInfo.dashCooldown;
        characterManager.AnimationController.DoDashAnimation();
        characterManager.AudioSource.PlayOneShot(characterManager.CharacterSounds.DashSound);
        yield return new WaitForSeconds(characterManager.CharacterInfo.dashDuration);
        characterManager.CharacterInfo.logicalSpeed = beforeSpeed;
        characterManager.AnimationController.StopDashAnimation();
        characterManager.CharacterInfo.IsDashing = false;
    }

    protected abstract void MoveUpdate();
    protected abstract void LookUpdate();
}
