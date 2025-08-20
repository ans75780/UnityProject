using System.Collections.Generic;
using System;
using Character;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public struct EnemyContext
{
    public Animator animator;
    public EnemyStateMachine fsm;
    public Enemy enemy;
    public Rigidbody rigidbody;
    public GameObject target;
    public NavMeshAgent navMeshAgent;
    public SO_EnemyData enemyData;
    public float StateTime;
}

public abstract class IEnemyState
{
    public abstract void Enter(EnemyContext context);
    public abstract void Update(EnemyContext context, float deltaTime);
    public abstract void FixedUpdate(EnemyContext context, float deltaTime);
    public abstract void LateUpdate(EnemyContext context, float fixedDeltaTime);
    public abstract void Exit(EnemyContext context);
    
    //특정 조건인지를 검사
    public virtual bool CanAttack(EnemyContext context)
    {
        return false;
    }
    
    //외부에서 임의로 해당 스테이트를 끝내야할때 호출한다.
    //전이되는 스테이트는 종료될 스테이트에서 정함.
    public virtual void EndState(EnemyContext context)
    {
        
    }
    

    public abstract void OnVisionSensed(EnemyContext context, SenseHit hit);

    public abstract void OnVisionLost(EnemyContext context, SenseHit hit);

    
    
}

public class EnemyStateMachine
{
    public IEnemyState CurrentState {
        get
        {
            return currentState;
        }
    }
    
    private IEnemyState currentState;
    
    private EnemyContext context;
    
    private Dictionary<Type, IEnemyState> states;
    
    public delegate void ChangeStateHandler(IEnemyState newState);
    public event ChangeStateHandler OnChangeState;

    public string GetCurrentStateName()
    {
        return currentState.GetType().Name;
    }
    
    public EnemyStateMachine(EnemyContext _context, IEnemyState InitState)
    {
        context = _context;
        context.fsm = this;
        currentState = InitState;
        currentState.Enter(context);
        
        states = new Dictionary<Type, IEnemyState>();
        CreateState(InitState.GetType(), InitState);
    }

    public void CreateState(Type type, IEnemyState state)
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

    public void ChangeState(IEnemyState changeState)
    {
        if (changeState == currentState) return;

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

    public void SetTarget(GameObject target)
    {
        context.target = target;
    }
    
    public void OnVisionSensed(SenseHit hit)
    {
        context.target = hit.target;
        
        currentState.OnVisionSensed(context, hit);
    }

    public void OnVisionLost(SenseHit hit)
    {
        context.target = null;
        
        currentState.OnVisionLost(context, hit);
    }
    
    public void Notify_ChangeState(Type stateType)
    {
        ChangeState(stateType);
    }

    
}
















