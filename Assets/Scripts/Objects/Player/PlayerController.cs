using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void PlayerEvent();
public class PlayerController : MonoBehaviour
{
    public event PlayerEvent EnemyDefeatedEvent;
    public event PlayerEvent TookDamageEvent;

    // OBJECT DATA
    public AudioClip attackSound;
    public AudioClip runeTimeSound;

    // BASE VALUES
    public float basePlayerSpeed = 3;
    public float basePlayerAcceleration = 16;
    public float decelerationScalar = 1;
    public float baseInvulnerableTime = .25f;
    public float baseAttackCooldown = 1;
    public float baseRuneCooldownScalar = 1;
    public float baseDashSpeed = 10;
    public float baseDashTime = .1f;

    // MOVEMENT VARIABLES
    private float currentVelocity;
    private Vector2 currentDirection;

    // COMBAT VARIABLES
    private bool invulnerable = false;
    private bool playerHitThisFrame = false;
    public bool CanAttack { get; private set;}
    public bool CanUseRune { get; private set; }
    public bool Dashing { get; private set; }

    // COMPONENTS
    private Animator myAnimator;
    private HealthSystem healthSystem;
    private SpriteRenderer mySpriteRenderer;

    // MODIFICATION PROPERTIES
    public float TempMoveSpeed { get; private set; }
    public float TempAttackCooldown { get; private set; }
    public RuneCard Rune { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0;
        currentDirection = Vector2.zero;
        CanAttack = true;
        CanUseRune = true;
        myAnimator = GetComponentInChildren<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        healthSystem = GetComponent<HealthSystem>();
    }

    void LateUpdate()
    {
        MovePlayer();

        CheckPlayerOutOfBounds();

        if (Input.GetMouseButtonDown(0) && CanAttack)
        {
            StartCoroutine(AttackCoroutine());
        }

        if (Input.GetMouseButtonDown(1))
        {
            UseRune();
        }

        if (playerHitThisFrame && !invulnerable) PlayerHit();

        SetAnimationVars();
        LogPlayerStats();
        ResetTempVariables();
    }

    private void SetAnimationVars()
    {
        myAnimator.SetFloat("velocity", currentVelocity);
        myAnimator.SetBool("invulnerable", invulnerable && !Dashing);
    }

    private void ResetTempVariables()
    {
        TempMoveSpeed = basePlayerSpeed;
        TempAttackCooldown = baseAttackCooldown;
        invulnerable = false;
        playerHitThisFrame = false;
    }

    private void LogPlayerStats()
    {
        Debug.Log("Movement Speed: " + TempMoveSpeed);
        Debug.Log("Attack Cooldown: " + TempAttackCooldown);
    }

    private void UseRune()
    {
        if (Rune == null) return;
        switch (Rune.runeID)
        {
            case CardManager.RuneID.Time:
                FindAnyObjectByType<SoundController>().PlaySound(runeTimeSound, volume: .5f);
                FindAnyObjectByType<TimeManager>().ChangeSceneTime(.25f, 3); // Slow time for 3 seconds
                break;
            case CardManager.RuneID.War:
                // Dash
                StartCoroutine(DashCoroutine(baseDashTime));
                break;
            case CardManager.RuneID.Death:
                foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                {
                    enemy.TakeDamage();
                }
                break;
            case CardManager.RuneID.Life:
                healthSystem.Heal(1);
                break;
        }
        StartCoroutine(RuneCooldown(Rune.runeCooldown * baseRuneCooldownScalar));
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
        CanAttack = false;
        myAnimator.SetTrigger("attack");
        FindAnyObjectByType<SoundController>().PlaySound(attackSound, volume: .5f);

        float timePassed = 0;
        while (timePassed < TempAttackCooldown)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        CanAttack = true;
    }

    private void MovePlayer()
    {
        /// Player Movement
        if (!Dashing)
        {
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
        }

        transform.Translate(currentVelocity * Time.deltaTime * currentDirection);
    }

    private float GetCurrentPlayerSpeed()
    {
        return TempMoveSpeed;
    }

    public void TriggerEnemyDefeatedEvent()
    {
        EnemyDefeatedEvent?.Invoke();
    }

    public void SetRune(RuneCard card)
    {
        Rune = card;
        // SET RUNE IMAGE
        FindAnyObjectByType<RuneDisplay>().UpdateRune(Rune);
    }

    private void PlayerHit()
    {
        healthSystem.TakeDamage();
        StartCoroutine(InvulnerableCoroutine(baseInvulnerableTime));
        TookDamageEvent?.Invoke();
    }

    public IEnumerator DashCoroutine(float dashTime)
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        StartCoroutine(InvulnerableCoroutine(dashTime));
        Dashing = true;
        if (direction == Vector2.zero) currentDirection = (GetComponentInChildren<PlayerAttackCenter>().transform.position - transform.position).normalized;
        while (dashTime > 0)
        {
            currentVelocity = baseDashSpeed;
            yield return null;
            dashTime -= Time.deltaTime;
        }
        Dashing = false;
    }

    public IEnumerator MultiplyMoveSpeed(float scalar, float duration = float.PositiveInfinity)
    {
        while (duration > 0)
        {
            TempMoveSpeed *= scalar;
            yield return null;
            duration -= Time.deltaTime;
        }
    }

    public IEnumerator MultiplyAttackSpeed(float scalar, float duration = float.PositiveInfinity)
    {
        while (duration > 0)
        {
            TempAttackCooldown /= scalar;
            yield return null;
            duration -= Time.deltaTime;
        }
    }

    private IEnumerator InvulnerableCoroutine(float duration)
    {
        while (duration > 0)
        {
            invulnerable = true;
            duration -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RuneCooldown(float duration)
    {
        CanUseRune = false;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        CanUseRune = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !invulnerable)
        {
            Debug.Log("Took Player Damage!");
            playerHitThisFrame = true;
        }
    }
}
