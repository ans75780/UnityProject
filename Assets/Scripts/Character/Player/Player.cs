using System;
using Ables;
using Character.Player;
using Character.Player.StateMachine;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;






public class Player : MonoBehaviour
{
    private int movementHash = Animator.StringToHash("Movement");
    private int isRollHash = Animator.StringToHash("IsRoll");
    private int hitAngleHash = Animator.StringToHash("HitAngle");
    
    Damageable damageable;
    
    private PlayerMotor playerMotor;
    
    public PlayerStateMachine fsm;
    
    public float InitMaxHealth;

    Animator animator;

    private Weapon weapon;
    
    private int playerLevel;

    private Rigidbody rb;
    private bool isDead = false;
    
    
    [SerializeField]
    private string currentStateName;
    
    void OnEnable()
    {
        damageable = GetComponent<Damageable>();

        isDead = false;
        
        if (damageable != null)
        {
            damageable.MaxHealth = InitMaxHealth;
        }
        
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMotor =  GetComponent<PlayerMotor>();
        
        PlayerContext context =  new PlayerContext();
        context.animator = animator;
        context.rigidbody = GetComponent<Rigidbody>();
        context.player = this;
        context.adapter = GetComponent<PlayerInputAdapter>();
        context.playerMotor = playerMotor;

        rb = GetComponent<Rigidbody>();
        damageable =  GetComponent<Damageable>();
        
        damageable.OnApplyDamage += ReceiveOnApplyDamage;
        damageable.OnDeath += ReceiveOnDeath;
        
        
        fsm = new PlayerStateMachine(context, new IdleState());
        fsm.CreateState(typeof(MoveState), new MoveState());
        fsm.CreateState(typeof(AttackState), new AttackState());
        fsm.CreateState(typeof(HitState), new HitState());
        fsm.CreateState(typeof(DeadState), new DeadState());
        
        
        fsm.OnChangeState += ReceiveOnChangeState;

        currentStateName = fsm.GetCurrentStateName();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLevel = 1;
        
        weapon = GetComponentInChildren<Weapon>();
        
        if (weapon)
        {
            weapon.SetOwner(this.gameObject);
            
            weapon.OnWeaponDamageCalc += OnWeaponDamageCalc;
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
    
    void OnAttack()
    {
       
    }

    void OnRoll()
    {
        animator.SetTrigger(isRollHash); 
    }
    
    float OnWeaponDamageCalc(float weaponDamage)
    {
        return weaponDamage * playerLevel;
    }

    void ReceiveOnChangeState(IPlayerState newState)
    {
        currentStateName = fsm.GetCurrentStateName();
    }

    void Notify_AttackStart()
    {
        weapon.OnAttackStart();
    }

    void Notify_AttackEnd()
    {
        weapon.OnAttackEnd();
    }
    
    void Notify_EnableNextAttack()
    {
     //   playerController.EnableNextAttack = true;
    }

    void Notify_DisableNextAttack()
    {
       // playerController.EnableNextAttack = false;
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


    public void SubcribeEvent()
    {
        
    }

    public void UnSubcribeEvent()
    {
        
    }
}
