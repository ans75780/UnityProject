using System.Collections.Generic;
using System;
using System.Net.NetworkInformation;
using Character;
using Character.FSM;
using Character.Player;
using Character.Player.FSM;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerContext : BaseContext
{
    public Player player;
    public PlayerStateMachine fsm;
    public PlayerMotor motor;
    public PlayerInputAdapter adapter;
    public PlayerCombat combat;
    public float StateTime;
}

public abstract class PlayerState : BaseFSMState<PlayerContext>
{
    
    //해당 상태로 전환될 수 있는지 여부를 체크하는 함수.
    public virtual bool CheckCondition(PlayerContext context)
    {
        return true;
    }
    
    //특정 조건인지를 검사
    public virtual bool CanAttack(PlayerContext context)
    {
        return false;
    }

    public virtual void InputedContext(PlayerContext context, InputAction.CallbackContext inputContext)
    {
        
    }
}

public class PlayerStateMachine : BaseFSM<PlayerContext, PlayerState>
{
    public PlayerStateMachine(PlayerContext playerContext, PlayerState initState) : base(playerContext, initState)
    {
        context.fsm = this;
    }
    
    public override void OnPossess()
    {
        base.OnPossess();   
        
        PlayerInputAdapter adapter = context.player.GetComponent<PlayerInputAdapter>();
        adapter.OnInputContext += Receive_InputedContext;

        adapter.OnDodge += Receive_OnDodge;
    }

    public override void OnDisPossess()
    {
        base.OnDisPossess();
        
        PlayerInputAdapter adapter = context.player.GetComponent<PlayerInputAdapter>();
        
        adapter.OnInputContext -= Receive_InputedContext;
        adapter.OnDodge -= Receive_OnDodge;
    }
    
    public void Receive_InputedContext(InputAction.CallbackContext inputContext)
    {
        currentState.InputedContext(context, inputContext);
    }

    public void Receive_OnDodge(InputAction.CallbackContext context)
    {
        //stamina check구문 필요
        
        ChangeState(typeof(DodgeState));
    }
    
}
















