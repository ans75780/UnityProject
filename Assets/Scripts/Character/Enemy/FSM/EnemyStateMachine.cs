using System.Collections.Generic;
using System;
using Character;
using Character.FSM;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemyContext : BaseContext
{
    public EnemyStateMachine fsm;
    public Enemy enemy;
    public GameObject target;
    public NavMeshAgent navMeshAgent;
    public SO_EnemyData enemyData;
    public float StateTime;
}

public abstract class EnemyState : BaseFSMState<EnemyContext>
{
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

public class EnemyStateMachine : BaseFSM<EnemyContext, EnemyState>
{
    public EnemyStateMachine(EnemyContext enemyContext, EnemyState initState) : base(enemyContext, initState)
    {
        context.fsm = this;
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
















