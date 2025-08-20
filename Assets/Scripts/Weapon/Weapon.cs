using System;
using System.Xml.Schema;
using Ables;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    
    GameObject owner;

    public event Func<float, float> OnWeaponDamageCalc;

    public float weaponDamage = 5;
    
    public void SetOwner(GameObject _owner)
    {
        owner = _owner;
        
        
        int ownerMask = 1 << owner.layer;
        int weaponMask = 1 << LayerMask.NameToLayer("Weapon");
        
        //오너와 무기는 충돌에서 제외
        capsuleCollider.excludeLayers = ownerMask | weaponMask;
    }

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAttackStart()
    {
        capsuleCollider.enabled = true;
        
    }

    public void OnAttackEnd()
    {
        capsuleCollider.enabled = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Enter : "  + other.gameObject.name);
        
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable == null || damageable.IsInvincible == true) return;
        
        DamageInfo damageInfo = new DamageInfo();
        float totalDamage = 0;

        if (OnWeaponDamageCalc != null)
        {
            totalDamage = OnWeaponDamageCalc(weaponDamage);
        }
        else
        {
            totalDamage = weaponDamage;
        }
        damageInfo.amount = totalDamage;
        damageInfo.damageSource = owner;
        damageInfo.damageDirection = owner.transform.forward.normalized;
        damageable.ApplyDamage(damageInfo);
    }
}
