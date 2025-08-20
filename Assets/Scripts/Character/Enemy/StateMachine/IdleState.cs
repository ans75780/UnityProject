using Unity.Mathematics;

namespace Character.Enemy.StateMachine
{
    public class IdleState : IEnemyState
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
                context.fsm.ChangeState(typeof(AlertState));
            }
        }

        public override void OnVisionLost(EnemyContext context,SenseHit hit)
        {
            
        }
    }
}