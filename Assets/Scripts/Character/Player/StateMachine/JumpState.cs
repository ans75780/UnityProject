using UnityEngine;

namespace Character.Player.StateMachine
{
    public class JumpState : IPlayerState
    {
        
        private int onJumpHash = Animator.StringToHash("OnJump");
        
        public override void Enter(PlayerContext context)
        {
            Debug.Log("Enter JumpState");
            context.motor.Jump();
            context.animator.SetTrigger(onJumpHash);
        }
        
        public override void Update(PlayerContext context, float deltaTime)
        {
           
        }

        public override void FixedUpdate(PlayerContext context, float fixedDeltaTime)
        {
          
        }

        public override bool CheckCondition(PlayerContext context)
        {
            return context.motor.IsGrounded && context.motor.IsFalling == false;
        }

        public override void LateUpdate(PlayerContext context, float deltaTime)
        {
            
        }

        public override void Exit(PlayerContext context)
        {
        }
    }
}