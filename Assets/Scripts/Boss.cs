using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float stoppingDistance = 1f;
    // Time interval before teleporting
    public float teleportCooldown = 2f;
    // Range to teleport away from the player
    public float teleportRange = 8f;

    public DetectionZone attackZone;
    public Collider2D losDetection;
    public ScoreManager scoreManager;
    private bool scoreAdded = false;
    public Transform target;
    public GameManager gameManager;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    private bool teleporting = false;
    private float teleportTimer = 0f;
    
    // Animation trigger
    public bool isCasting = false;

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (losDetection.IsTouching(target.GetComponent<Collider2D>()))
        {
            if(CanMove)
            {
                // Move towards the player
                Vector2 direction = new Vector2(target.position.x - transform.position.x, 0).normalized;
                // Calculate distance to the target
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (distanceToTarget >= stoppingDistance)
                {
                    rb.velocity = direction * walkAcceleration;

                    if (direction.x > 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else if (direction.x < 0)
                    {
                        transform.localScale = Vector3.one;
                    }
                }
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 1), rb.velocity.y);
            }
            
            // Start teleporting timer if the boss is near the player
            teleportTimer += Time.deltaTime;
            
            if (teleportTimer >= teleportCooldown && !teleporting && (Vector2.Distance(transform.position, target.position) <= teleportRange))
            {
                StartCoroutine(TeleportAway());
            }
        }
        else
        {
            HasTarget = false;
            // Stop moving if player is not in line of sight
            rb.velocity = Vector2.zero;
            teleportTimer = 0f;
        }

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    IEnumerator TeleportAway()
    {
        teleporting = true;

        // Trigger casting animation
        animator.SetTrigger("Cast");
        isCasting = true;
        damageable.SetInvincibility(true);

        // Wait for the casting animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isCasting = false;

        // Calculate a random position along the X-axis within the specified range
        float teleportX = transform.position.x + Random.Range(-teleportRange - 10, teleportRange + 10);
        Vector3 teleportPosition = new Vector3(teleportX, transform.position.y, transform.position.z);

        transform.position = teleportPosition;

        // Ensure a small delay to avoid multiple teleports in quick succession
        yield return new WaitForSeconds(2f);
        teleporting = false;
        // Reset teleport timer after teleporting
        teleportTimer = 0f;
    }

    private void FixedUpdate()
    {
        if (!damageable.IsAlive && !scoreAdded)
        {
            ScorePoints();
            scoreAdded = true;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    void ScorePoints()
    {
        // Add points to the score
        scoreManager.AddScore(2000);
    }
}
