using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player.StateMachine
{
    public class AttackState : IPlayerState
    {
        private int isAttackHash = Animator.StringToHash("IsAttack");
        private int comboCountHash = Animator.StringToHash("ComboCount");

        
        public int comboCount = 0;
        
        public override void Enter(PlayerContext context)
        {
            comboCount = 0;
            Debug.Log("Enter AttackState");
    
            context.animator.SetTrigger(isAttackHash);
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
            comboCount = 0;
            //나가기 전 애니메이터의 콤보카운트 초기화
            context.animator.SetInteger(comboCountHash, 0);
        }
        
        public override void InputedContext(PlayerContext context, InputAction.CallbackContext inputContext)
        {
            if (inputContext.action.name.Equals("Attack"))
            {
                if (context.combat.EnableNextAttack)
                {
                    context.animator.SetTrigger(isAttackHash);
                    context.animator.SetInteger(comboCountHash, ++comboCount);
                    context.combat.EnableNextAttack = false;
                }
            }
        }
    }
}