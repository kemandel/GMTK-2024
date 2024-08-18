using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // BASE VALUES
    public float basePlayerSpeed = 3;
    public float basePlayerAcceleration = 16;
    public float decelerationScalar = 1;
    public float baseInvulnerableTime = .25f;
    public float baseAttackCooldown = 1;

    // MOVEMENT VARIABLES
    private float currentVelocity;
    private Vector2 currentDirection;

    // COMBAT VARIABLES
    private float invulnerableTimeCounter = 0;
    private float attackCooldownCounter = 0;

    // COMPONENTS
    private Animator myAnimator;
    private HealthSystem healthSystem;

    // MODIFICATION PROPERTIES
    public float MoveSpeedScalar { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0;
        currentDirection = Vector2.zero;
        myAnimator = GetComponentInChildren<Animator>();
        healthSystem = FindAnyObjectByType<HealthSystem>();

        MoveSpeedScalar = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        CheckPlayerOutOfBounds();

        if (Input.GetMouseButtonDown(0) && attackCooldownCounter >= baseAttackCooldown)
        {
            Attack();
        }
        else if (attackCooldownCounter < baseAttackCooldown) attackCooldownCounter += Time.deltaTime;

        if (invulnerableTimeCounter < baseInvulnerableTime) invulnerableTimeCounter += Time.deltaTime;
    }

    public void AddToMoveSpeedScalar(float movementPercent)
    {
        MoveSpeedScalar += movementPercent;
    }

    private void CheckPlayerOutOfBounds()
    {
        Vector2 boundsExtents = CameraManager.EdgeExtents;
        Vector2 playerExtents = GetComponentInChildren<BoxCollider2D>().bounds.extents;

        if (transform.position.x - playerExtents.x < -boundsExtents.x) transform.position = new Vector2(-boundsExtents.x + playerExtents.x, transform.position.y);
        else if (transform.position.x + playerExtents.x > boundsExtents.x) transform.position = new Vector2(boundsExtents.x - playerExtents.x, transform.position.y);

        if (transform.position.y - playerExtents.y < -boundsExtents.y) transform.position = new Vector2(transform.position.x, -boundsExtents.y + playerExtents.y);
        else if (transform.position.y + playerExtents.y > boundsExtents.y) transform.position = new Vector2(transform.position.x, boundsExtents.y - playerExtents.y);
    }

    private void Attack()
    {
        myAnimator.SetTrigger("attack");
        attackCooldownCounter = 0;
    }

    private void MovePlayer()
    {
        /// Player Movement
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float playerSpeed = GetCurrentPlayerSpeed();

        if (direction != Vector2.zero)
        {
            currentDirection = direction;
            currentVelocity += basePlayerAcceleration * Time.deltaTime;
        }
        else currentVelocity -= basePlayerAcceleration * decelerationScalar * Time.deltaTime;

        if (currentVelocity > playerSpeed) currentVelocity = playerSpeed;
        if (currentVelocity < 0) currentVelocity = 0;
        
        transform.Translate(currentVelocity * Time.deltaTime * currentDirection);
    }

    private float GetCurrentPlayerSpeed()
    {
        return basePlayerSpeed * MoveSpeedScalar;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && invulnerableTimeCounter >= baseInvulnerableTime)
        {
            Debug.Log("Took Player Damage!");
            healthSystem.TakeDamage();
            invulnerableTimeCounter = 0;
        }
    }
}
