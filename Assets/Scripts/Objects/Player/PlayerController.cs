using System;
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
    private bool invulnerable = false;
    private bool canAttack = true;

    // COMPONENTS
    private Animator myAnimator;
    private HealthSystem healthSystem;
    private SpriteRenderer mySpriteRenderer;

    // MODIFICATION PROPERTIES
    public float TempMoveSpeed { get; private set; }
    public float TempAttackCooldown { get; private set; }
    public float TempInvulnerableTime { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0;
        currentDirection = Vector2.zero;
        myAnimator = GetComponentInChildren<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        healthSystem = FindAnyObjectByType<HealthSystem>();
    }

    void LateUpdate()
    {
        MovePlayer();

        CheckPlayerOutOfBounds();

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }

        ResetTempVariables();
    }

    private void ResetTempVariables()
    {
        TempMoveSpeed = basePlayerSpeed;
        TempAttackCooldown = baseAttackCooldown;
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

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        myAnimator.SetTrigger("attack");

        float timePassed = 0;
        while (timePassed < baseAttackCooldown)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        canAttack = true;
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
            mySpriteRenderer.flipX = direction.x > 0 || (direction.x >= 0 && mySpriteRenderer.flipX);
        }
        else currentVelocity -= basePlayerAcceleration * decelerationScalar * Time.deltaTime;

        if (currentVelocity > playerSpeed) currentVelocity = playerSpeed;
        if (currentVelocity < 0) currentVelocity = 0;
        
        transform.Translate(currentVelocity * Time.deltaTime * currentDirection);
    }

    private float GetCurrentPlayerSpeed()
    {
        return TempMoveSpeed;
    }

    public IEnumerator MultiplyMoveSpeed(float scalar, float duration = float.PositiveInfinity)
    {
        while (duration > 0)
        {
            TempMoveSpeed *= scalar;
            yield return null;
            duration -= Time.time;
        }
    }

    public IEnumerator MultiplyAttackSpeed(float scalar, float duration = float.PositiveInfinity)
    {
        while (duration > 0)
        {
            TempAttackCooldown /= scalar;
            yield return null;
            duration -= Time.time;
        }
    }

    private IEnumerator InvulnerableCoroutine(float duration)
    {
        invulnerable = true;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        invulnerable = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !invulnerable)
        {
            Debug.Log("Took Player Damage!");
            healthSystem.TakeDamage();
            StartCoroutine(InvulnerableCoroutine(baseInvulnerableTime));
        }
    }
}
