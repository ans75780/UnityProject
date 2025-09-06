using Unity.Mathematics;
using UnityEngine;

namespace Character.Enemy.FSM
{
    public class HitState : EnemyState
    {
        private int onHitHash = Animator.StringToHash("OnHit");


        public override void Enter(EnemyContext context)
        {
            context.animator.SetTrigger(onHitHash);
        }

        public override void Update(EnemyContext context, float deltaTime)
        {
          
        }

        public override void FixedUpdate(EnemyContext context, float deltaTime)
        {
            
        }

        public override void LateUpdate(EnemyContext context, float fixedDeltaTime)
        {
            
        }

        public override void Exit(EnemyContext context)
        {
            
        }

        public override void OnVisionSensed(EnemyContext context, SenseHit hit)
        {
           
        }

        public override void OnVisionLost(EnemyContext context,SenseHit hit)
        {
            
        }
    }
}