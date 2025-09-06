
using UnityEngine;

namespace Character.Ability
{
    public class AbilityWrapper
    {
        private IAbility ability;

        public IAbility Ability
        {
            get { return ability; }
            private set { }
        }

        private float duration;

        public float Duration
        {
            get { return duration; }
            private set { }
        }
        
        private float executeTick;
        
        private bool loop;

        public bool Loop
        {
            get { return loop; }
            private set { }
        }
        
        
        public AbilityWrapper(GameObject owner, IAbility _ability)
        {
            ability = _ability;
            ability.Setup(owner);
            
            duration = ability.GetDuration();
            executeTick = ability.GetExecuteTick();
            loop = ability.GetHasLoop();
            
            
        }
        
        public virtual void Tick(float deltaTime)
        {
            duration -= deltaTime;
            executeTick -= deltaTime;
        }

        public virtual bool CanExecute()
        {
            return executeTick <= 0 && duration >= 0;
        }

        public virtual void Execute(GameObject owner)
        {
            ability.Execute(owner);
            executeTick = ability.GetExecuteTick();
        }

        public void Reset()
        {
            duration = ability.GetDuration();
            executeTick = ability.GetExecuteTick();
        }
    }
    
    public interface IAbility
    {
        public void Setup(GameObject Owner);
        
        public void Reset(GameObject Owner);
        
        public void Execute(GameObject owner);

        public float GetDuration();

        public float GetExecuteTick();

        public bool GetHasLoop();

    }
}