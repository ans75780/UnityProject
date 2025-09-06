using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Character.Enemy.FSM
{
    public class ChaseState : EnemyState
    {
        private int chasingHash = Animator.StringToHash("Chasing");
        private int movementHash = Animator.StringToHash("Movement");
        
        private bool lostTarget = false;

        private float chaseSpeed = 2f;
        
        public override void Enter(EnemyContext context)
        {
            context.animator.SetBool(chasingHash, true);
            
            lostTarget = false;
            
            context.navMeshAgent.speed =  chaseSpeed;
        }

        public override void Update(EnemyContext context, float deltaTime)
        {
          
        }

        public override void FixedUpdate(EnemyContext context, float deltaTime)
        {
            Debug.Log("Lost Target : " +  lostTarget);
            
            if (lostTarget)
            {
                if (context.enemy.IsArrived())
                {
                    context.fsm.ChangeState(typeof(IdleState));
                }    
            }
            else
            {
                context.navMeshAgent.SetDestination(context.target.transform.position);
                if (context.enemy.InAttackRange() && 
                    context.enemy.IsFacingTarget(
                    context.enemy.transform, 
                    context.target.transform.position))
                {
                    context.navMeshAgent.ResetPath();
                    context.fsm.ChangeState(typeof(AttackState));
                    return;
                }
            }
            context.animator.SetFloat(movementHash, context.navMeshAgent.velocity.magnitude);
        }
        
        public override void LateUpdate(EnemyContext context, float fixedDeltaTime)
        {
            
        }

        public override void Exit(EnemyContext context)
        {
            context.animator.SetBool(chasingHash, false);
            context.animator.SetFloat(movementHash, 0f);
            
            context.navMeshAgent.speed = context.enemyData.MoveSpeed;
        }

        public override void OnVisionSensed(EnemyContext context, SenseHit hit)
        {
            lostTarget = false;
            context.target = hit.target;
            context.navMeshAgent.SetDestination(context.target.transform.position);
        }

        public override void OnVisionLost(EnemyContext context,SenseHit hit)
        {
            lostTarget = true;
            context.navMeshAgent.SetDestination(hit.lastKnownPos);
        }
    }
}