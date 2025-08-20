using System;
using System.Collections.Generic;
using Ables;
using Character.Enemy.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Sense sense;

    public SO_EnemyData enemyData;
    
    public float patrolRange;
    public int patrolSize = 3;
    
    public List<Vector3> patrolPoints;
    
    [SerializeField]
    private string currentStateName;
    
    private GameObject target;
    private NavMeshAgent navMeshAgent;
   
    private Animator animator;
    private Rigidbody rb;
    
    private EnemyStateMachine fsm;
 
    
    private Damageable damageable;
    
    public float knockBackPower = 1.5f;

    private Weapon weapon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sense = GetComponent<Sense>();

        weapon = GetComponentInChildren<Weapon>();

        if (weapon != null)
        {
            weapon.SetOwner(this.gameObject);
        }
        
        
        sense.AddModality(GetComponent<VisionSensor>());
        
        patrolPoints = new  List<Vector3>();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();
        
        rb = GetComponent<Rigidbody>();
        
       EnemyContext context = new EnemyContext();
        
        context.enemy = this;
        context.animator = animator;
        context.fsm = fsm;
        context.rigidbody = rb;
        context.enemyData = enemyData;
        context.navMeshAgent = navMeshAgent;
        
        fsm = new EnemyStateMachine(context, new IdleState());
        
        fsm.CreateState(typeof(PatrolState), new PatrolState());
        
        fsm.CreateState(typeof(ChaseState), new ChaseState());
        
        fsm.CreateState(typeof(AlertState), new AlertState());
        
        fsm.CreateState(typeof(AttackState), new AttackState());
        
        fsm.CreateState(typeof(HitState), new HitState());

        fsm.CreateState(typeof(DeadState), new DeadState());
        
        SetupPatrolPoints();

        damageable = GetComponent<Damageable>();

        damageable.MaxHealth = enemyData.MaxHp;
        
        navMeshAgent.speed = enemyData.MoveSpeed;
        
        SubscriptionEvents();
        
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    void FixedUpdate()
    {
        fsm.FixedUpdate(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        fsm.LateUpdate(Time.deltaTime);
    }
    
    void SetupPatrolPoints()
    {
        patrolPoints.Clear();
        
        for (int i = 0; i < patrolSize; i++)
        {
            float randX = UnityEngine.Random.Range(transform.position.x - patrolRange, transform.position.x + patrolRange);
            float randZ = UnityEngine.Random.Range(transform.position.z - patrolRange, transform.position.z + patrolRange);
            Vector3 patrolPoint = new Vector3(randX, 50 , randZ);
            
            Physics.Raycast(patrolPoint, Vector3.down, out RaycastHit hit, patrolRange);
            
            patrolPoint.y = hit.point.y;
         
            patrolPoints.Add(patrolPoint);
        }
    }

    public  bool IsArrived()
    {
        if (navMeshAgent.pathPending)
        {
            return false;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }
    
    public  bool InAttackRange()
    {
        if (navMeshAgent.pathPending)
        {
            return false;
        }
        
        if (navMeshAgent.remainingDistance <= enemyData.AttackRange)
        {
            return true;
        }
        return false;
    }
    
    public bool IsFacingTarget(Transform self, Vector3 targetPos, float maxAngleDeg = 3f)
    {
        Vector3 fwd = self.forward;      
        fwd.y = 0f;
        Vector3 to  = targetPos - self.position; to.y = 0f;

        if (to.sqrMagnitude < 1e-6f) return true; // 너무 가깝다면 정면으로 간주

        float angleDeg = Vector3.Angle(fwd, to);
        return angleDeg <= maxAngleDeg;
    }

    void OnEnable()
    {
       
    }

    void OnDisable()
    {
        
    }

    void SubscriptionEvents()
    {
        if (sense != null)
        {
            sense.OnVisionSensed += ReceiveOnVisionSensed;
            sense.OnVisionLost += ReceiveOnVisionLost;
        }

        if (damageable)
        {
            damageable.OnApplyDamage += ReceiveOnApplyDamage;
            damageable.OnDeath += ReceiveOnDeath;
        }

        if (fsm != null)
        {
            fsm.OnChangeState += ReceiveOnChangeState;
        }
        
    }

    void UnSubscriptionEvents()
    {
        if (sense != null)
        {
            sense.OnVisionSensed -= ReceiveOnVisionSensed;
            sense.OnVisionLost -= ReceiveOnVisionLost;
        }

        if (damageable)
        {
            damageable.OnApplyDamage -= ReceiveOnApplyDamage;
            damageable.OnDeath -= ReceiveOnDeath;
        }

        if (fsm != null)
        {
            fsm.OnChangeState -= ReceiveOnChangeState;
        }
    }
    
    void Notify_ChangeState(string newStateName)
    {
        fsm.ChangeState(Type.GetType("Character.Enemy.StateMachine." + newStateName));
    }
    
    void Notify_AttackStart()
    {
        weapon.OnAttackStart();
    }

    void Notify_AttackEnd()
    {
        weapon.OnAttackEnd();
    }
    
    void ReceiveOnChangeState(IEnemyState newState)
    {
        currentStateName = fsm.GetCurrentStateName();
    }
    
    void ReceiveOnApplyDamage(DamageInfo damageInfo)
    {
        Vector3 power = transform.position + (damageInfo.damageDirection * knockBackPower);
        
        rb.AddForce(power, ForceMode.Impulse);
        
        fsm.SetTarget(damageInfo.damageSource);
        fsm.ChangeState(typeof(HitState));
    }
    void ReceiveOnDeath(DamageInfo damageInfo)
    {
        navMeshAgent.ResetPath();
        
        UnSubscriptionEvents();
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        fsm.ChangeState(typeof(DeadState));
    }
    
    void ReceiveOnVisionSensed(SenseHit hit)
    {
        target = hit.target;
        
        fsm.OnVisionSensed(hit);
    }
    
    void ReceiveOnVisionLost(SenseHit hit)
    {
        fsm.OnVisionLost(hit);
        target = null;
    }
    
    
    
    
}
