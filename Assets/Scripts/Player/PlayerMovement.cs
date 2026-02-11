using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed = 5f; // Speed of the player movement
    public float groundDrag = 6f; // Drag when on the ground
    
    [Header("Jumping")] 
    public float jumpForce = 5f; // Force applied when jumping
    public float jumpCooldown = 0.25f; // Cooldown time between jumps
    public float airMultiplier = 0.5f; // Multiplier for movement in the air
    bool readyToJump; 

    [Header("Keybinds")] 
    public KeyCode jumpKey = KeyCode.Space; // Key to jump 

    [Header("Ground check")] 
    public float playerHeight = 2f; // Height of the player for ground check 
    public LayerMask groundLayer; // Layer mask for ground detection 
    bool isGrounded;

    public Transform orientation; // Reference to the orientation object 
    
    // Input axes
    float horizontalInput; 
    float verticalInput; 
    
    // direction
    Vector3 moveDirection; 
    
    // Rigidbody
    Rigidbody rb; 
    private void Start() 
    { 
        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true; 
        
        // Prevent rotation due to physics
        readyToJump = true; 
    } 
    
    private void Update() { 
        
        // Ground check
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer); 
        MyInput(); 
        SpeedControl(); 
        
        // Apply drag when grounded
        if (isGrounded) 
            rb.drag = groundDrag; 
        else rb.drag = 0; 
    } 
    private void FixedUpdate() 
    {
        MovePlayer(); 
    } 
    private void MyInput() 
    { 
        // Get input from keyboard
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        verticalInput = Input.GetAxisRaw("Vertical"); 
        
        // Jumping
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded) 
        { 
            readyToJump = false; 
            Jump(); 
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    } 
    
    private void MovePlayer() 
    { 
        // Calculate move direction based on orientation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; 
        // Move the player
        if(isGrounded) 
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); 
        else if(!isGrounded) 
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); 
    } 
    private void SpeedControl() 
    { 
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed) 
        { 
            Vector3 limitedVel = flatVel.normalized * moveSpeed; rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); 
        } 
    } 
    private void Jump() 
    { 
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
        // Reset vertical velocity
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); 
        // Apply jump force
    } 
    private void ResetJump() 
    { 
        readyToJump = true;
    } 
}
