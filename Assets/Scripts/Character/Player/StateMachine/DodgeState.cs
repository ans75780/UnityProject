using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Character.Player.StateMachine
{
    public class DodgeState :IPlayerState
    {
        private int onDodgeHash = Animator.StringToHash("OnDodge");
        
        public override void Enter(PlayerContext context)
        {
            Debug.Log("Enter DodgeState");
            
            GameObject playerCamera = context.motor.playerCamera;
        
            Vector3 forwardVector = playerCamera.transform.forward;

            forwardVector.y = 0;
            forwardVector.Normalize();
        
            Vector3 rightVector =  playerCamera.transform.right;
            rightVector.y = 0;
            rightVector.Normalize();
        
            Vector2 inputAxis  = context.adapter.InputAxis;

            Vector3 direction;
            
            if (inputAxis != Vector2.zero)
            {   
                direction = ((forwardVector * context.adapter.InputAxis.y) + 
                             (rightVector * context.adapter.InputAxis.x));
                
                Quaternion rotation = Quaternion.LookRotation(direction);
                context.motor.Turn(rotation);
            }
            
            context.animator.SetTrigger(onDodgeHash);
        }

        public override void Update(PlayerContext context, float deltaTime)
        {
        
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
            
        }
    }
}