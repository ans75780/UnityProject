using System;
using Character.Player;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerMotor : MonoBehaviour
{
    public GameObject playerCamera;
    
    private Rigidbody rigidbody;
    private PlayerInputAdapter adapter;
    
    
    public float walkSpeed = 5;
    public float rotateSpeed = 15;
    public float jumpPower = 10;

    public float sphereRadius = 0.5f;
    public  float checkDistance = 0.5f;
    
    public LayerMask groundMask;
    
    [SerializeField]    
    private bool isGrounded;
    
    public bool IsGrounded
    {
        get { return isGrounded; }
    }
    
    [SerializeField] 
    private bool isFalling;

    public bool IsFalling
    {
        get { return isFalling; }
    }
    
    private float velocity;
    
    private int isFallingHash = Animator.StringToHash("IsFalling");
    private int isGroundedHash = Animator.StringToHash("IsGrounded");
    private int movementHash = Animator.StringToHash("Movement");
    private Animator animator;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        adapter = GetComponent<PlayerInputAdapter>();
        isGrounded = true;
    }
    
    void Update()
    {
        CalcIsGrounded();
        
        isFalling = rigidbody.linearVelocity.y < -0.01f;
        
    }

    void LateUpdate()
    {
        animator.SetBool(isFallingHash, IsFalling);                
        animator.SetBool(isGroundedHash, IsGrounded);
        animator.SetFloat(movementHash, adapter.InputAxis.magnitude);
    }

    bool CalcIsGrounded()
    {
        isGrounded = Physics.CheckSphere(
            transform.position + (Vector3.down * checkDistance), 
            sphereRadius, 
            groundMask);
        
        return isGrounded;
    }

    public void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.0001f)
            direction.Normalize();
        
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, 
            newRotation, rotateSpeed * Time.fixedDeltaTime);
            
        Vector3 velocity = direction * (walkSpeed * Time.fixedDeltaTime);
        
        rigidbody.MovePosition(transform.position + velocity);
    }

    public void Turn(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void Jump()
    {
        Vector3 jumpForce = Vector3.up * jumpPower;
        
        rigidbody.AddForce(jumpForce, ForceMode.Impulse);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * checkDistance), sphereRadius);
    }
}
