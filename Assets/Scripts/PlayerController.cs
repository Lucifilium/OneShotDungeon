using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Move speed
    public float moveSpeed = 5f;
    // Jump force
    public float jumpImpulse = 10f;
    // Dash
    private bool isDashing = false;
    public float dashSpeed = 7f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 1f;
    private float lastDashTime = 0f;

    public GameManager gameManager;

    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    public float CurrentMoveSpeed 
    { 
        get 
        {
            // Allows movement
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (isDashing)
                    {
                        return dashSpeed;
                    }
                    return moveSpeed;
                }
                else
                {
                    // Idle speed
                    return 0;
                }
            }
            else
            {
                // Movement locked
                return 0;
            }
        } 
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
        {
            return _isMoving;
        }
        private set 
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            // Flip only if value is new
            if (_isFacingRight != value)
            {
                // Flip the player to face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    // Play on start
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
            
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    private void Update()
    {
        if (!IsAlive)
        {
            gameManager.GameOver();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // X and Y movement input
        moveInput = context.ReadValue<Vector2>();
        if(IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
        
    }

    // Set character direction
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //Face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            //Face left
            IsFacingRight = false;
        }
    }

    // Jump trigger
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    // Attack trigger
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    // On hit, get knockback
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    // Dash trigger
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && CanDash() && IsAlive)
        {
            StartCoroutine(Dash());
        }
    }

    private bool CanDash()
    {
        return Time.time >= lastDashTime + dashCooldown;
    }

    private IEnumerator Dash()
    {
        // Enable invincibility during the dash
        damageable.SetInvincibility(true);
        isDashing = true;
        lastDashTime = Time.time;

        animator.SetTrigger(AnimationStrings.dash);

        float dashEndTime = Time.time + dashDuration;

        while (Time.time < dashEndTime)
        {
            // Apply the dash velocity to the rigidbody
            rb.velocity = new Vector2(dashSpeed, 0);

            yield return null;
        }

        isDashing = false;
    }

    // Kills player if entered killzone trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("KillZone"))
        {
            damageable.IsAlive = false;
        }
    }
}
