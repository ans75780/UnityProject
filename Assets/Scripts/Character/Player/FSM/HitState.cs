using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player.FSM
{
    public class HitState : PlayerState
    {
        private int isHit = Animator.StringToHash("OnHit");
        
        public HitState()
        {
   
        }
        
        public override void Enter(PlayerContext context)
        {
            context.animator.SetTrigger(isHit);
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