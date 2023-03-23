using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script for moving player around. 
/// </summary>
public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jump = 20f;
    [SerializeField]
    private Transform feet;
    [SerializeField]
    private LayerMask groundLayers;
    [SerializeField]
    Animator animator;
    private bool facingRight = true;
    private float movementX;
    private Rigidbody2D rb;

    /// <summary>
    /// Gets info needed (rigidbody)
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// Gets triggered, when we press keys for move
    /// </summary>
    /// <param name="context">Info from input manager</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<float>();
        // movementX is -1 when moving left, 0 when not moving and 1 when moving right
        if(movementX == 0 ) {
            animator.CrossFadeInFixedTime("Idle", 0.1f, 0);
        }
        else {
            animator.CrossFadeInFixedTime("Walk", 0.1f, 0);
        }
    }

    /// <summary>
    /// Gets triggered, when we press button to jump
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            Jump();
        }
    }


    /// <summary>
    /// Handles moving of player
    /// </summary>
    void FixedUpdate()
    {
        if (movementX < 0 && facingRight) //starts moving in the opoite direction
        {
            Flip();
        }
        else if (movementX > 0 && !facingRight)
        {
            Flip();
        }
        Vector2 movement = new Vector2(movementX * speed, rb.velocity.y);
        rb.velocity = movement;
    }

    /// <summary>
    /// Jumps player
    /// </summary>
    void Jump()
    {
        if (TouchesGround())
        {
            animator.Play("Jump", 0);
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Checks, if we are touching ground by feet object, so we dont jump till infinity
    /// </summary>
    /// <returns>True if we touch ground by feet</returns>
    bool TouchesGround()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);
        if (groundCheck != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Makes player face the other way around, including weapon
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    /// <summary>
    /// Sets player to face right
    /// </summary>
    public void FaceRight()
    {
        if (!facingRight)
        {
            Flip();
        }
    }
}
