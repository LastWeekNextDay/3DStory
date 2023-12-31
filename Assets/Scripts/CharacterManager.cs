using System;
using System.Collections;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour
{
    [SerializeField] private CharacterInfo _characterInfo;
    public CharacterInfo CharacterInfo { get => _characterInfo; protected set => _characterInfo = value; }
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public AudioSource AudioSource => audioSource;
    [SerializeField] private CharacterSounds characterSounds;
    public CharacterSounds CharacterSounds => characterSounds;
    
    [Header("Weapon")]
    [SerializeField] private GameObject holdingSpace;
    [SerializeField] private GameObject weaponObject;
    public GameObject WeaponObject
    {
        get => weaponObject;
        set
        {
            if (value == null)
            {
                weaponObject = null;
                CharacterInfo.EquippedWeapon = null;
                return; 
            }

            if (!value.TryGetComponent(out Weapon weapon)) return;
            weapon.OnHitHardObstacle += controller.StopAttack;
            weapon.Side = CharacterInfo.side;
            CharacterInfo.EquippedWeapon = weapon;
            weaponObject = value;
            weaponObject.transform.SetParent(holdingSpace.transform);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            weaponObject.transform.localScale = Vector3.one;
        }
    }
    
    [Header("Character Essentials")]
    [SerializeField] private Rigidbody rigidBody;
    public Rigidbody RigidBody => rigidBody;
    public AnimationController AnimationController { get; private set; }
    public Controller controller;
    [SerializeField] private Collider collisionCollider;
    public Collider Collider => collisionCollider;
    
    [SerializeField] private bool canRagdoll;
    public bool CanRagdoll
    {
        get => canRagdoll;
        protected set
        {
            canRagdoll = value;
            if (canRagdoll)
            {
                CharacterInfo.OnDeath -= HandleAnimDeath;
                CharacterInfo.OnDeath += HandleRagdollDeath;
            }
            else
            {
                CharacterInfo.OnDeath -= HandleRagdollDeath;
                CharacterInfo.OnDeath += HandleAnimDeath;
            }
        }
    }
    [SerializeField] private bool isRagdolled;
    public bool IsRagdolled
    {
        get => isRagdolled;
        set {
            if (!CanRagdoll) return;
            isRagdolled = value;
            AnimationController.SetAnimatorActive(!value);
            foreach (var rb in GetComponentsInChildren<Rigidbody>())
            {
                if (rb.transform.parent != null)
                {
                    if (rb.transform.parent.gameObject.name == "HoldingSpace") continue;    
                }
                else
                {
                    rb.isKinematic = value;
                    continue;
                }
                rb.isKinematic = !value;    
            }
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                if (col.transform.parent != null)
                {
                    if (col.transform.parent.gameObject.name == "HoldingSpace") continue;
                }
                else
                {
                    col.enabled = !value;
                    continue;
                }
                col.enabled = value;
            }
            controller.enabled = !value;
        }
    }

    private Action<CharacterManager, WeaponName> _onRequestWeapon;

    protected virtual void Awake()
    {
        AnimationController = new AnimationController(GetComponent<Animator>());

        _onRequestWeapon += GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>()
            .AssignWeaponToCharacter;
        
        if (holdingSpace == null)
        {
            throw new Exception(gameObject.name + ": HoldingSpace not found! Create a child object called HoldingSpace.");
        }
        
        if (WeaponObject == null)
        {
            _onRequestWeapon?.Invoke(this, WeaponName.DefaultSword);
        } else
        {
            WeaponObject = WeaponObject;
        }
        
        CanRagdoll = canRagdoll;
        IsRagdolled = isRagdolled;

        CharacterInfo.OnDeath += controller.StopMovement;
        CharacterInfo.OnDeath += controller.StopAttack;
        CharacterInfo.OnDeath += () => Destroy(controller.TargetToMoveTo);
        CharacterInfo.OnDeath += DeathInfoHandling;
        CharacterInfo.OnTakeDamage +=
            (_, vector3_dir, vector3_pos) => GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<VFXManager>().PlayHitVFX(
                CharacterInfo.Material, vector3_pos, Quaternion.LookRotation(-vector3_dir), transform);
        if (characterSounds.HurtSound != null){
            CharacterInfo.OnTakeDamage += (_,_,_) => AudioSource.PlayOneShot(characterSounds.HurtSound);
        }
    }

    private void Start()
    {
        CharacterInfo.RealSpeed = CharacterInfo.logicalSpeed;
        AudioSource.clip = characterSounds.IdleSound;
        if (AudioSource.clip != null)
        {
            AudioSource.loop = true;
            AudioSource.Play();    
        }
    }

    protected virtual void Update()
    {
        CharacterInfo.LowerAttackCooldown(Time.deltaTime);
        CharacterInfo.LowerDashCooldown(Time.deltaTime);
    }

    private void DeathInfoHandling()
    {
        CharacterInfo.IsDead = true;
        if (characterSounds.DeathSound != null)
        {
            AudioSource.PlayOneShot(characterSounds.DeathSound);
        }
    }

    private void HandleAnimDeath()
    {
        AnimationController.DoDeathAnimation();
        StartCoroutine(nameof(HideBodyByScaling));
    }

    private void HandleRagdollDeath()
    {
        IsRagdolled = true;
    }
    
    protected IEnumerator HideBodyByScaling()
    {
        yield return new WaitForSeconds(2f);
        while (transform.localScale.magnitude > 0.1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 2f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
