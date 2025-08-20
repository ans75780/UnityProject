using System;
using Character.Player.StateMachine;
using UnityEngine;

public class ChagePlayerState : StateMachineBehaviour
{
    public enum EChangeStates
    {
        None,
        Idle,
        Move,
        Attack,
        Jump
    }
    
  
    
    public EChangeStates changeState;
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("TestCode");
        
        Type type = null;

        if (changeState == EChangeStates.None)
            return;
        
        switch (changeState)
        {
            case EChangeStates.Idle:
                type = typeof(IdleState);
                break;
            case EChangeStates.Move:
                type = typeof(MoveState);
                break;
            case EChangeStates.Attack:
                type = typeof(AttackState);
                break;
        }
        animator.GetComponent<Player>().FSM.ChangeState(type);
    }

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
