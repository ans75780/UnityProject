using Ables;
using UnityEngine;

namespace Character.Ability.Abilities
{
    [CreateAssetMenu(fileName = "Ability_Posion", menuName = "Scriptable Objects/Abilities/Ability_Posion")]
    public class Ability_Posion : ScriptableObject,  IAbility
    {
        [SerializeField]
        private float damage;

        [Range(1, 100), SerializeField]
        private float duration;
        
        
        [Range(1, 10), SerializeField]
        public float executeTick;

        [SerializeField]
        private bool loop = false;
        
        
        private DamageInfo damageInfo;
        
        public void Setup(GameObject owner)
        {
            damageInfo = new DamageInfo();

            damageInfo.damageSource = owner;
            damageInfo.damageType = DamageType.Tick;
            damageInfo.amount = damage;
            damageInfo.damageSource = null;
            damageInfo.damageDirection = Vector3.zero;
        }

        public void Reset(GameObject Owner)
        {
          
        }
        
        public void Execute(GameObject owner)
        {
            Damageable damageable = owner.GetComponent<Damageable>();

            if (damageable == null) return;
            
            damageable.ApplyDamage(damageInfo);
        }

        public float GetDuration()
        {
            return duration;
        }

        public float GetExecuteTick()
        {
            return executeTick;
        }

        public bool GetHasLoop()
        {
            return loop;
        }
    }
}