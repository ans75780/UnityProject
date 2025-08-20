using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerMotor : MonoBehaviour
{
    public GameObject playerCamera;
    
    private Rigidbody rigidbody;
    
    public float walkSpeed = 5;
    public float rotateSpeed = 15;
    public float jumpPower = 10;

    public float sphereRadius = 0.5f;
    public  float checkDistance = 0.5f;
    
    public LayerMask groundMask;
    
    [SerializeField]    
    private bool isGrounded;

    [SerializeField]
    private bool enableNextAttack;
    
    
    private float velocity;
    
    public bool EnableNextAttack {
        get { return enableNextAttack; }
        set
        {
            enableNextAttack = value;
        }
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }
    
    void Update()
    {
        IsGrounded();
    }

    bool IsGrounded()
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * checkDistance), sphereRadius);
    }
}
