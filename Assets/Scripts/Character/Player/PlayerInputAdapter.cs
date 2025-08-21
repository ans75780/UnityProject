using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ables;
namespace Character.Player
{
    enum EInputAction
    {
        Move,
        Attack,
        Jump,
        Roll,
        SKill1,
        SKill2,
        Skill3
    }
    
    public class PlayerInputAdapter : MonoBehaviour
    {
        private PlayerInput playerInput;
        
        private PlayerCombat playerCombat;
        
        private InputAction moveAction;
       
        private InputAction attackAction;

        private InputAction dodgeAction;
        
        
        public delegate void InputContextHandler(InputAction.CallbackContext context);
        
        public event InputContextHandler OnInputContext;

        //AnyState
        public event InputContextHandler OnDodge;
        
        [SerializeField]
        private Vector2 inputAxis;
        public Vector2 InputAxis { get { return inputAxis; } }
        
        
        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
         
            playerCombat = GetComponent<PlayerCombat>();
            
        }
        
        void Start()
        {
          
        }
        void OnEnable()
        {
            InputActionMap actionMap = playerInput.actions.FindActionMap("Default");
            
            moveAction = playerInput.actions.FindAction("Move");

            //moveAction.started += ReceiveInput;
            //moveAction.performed += ReceiveInput;
            //moveAction.canceled += ReceiveInput;
            
            moveAction.started += ReceiveOnMove;
            moveAction.performed += ReceiveOnMove;
            moveAction.canceled += ReceiveOnMove;
            
            attackAction = playerInput.actions.FindAction("Attack");
            attackAction.started += ReceiveOnAttack;
            
            dodgeAction = playerInput.actions.FindAction("Dodge");
            dodgeAction.started += ReceiveOnDodge;
            
        }

        void OnDisable()
        {
            moveAction.started -= ReceiveOnMove;
            moveAction.performed -= ReceiveOnMove;
            moveAction.canceled -= ReceiveOnMove;
            
            attackAction.started -= ReceiveOnAttack;
            
            dodgeAction.started -= ReceiveOnDodge;
            
        }

        void ReceiveOnMove(InputAction.CallbackContext context)
        {
            inputAxis = context.ReadValue<Vector2>();
        }
        
        void ReceiveOnAttack(InputAction.CallbackContext context)
        {
            if (playerCombat.IsWeaponEquipped)
            {
                OnInputContext.Invoke(context);
            }
        }

        void ReceiveOnDodge(InputAction.CallbackContext context)
        {
            Debug.Log("DodgeTest");
            
            OnDodge.Invoke(context);
        }
        
        void ReceiveInput(InputAction.CallbackContext context)
        { 
            OnInputContext?.Invoke(context);
        }
    }
}