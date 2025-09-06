using System;
using Character.Player;
using Character.Player.FSM;
using Character.Enemy.FSM;
using UnityEngine;

public class ExitToIdle : StateMachineBehaviour
{
    
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);

        Debug.Log("ExitToIdle");
        
        string ownerTag = animator.transform.tag;

        if (ownerTag == "Player")
        {
            PlayerStateMachine fsm = animator.GetComponent<Player>().FSM;
        
            fsm.RequestNextState(typeof(Character.Player.FSM.IdleState));            
        }
        else if (ownerTag == "Enemy")
        {
            Debug.Log("Exit Enemy");
            
            EnemyStateMachine fsm = animator.GetComponent<Enemy>().FSM;
            
            fsm.RequestNextState(typeof(Character.Enemy.FSM.IdleState));
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
