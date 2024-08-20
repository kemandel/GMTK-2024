using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public delegate void PlayerEvent();
public class PlayerController : MonoBehaviour
{
    public event PlayerEvent EnemyDefeatedEvent;
    public event PlayerEvent TookDamageEvent;

    // OBJECT DATA
    public AudioClip attackSound;
    public GameObject vine;

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
    public bool Invulnerable { get; private set; }
    public bool PlayerHitThisFrame = false;
    public bool CanAttack { get; private set;}
    public bool CanUseRune { get; private set; }
    public bool Dashing { get; private set; }

    // COMPONENTS
    private Animator myAnimator;
    private HealthSystem healthSystem;
    private SpriteRenderer mySpriteRenderer;
    private Rigidbody2D myRigidbody;

    // MODIFICATION PROPERTIES
    public float TempMoveSpeed { get; private set; }
    public float TempAttackCooldown { get; private set; }
    public RuneCard Rune { get; private set; }
    public Coroutine currentRuneCoroutine;

    //UI VARIABLES
    RuneDisplay runeDisplay;
    UIFollow playerAttackBar;

    private void Awake()
    {
        runeDisplay = FindAnyObjectByType<RuneDisplay>();
    }

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
        myRigidbody = GetComponent<Rigidbody2D>();
        playerAttackBar = FindAnyObjectByType<UIFollow>();

        //turn off cooldown UI
        playerAttackBar.gameObject.GetComponentsInChildren<Image>()[0].enabled = false;
        playerAttackBar.gameObject.GetComponentsInChildren<Image>()[1].enabled = false;
    }

    void LateUpdate()
    {
        CalculateMovementVariables();

        CheckPlayerOutOfBounds();

        if (Input.GetMouseButtonDown(0) && CanAttack && !FindAnyObjectByType<CardManager>().ChoosingCard)
        {
            StartCoroutine(AttackCoroutine());
        }

        if (Input.GetMouseButtonDown(1))
        {
            UseRune();
        }

        if (PlayerHitThisFrame && !Invulnerable) PlayerHit();

        SetAnimationVars();
        //LogPlayerStats();
        ResetTempVariables();
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = currentVelocity * currentDirection;
    }

    private void SetAnimationVars()
    {
        myAnimator.SetFloat("velocity", currentVelocity);
        myAnimator.SetBool("invulnerable", Invulnerable && !Dashing);
    }

    private void ResetTempVariables()
    {
        TempMoveSpeed = basePlayerSpeed;
        TempAttackCooldown = baseAttackCooldown;
        Invulnerable = false;
        PlayerHitThisFrame = false;
    }

    private void LogPlayerStats()
    {
        Debug.Log("Movement Speed: " + TempMoveSpeed);
        Debug.Log("Attack Cooldown: " + TempAttackCooldown);
    }

    private void UseRune()
    {
        if (Rune == null || !CanUseRune) return;
        switch (Rune.runeID)
        {
            case CardManager.RuneID.Time:
                FindAnyObjectByType<TimeManager>().ChangeSceneTime(.25f, 3); // Slow time for 3 seconds
                break;
            case CardManager.RuneID.War:
                StartCoroutine(DashCoroutine(baseDashTime));
                break;
            case CardManager.RuneID.Death:
                foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                {
                    AttackEnemyWithVine(enemy);
                }
                break;
            case CardManager.RuneID.Life:
                healthSystem.Heal(1);
                break;
        }
        currentRuneCoroutine = StartCoroutine(RuneCooldown(Rune.runeCooldown * baseRuneCooldownScalar));
        FindAnyObjectByType<SoundController>().PlaySound(Rune.sound, volume: .5f);
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

        //turn on cooldown UI
        Image glow = playerAttackBar.gameObject.GetComponentsInChildren<Image>()[1];
        Image container = playerAttackBar.gameObject.GetComponentsInChildren<Image>()[0];
        glow.enabled = true;
        container.enabled = true;
        glow.fillAmount = 1;

        float timePassed = 0;
        while (timePassed < TempAttackCooldown)
        {
            //lerp cooldown image
            float valueToLerp = Mathf.Lerp(1, 0, timePassed / TempAttackCooldown);
            glow.fillAmount = valueToLerp;
            timePassed += Time.deltaTime;
            yield return null;
        }
        glow.enabled = false;
        container.enabled = false;

        CanAttack = true;
    }

    private void CalculateMovementVariables()
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

        //transform.Translate(currentVelocity * Time.deltaTime * currentDirection);
    }

    private float GetCurrentPlayerSpeed()
    {
        return TempMoveSpeed;
    }

    public void AttackEnemyWithVine(Enemy enemy)
    {
        Instantiate(vine, enemy.transform.position, Quaternion.identity);
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
        currentRuneCoroutine = StartCoroutine(RuneCooldown(1));
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
            Invulnerable = true;
            duration -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RuneCooldown(float duration)
    {
        if (currentRuneCoroutine != null) StopCoroutine(currentRuneCoroutine);
        CanUseRune = false;

        Image cooldownImg = runeDisplay.GetComponentsInChildren<Image>()[0];

        //update sprite to be dull sprite
        runeDisplay.GetComponentsInChildren<Image>()[2].sprite = runeDisplay.runeCard.runeImage;
        cooldownImg.fillAmount = 0;
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            //lerp cooldown image
            float valueToLerp = Mathf.Lerp(0, 1, timeElapsed / duration);
            cooldownImg.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        cooldownImg.fillAmount = 1;
        //update sprite to be glow sprite
        runeDisplay.GetComponentsInChildren<Image>()[2].sprite = runeDisplay.runeCard.runeGlowImage;
        CanUseRune = true;
        currentRuneCoroutine = null;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !Invulnerable && other.isTrigger && other.GetComponent<Enemy>().canDamage)
        {
            Debug.Log("Took Player Damage!");
            PlayerHitThisFrame = true;
        }
    }
}
