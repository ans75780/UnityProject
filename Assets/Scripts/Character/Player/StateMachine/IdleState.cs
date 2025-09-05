using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player.StateMachine
{
    public class IdleState : IPlayerState
    {
        int idleStateHash = Animator.StringToHash("Idle");

        public override void Enter(PlayerContext context)
        {
            Debug.Log("Enter IdleState");
        }

        public override void Update(PlayerContext context, float deltaTime)
        {
            if (context.adapter.InputAxis.sqrMagnitude > 0)
            {
                context.fsm.ChangeState(typeof(MoveState));
            }
        }
        
        public override void FixedUpdate(PlayerContext context, float fixedDeltaTime)
        {
        }

        public override void LateUpdate(PlayerContext context, float deltaTime)
        {
        }

        public override void Exit(PlayerContext context)
        {
            
        }
        public override void InputedContext(PlayerContext context, InputAction.CallbackContext inputContext)
        {
            if (context.animator.IsInTransition(0))
            {
                return;
            }
            if (inputContext.action.name.Equals("Attack"))
            {
                context.fsm.ChangeState(typeof(AttackState));
            }
            
            if (inputContext.action.name.Equals("Jump"))
            {
                context.fsm.ChangeState(typeof(JumpState));
            }
            
        }
    }
}