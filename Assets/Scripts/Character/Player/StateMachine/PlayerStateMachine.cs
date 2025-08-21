using System.Collections.Generic;
using System;
using Character;
using Character.Player;
using Character.Player.StateMachine;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct PlayerContext
{
    public Animator animator;
    public Player player;
    public PlayerStateMachine fsm;
    public PlayerMotor motor;
    public PlayerInputAdapter adapter;
    public PlayerCombat combat;
    public Rigidbody rigidbody;
    public float StateTime;
}

public abstract class IPlayerState
{
    public abstract void Enter(PlayerContext context);
    public abstract void Update(PlayerContext context, float deltaTime);
    public abstract void FixedUpdate(PlayerContext context, float deltaTime);
    public abstract void LateUpdate(PlayerContext context, float fixedDeltaTime);
    public abstract void Exit(PlayerContext context);
    
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

public class PlayerStateMachine
{
    public IPlayerState CurrentState {
        get
        {
            return currentState;
        }
    }
    
    private IPlayerState currentState;
    
    private PlayerContext context;
    
    private Dictionary<Type, IPlayerState> states;
    
    public delegate void ChangeStateHandler(IPlayerState newState);
    public event ChangeStateHandler OnChangeState;
    

    
    public string GetCurrentStateName()
    {
        return currentState.GetType().Name;
    }
    
    public PlayerStateMachine(PlayerContext _context, IPlayerState InitState)
    {
        context = _context;
        context.fsm = this;
        currentState = InitState;
        currentState.Enter(context);
        
        states = new Dictionary<Type, IPlayerState>();
        CreateState(InitState.GetType(), InitState);


        PlayerInputAdapter adapter = context.player.GetComponent<PlayerInputAdapter>();
        
        adapter.OnInputContext += Receive_InputedContext;

        adapter.OnDodge += Receive_OnDodge;

    }

    ~PlayerStateMachine()
    {
        PlayerInputAdapter adapter = context.player.GetComponent<PlayerInputAdapter>();
        
        adapter.OnInputContext -= Receive_InputedContext;
        adapter.OnDodge -= Receive_OnDodge;
    }
    
    public void CreateState(Type type, IPlayerState state)
    {
        states.Add(type, state);
    }
    
    public void ChangeState(Type stateType)
    {
        if (states.ContainsKey(stateType))
        {
            ChangeState(states[stateType]);
        }
        else
        {
            Debug.LogError("Not Found : " + stateType.ToString());
        }
    }

    public void ChangeState(IPlayerState changeState)
    {
        if (changeState == currentState) return;
        if (changeState.CheckCondition(context) == false) return;
        
        currentState.Exit(context);

        currentState = changeState;

        currentState.Enter(context);

        OnChangeState?.Invoke(currentState);
    }

    public void Update(float deltaTime)
    {
        currentState.Update(context, deltaTime);
    }
    
    public void FixedUpdate(float deltaTime)
    {
        currentState.FixedUpdate(context, deltaTime);
    }
    public void LateUpdate(float deltaTime)
    {
        currentState.LateUpdate(context, deltaTime);
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
    
    public void Notify_ChangeState(Type stateType)
    {
        ChangeState(stateType);
    }
}
















