using UnityEngine;

namespace Character.Player.StateMachine
{
    public class JumpState : IPlayerState
    {
        private int isJumpHash = Animator.StringToHash("IsJumping");
        
        public override void Enter(PlayerContext context)
        {
            Debug.Log("Enter JumpState");

            context.animator.SetTrigger(isJumpHash);


            Vector3 jumpForce = Vector3.up;
            
          //  context.rigidbody.AddForce(jumpForce * 
          //      context.playerController.jumpPower, ForceMode.Force);
            
        }
        
        public override void Update(PlayerContext context, float deltaTime)
        {
           
        }

        public override void FixedUpdate(PlayerContext context, float fixedDeltaTime)
        {
            //하강중인지
            bool isFalling = context.rigidbody.linearVelocity.y < -0.01f;
            
            if (isFalling)
            {
                
            }
            else
            {
                
            }
        }

        public override void LateUpdate(PlayerContext context, float deltaTime)
        {
            
        }

        public override void Exit(PlayerContext context)
        {
            
        }
    }
}