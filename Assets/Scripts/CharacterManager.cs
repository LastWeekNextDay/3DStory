using System;
using Prefabs;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterManager : MonoBehaviour
{
    public CharacterInfo CharacterInfo;
    
    [SerializeField] private PrefabContainer prefabContainer;
    [SerializeField] private GameObject holdingSpace;
    [SerializeField] private GameObject weaponObject;
    private GameObject WeaponObject
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
            weaponObject.transform.SetParent(holdingSpace.transform);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            weaponObject.transform.localScale = Vector3.one;
        }
    }
    [SerializeField] private Rigidbody rigidBody;
    public Rigidbody RigidBody => rigidBody;
    public AnimationController AnimationController { get; private set; }

    protected void Awake()
    {
        AnimationController = new AnimationController(GetComponent<Animator>());
        if (WeaponObject == null)
        {
            if (prefabContainer.TryGetByName("Sword1", out var weapon))
            {
                WeaponObject = Instantiate(weapon);
            }
            else
            {
                throw new Exception(gameObject.name + ": Default Weapon not found!");
            }
        }
        WeaponObject.transform.SetParent(holdingSpace.transform);
        WeaponObject.transform.localPosition = Vector3.zero;
        WeaponObject.transform.localRotation = Quaternion.identity;
    }

    private void Start()
    {
        CharacterInfo.CurrentSpeed = CharacterInfo.DefaultSpeed;
    }

    protected void Update()
    {
        CharacterInfo.LowerAttackCooldown(Time.deltaTime);
        CharacterInfo.LowerDashCooldown(Time.deltaTime);
        CharacterInfo.HealthUpdate();
    }
}
