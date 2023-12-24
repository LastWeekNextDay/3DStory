using System;
using System.Collections;
using Prefabs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterManager : MonoBehaviour
{
    public CharacterInfo CharacterInfo;
    
    [SerializeField] private GameManager gameManager;
    private GameObject _holdingSpace;
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
            weapon.Side = CharacterInfo.Side;
            CharacterInfo.EquippedWeapon = weapon;
            weaponObject = value;
            weaponObject.transform.SetParent(_holdingSpace.transform);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            weaponObject.transform.localScale = Vector3.one;
        }
    }
    [DoNotSerialize] public Rigidbody RigidBody { get; private set; }
    public AnimationController AnimationController { get; private set; }
    
    public Controller Controller { get; private set; }

    protected void Awake()
    {
        AnimationController = new AnimationController(GetComponent<Animator>());
        RigidBody = GetComponent<Rigidbody>();
        
        if (gameManager == null)
        {
            throw new Exception(gameObject.name + ": GameManager not assigned!");
        }

        GameObject FindHoldingSpace(Transform transformRoot)
        {
            foreach (Transform child in transformRoot)
            {
                if (child.gameObject.name == "HoldingSpace")
                {
                    return child.gameObject;
                }
                var childHoldingSpace = FindHoldingSpace(child);
                if (childHoldingSpace != null)
                {
                    return childHoldingSpace;
                }
            }

            return null;
        }
        
        Controller = GetComponent<Controller>();
        
        _holdingSpace = FindHoldingSpace(transform);
        if (_holdingSpace == null)
        {
            throw new Exception(gameObject.name + ": HoldingSpace not found! Create a child object called HoldingSpace.");
        }
        
        if (WeaponObject == null)
        {
            gameManager.AssignWeaponToCharacter(this, WeaponName.DefaultSword);
        }
    }

    private void Start()
    {
        CharacterInfo.RealSpeed = CharacterInfo.LogicalSpeed;
    }

    protected void Update()
    {
        CharacterInfo.LowerAttackCooldown(Time.deltaTime);
        CharacterInfo.LowerDashCooldown(Time.deltaTime);
        CharacterInfo.HealthUpdate();
    }
    
    protected void SetRagdoll(bool isRagdoll)
    {
        AnimationController.SetAnimatorActive(!isRagdoll);
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = !isRagdoll;
        }
        Controller.enabled = !isRagdoll;
        WeaponObject = WeaponObject;
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
