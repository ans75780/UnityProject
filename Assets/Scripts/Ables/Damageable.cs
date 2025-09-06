using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

namespace Ables
{
    public enum DamageType
    {
        Default, //기본 공격 타입, 피격 애니메이션을 출력한다.
        Knockback, //멀리 날아가야하는 경우에 붙여준다.
        Tick, //독뎀, 화상뎀같은 틱마다 발생하 효과, 피격 애니메이션을 출력하지 않는다.
    }
    
    

    public struct DamageInfo
    {
        public float amount;
        public GameObject damageSource;
        public Vector3 damageDirection;
        public DamageType damageType;
    }


    public class Damageable : MonoBehaviour
    {
        [SerializeField] float maxHealth;

        private float maxHpRatio = 1f;
        private float minHpRatio = 0f;
        
        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        [SerializeField] float currentHealth;

        public float CurrentHealth
        {
            get { return currentHealth; }
        }

        public float invincibilityDuration = 0.5f;

        private bool isInvincible = false;

        public bool IsInvincible
        {
            get { return isInvincible; }
        }

        public delegate void ApplyDamageHandler(DamageInfo damageInfo);

        public event ApplyDamageHandler OnApplyDamage;

        public delegate void DeathHandler(DamageInfo damageInfo);

        public event DeathHandler OnDeath;
        
        public delegate void ChangedHpRatioHandler(float ratio);
        
        public event  ChangedHpRatioHandler OnChangedHpRatio;
        
        public void ApplyDamage(DamageInfo damageInfo)
        {
            if (currentHealth == 0f)
                return;

            //나머지 처리는 일단 패스
            currentHealth = Mathf.Clamp(currentHealth - damageInfo.amount, 0f, maxHealth);

            Debug.Log(this.transform.name + " damage applied : " + damageInfo.amount);

            if (currentHealth == 0f)
            {
                OnDeath(damageInfo);
                OnChangedHpRatio(minHpRatio);

            }
            else
            {
                if (damageInfo.damageType != DamageType.Tick)
                {
                    OnApplyDamage(damageInfo);
                }
                OnChangedHpRatio(GetRatio());
            }
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            OnChangedHpRatio(GetRatio());
        }

        public void Reset()
        {
            currentHealth = maxHealth;
            OnChangedHpRatio?.Invoke(maxHpRatio);
        }
        
        public bool IsDead()
        {
            return currentHealth == 0f;
        }

        public void Start()
        {
            currentHealth = MaxHealth;
        }

        public float GetRatio()
        {
            return currentHealth / maxHealth;
        }
        
    }
}