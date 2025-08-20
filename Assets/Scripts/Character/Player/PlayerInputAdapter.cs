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
        
        private InputAction moveAction;
       
        private InputAction attackAction;
        
        
        public delegate void InputContextHandler(InputAction.CallbackContext context);
        
        public event InputContextHandler OnInputContext;


        private Vector2 inputAxis;
        public Vector2 InputAxis { get { return inputAxis; } }
        
        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
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
            attackAction.started += ReceiveInput;
            
        }

        void OnDisable()
        {
            moveAction.started -= ReceiveOnMove;
            moveAction.performed -= ReceiveOnMove;
            moveAction.canceled -= ReceiveOnMove;
            
            attackAction.started -= ReceiveInput;
        }

        void ReceiveOnMove(InputAction.CallbackContext context)
        {
            inputAxis = context.ReadValue<Vector2>();
        }
        
        void ReceiveInput(InputAction.CallbackContext context)
        { 
            OnInputContext?.Invoke(context);
        }
    }
}