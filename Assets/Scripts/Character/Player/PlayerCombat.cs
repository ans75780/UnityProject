using System;
using UnityEngine;

namespace Character.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private Weapon currentWeapon;
        
        private Transform weaponSocket;
        public string socketName;
        
        
        private bool enableNextAttack = false;
        
        public bool EnableNextAttack {
            get
            {
                return enableNextAttack;
            }
        }
        
        private bool isWeaponEquipped = false;

        public bool IsWeaponEquipped
        {
            get
            {
                return isWeaponEquipped;
            }
        }
        
        private void Awake()
        {
            weaponSocket = GameObject.Find(socketName).GetComponent<Transform>();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            UnEquip();
        }
        
        public void Equip(GameObject weaponObject)
        {
            //PopupInventory
            
            currentWeapon = weaponObject.GetComponent<Weapon>();
            
            currentWeapon.transform.SetParent(weaponSocket, false);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
            
            currentWeapon.SetOwner(this.gameObject);

            isWeaponEquipped = true;
        }
        
        public void UnEquip()
        {
            if (currentWeapon)
            {
                currentWeapon.ClearOwner();

                Destroy(currentWeapon);
                currentWeapon = null;

                //ToInventory
                
            }
            isWeaponEquipped = false;
        }
        
        void Notify_AttackStart()
        {
            currentWeapon.OnAttackStart();
        }

        void Notify_AttackEnd()
        {
            currentWeapon.OnAttackEnd();
        }
        
        void Notify_EnableNextAttack()
        {
            enableNextAttack = true;
        }

        void Notify_DisableNextAttack()
        {
            enableNextAttack = false;
        }
        
    }
}