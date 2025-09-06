using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Ability
{
    public class AbilitySystem : MonoBehaviour
    {
        public List<ScriptableObject> defaultHasAbilities = new List<ScriptableObject>();
        public Dictionary<Type, ScriptableObject> abilitiesDict;
        
        private List<AbilityWrapper> wrappers = new List<AbilityWrapper>();
        
        void Awake()
        {
          
        }
        
        void OnEnable()
        {
            foreach (IAbility ability in defaultHasAbilities)
            {
                wrappers.Add(new AbilityWrapper(this.gameObject,ability));
            }
        }

        void OnDisable()
        {
            wrappers.Clear();
        }

        void Start()
        {
           
        }
        
        void Update()
        {
            for (int i = wrappers.Count - 1; i >= 0; i--)
            {
                var wrapper = wrappers[i];
                wrapper.Tick(Time.deltaTime);

                if (wrapper.CanExecute())
                {
                    wrapper.Execute(this.gameObject);
                }
                else if (wrapper.Duration <= 0f)
                {
                    if (wrapper.Loop)
                    {
                        wrapper.Reset();
                    }
                    else
                    {
                        wrappers.RemoveAt(i); // 안전하게 삭제 가능
                    }
                }
            }
        }

        void FixedUpdate()
        {
            
        }
    }
}