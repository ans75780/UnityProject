using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;

namespace Character.Enemy.FSM
{
    public class PatrolState : EnemyState
    {
        private int patrolIndex = 0;
        private int movementHash = Animator.StringToHash("Movement");
        
        public override void Enter(EnemyContext context)
        {
            int rand = 0;

            while (patrolIndex == rand)
            {
                rand = (int)UnityEngine.Random.Range(0, context.enemy.patrolSize - 1);
            }
            patrolIndex = rand;
            
            Debug.Log("Patrol Enter : " + patrolIndex);
            
            context.navMeshAgent.SetDestination(context.enemy.patrolPoints[patrolIndex]);
        }

        public override void Update(EnemyContext context, float deltaTime)
        {
            //도착했다면
            if (context.enemy.IsArrived())
            {
                context.fsm.ChangeState(typeof(IdleState));
            }
            
            context.animator.SetFloat(movementHash, context.navMeshAgent.velocity.magnitude);
        }

        public override void FixedUpdate(EnemyContext context, float deltaTime)
        {
            
        }

        public override void LateUpdate(EnemyContext context, float fixedDeltaTime)
        {
            
        }

        public override void Exit(EnemyContext context)
        {
            context.navMeshAgent.ResetPath();
            context.animator.SetFloat(movementHash, 0);
        }

        public override void OnVisionSensed(EnemyContext context, SenseHit hit)
        {
            if (hit.target.tag == "Player")
            {
                context.fsm.ChangeState(typeof(AlertState));
            }
        }

        public override void OnVisionLost(EnemyContext context, SenseHit hit)
        {
            
        }
        
        void Receive_OnTimeEnd(object data)
        {
            
        }
        
    }
}