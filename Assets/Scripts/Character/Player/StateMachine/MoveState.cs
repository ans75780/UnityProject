using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player.StateMachine
{
    public class MoveState : IPlayerState
    {
        private int movementHash = Animator.StringToHash("Movement");
        
        public override void Enter(PlayerContext context)
        {
            Debug.Log("Enter MoveState");
        }

        public override void Update(PlayerContext context, float deltaTime)
        {
            if (context.adapter.InputAxis.sqrMagnitude < 0.1)
            {
                context.fsm.ChangeState(typeof(IdleState));
            }
        }

        public override void FixedUpdate(PlayerContext context, float fixedDeltaTime)
        {
            GameObject playerCamera = context.motor.playerCamera;
            
            Vector3 forwardVector = playerCamera.transform.forward;

            forwardVector.y = 0;
            forwardVector.Normalize();
            
            Vector3 rightVector =  playerCamera.transform.right;
            rightVector.y = 0;
            rightVector.Normalize();
            
            Vector3 direction = ((forwardVector * context.adapter.InputAxis.y) + (rightVector * context.adapter.InputAxis.x));
        
            if (direction.magnitude > 0.1)
            {   
                context.motor.Move(direction.normalized);
            }
        }

        public override void LateUpdate(PlayerContext context, float fixedDeltaTime)
        {
            context.animator.SetFloat(movementHash, context.adapter.InputAxis.sqrMagnitude);
        }

        public override void Exit(PlayerContext context)
        {
            context.animator.SetFloat(movementHash, 0);
        }

        public override void InputedContext(PlayerContext context, InputAction.CallbackContext inputContext)
        {
            if (inputContext.action.name.Equals("Attack"))
            {
                context.fsm.ChangeState(typeof(AttackState));
            }
        }
    }
}