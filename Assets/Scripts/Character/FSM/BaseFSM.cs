using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.FSM
{
    public abstract class BaseContext
    {
        public Animator animator;
        public Rigidbody rigidbody;
    }
    
    public abstract class BaseFSMState<T>
    {
        public abstract void Enter(T context);
        public abstract void Update(T context, float deltaTime);
        public abstract void FixedUpdate(T context, float deltaTime);
        public abstract void LateUpdate(T context, float fixedDeltaTime);
        public abstract void Exit(T context);
        public virtual bool CheckCondition(T context)
        {
            return true;
        }
    }
    
    public abstract class BaseFSM<TContext, TState>
        where TState : BaseFSMState<TContext>
        where TContext : BaseContext
    {
        protected TContext context;
        
        protected TState currentState;
        
        protected TState requestState;

        private Dictionary<Type, TState> states;

        
        public delegate void ChangeStateHandler(TState newState);
        public event ChangeStateHandler OnChangeState;
        
        
        public BaseFSM(TContext context, TState initState)
        {
            this.context = context;
            currentState = initState;
            states = new Dictionary<Type, TState>();
            CreateState(initState.GetType(), initState);
        }
        public virtual void OnPossess()
        {
            currentState.Enter(context);
        }
        
        public virtual void OnDisPossess()
        {
            
        }
        
        public void CreateState(Type type, TState state)
        {
            states.Add(type, state);
        }
        
        public virtual void Update(float deltaTime)
        {
            currentState.Update(context, deltaTime);

            if (requestState != null)
            {
                CheckRequestState();
            }
        }
    
        public virtual void FixedUpdate(float deltaTime)
        {
            currentState.FixedUpdate(context, deltaTime);
        }
        public virtual  void LateUpdate(float deltaTime)
        {
            currentState.LateUpdate(context, deltaTime);
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

        public void ChangeState(TState changeState)
        {
            if (changeState == currentState) return;
            if (changeState.CheckCondition(context) == false) return;
        
            currentState.Exit(context);

            currentState = changeState;

            currentState.Enter(context);

            OnChangeState?.Invoke(currentState);
        }
    
        public void RequestNextState(Type stateType)
        {
            if (states.ContainsKey(stateType))
            {
                requestState = states[stateType];
            }
            else
            {
                Debug.LogError("Not Found : " + stateType.ToString());
            }
        }

        public void CheckRequestState()
        {
            if (currentState.CheckCondition(context) == true && 
                context.animator.IsInTransition(0) == false)
            {
                ChangeState(requestState);
                requestState = null;
            }
        }
        
        public string GetCurrentStateName()
        {
            return currentState.GetType().Name;
        }
        
    }
}