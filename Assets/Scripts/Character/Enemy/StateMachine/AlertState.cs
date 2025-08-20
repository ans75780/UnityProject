using Unity.Mathematics;
using UnityEngine;

namespace Character.Enemy.StateMachine
{
    public class AlertState : IEnemyState
    {
        private float waitForTime;

        private float waitLatency;
        
        private int alertHash = Animator.StringToHash("Alert");
        
        
        public override void Enter(EnemyContext context)
        {
            waitForTime = 3f;
            waitLatency = waitForTime;
            
            context.animator.SetTrigger(alertHash);
        }

        public override void Update(EnemyContext context, float deltaTime)
        {
            waitLatency -= deltaTime;

            if (waitLatency <= 0)
            {
                context.fsm.ChangeState(typeof(ChaseState));
            }
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