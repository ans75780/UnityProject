using System;
using Ables;
using Character.Player;
using Character.Player.StateMachine;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerCombat = Character.Player.PlayerCombat;


public class Player : MonoBehaviour
{
    private int movementHash = Animator.StringToHash("Movement");
    private int isRollHash = Animator.StringToHash("IsRoll");
    private int hitAngleHash = Animator.StringToHash("HitAngle");
    
    Damageable damageable;
    
    private PlayerMotor playerMotor;
    
    private PlayerCombat playerCombat;
    
    private PlayerStateMachine fsm;

    
    
    public PlayerStateMachine FSM
    {
        get { return fsm; }
    }
    public GameObject weapon;
    
    Animator animator;
    
    private int playerLevel;

    private Rigidbody rb;
    private bool isDead = false;
    
    
    [SerializeField]
    private string currentStateName;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMotor =  GetComponent<PlayerMotor>();
        playerCombat = GetComponent<PlayerCombat>();
        
        
        PlayerContext context =  new PlayerContext();
        context.animator = animator;
        context.rigidbody = GetComponent<Rigidbody>();
        context.player = this;
        context.adapter = GetComponent<PlayerInputAdapter>();
        context.playerMotor = playerMotor;

        rb = GetComponent<Rigidbody>();
        damageable =  GetComponent<Damageable>();
        
        fsm = new PlayerStateMachine(context, new IdleState());
        fsm.CreateState(typeof(MoveState), new MoveState());
        fsm.CreateState(typeof(AttackState), new AttackState());
        fsm.CreateState(typeof(HitState), new HitState());
        fsm.CreateState(typeof(DeadState), new DeadState());
        
        currentStateName = fsm.GetCurrentStateName();
    }
    
    void OnEnable()
    {
        isDead = false;

        damageable.OnApplyDamage += ReceiveOnApplyDamage;
        damageable.OnDeath += ReceiveOnDeath;
        fsm.OnChangeState += ReceiveOnChangeState;
    }
    
    void OnDisable()
    {
        isDead = true;
        damageable.OnApplyDamage -= ReceiveOnApplyDamage;
        damageable.OnDeath -= ReceiveOnDeath;
        fsm.OnChangeState -= ReceiveOnChangeState;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLevel = 1;

        if (weapon)
        {
            GameObject weaponObject = Instantiate(weapon);
            playerCombat.Equip(weaponObject);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isDead)
            fsm.Update(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (!isDead)
            fsm.FixedUpdate(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (!isDead)
            fsm.LateUpdate(Time.deltaTime);
    }
    
    void ReceiveOnChangeState(IPlayerState newState)
    {
        currentStateName = fsm.GetCurrentStateName();
    }
    
    void ReceiveOnApplyDamage(DamageInfo damageInfo)
    {
        if (isDead)
            return;
        
        Vector3 forward = transform.forward;
        Vector3 direction = damageInfo.damageDirection.normalized;
        
        rb.AddForce(direction * 1.5f,  ForceMode.Impulse);
        
        float dot =  Vector3.Dot(forward, direction);
        //Debug.Log("Hit : " + dot);
        
        animator.SetFloat(hitAngleHash, dot);
        fsm.ChangeState(typeof(HitState));
    }

    void ReceiveOnDeath(DamageInfo damageInfo)
    {
        isDead = true;
        fsm.ChangeState(typeof(DeadState));
    }
}
