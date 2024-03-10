using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBat : MonoBehaviour
{
    public ScoreManager scoreManager;
    public DetectionZone biteDetectionZone;
    public float chaseSpeed = 2f;

    //Distance at which bat stops chasing
    public float stoppingDistance = 0.8f;
    public Transform target;
    public Collider2D deathCollider;
    private bool scoreAdded = false;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;

        if (damageable.IsAlive)
        {
            if (HasTarget)
            {
                // Calculate direction to the target
                Vector2 direction = (target.position - transform.position).normalized;

                // Calculate distance to the target
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                // Move towards the target if distance is greater than stoppingDistance
                if (distanceToTarget > stoppingDistance)
                {
                    rb.velocity = direction * chaseSpeed;

                    // Flip sprite if necessary (assuming the sprite faces right by default)
                    if (direction.x < 0)
                        transform.localScale = new Vector3(-1, 1, 1);
                    else if (direction.x > 0)
                        transform.localScale = Vector3.one;
                }
                else
                {
                    // Stop moving if within stoppingDistance
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                // Stop moving if no target or target lost
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            // Dead bat falls to the ground
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
            deathCollider.enabled = true;
        }

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void FixedUpdate()
    {
        if (!damageable.IsAlive && !scoreAdded)
        {
            ScorePoints();
            scoreAdded = true;
        }
    }

    void ScorePoints()
    {
        // Add points to the score
        scoreManager.AddScore(50);
    }
}
