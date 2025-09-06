using Unity.Mathematics;
using UnityEngine;

namespace Character.Enemy.FSM
{
    public class IdleState : EnemyState
    {
        private float waitForTime;

        private float waitLatency;
        
        public override void Enter(EnemyContext context)
        {
            waitForTime = UnityEngine.Random.Range(3f, 7f);
            waitLatency = waitForTime;
        }

        public override void Update(EnemyContext context, float deltaTime)
        {
            waitLatency -= deltaTime;

            if (waitLatency <= 0)
            {
                context.fsm.ChangeState(typeof(PatrolState));
            }

            if (context.target != null)
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
            if (hit.target.tag == "Player")
            {
                Debug.Log("Sensed Target");
                context.fsm.ChangeState(typeof(AlertState));
            }
        }

        public override void OnVisionLost(EnemyContext context,SenseHit hit)
        {
            
        }
    }
}