using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player.FSM
{
    public class MoveState : PlayerState
    {
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
           
        }

        public override void Exit(PlayerContext context)
        {
            
        }

        public override void InputedContext(PlayerContext context, InputAction.CallbackContext inputContext)
        {
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