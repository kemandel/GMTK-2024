using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float attackRange = 1;
    public float moveSpeed = 2;
    public int health = 1;

    public XP xpReference;
    public Material flashWhiteMat;

    [DoNotSerialize]
    public PlayerController player;
    [DoNotSerialize]
    public Animator myAnimator;
    [DoNotSerialize]
    public SpriteRenderer mySpriteRenderer;
    [DoNotSerialize]
    public bool attacking;
    [DoNotSerialize]
    public bool spawning;

    void Start()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        this.player = player;
        myAnimator = GetComponentInChildren<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Skip movement during anims
        if (attacking || spawning || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < attackRange)
        {
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            Move();
        }
    }

    public void TakeDamage()
    {
        health--;
        StartCoroutine(CheckHealth());
    }

    private IEnumerator CheckHealth()
    {
        yield return FlashWhite();
        // Unit has been killed
        if (health <= 0)
        {
            if (player != null) player.TriggerEnemyDefeatedEvent();
            Instantiate(xpReference, transform.position + new Vector3(Random.Range(-.5f,.5f), Random.Range(-.5f,.5f), 0), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        mySpriteRenderer.flipX = direction.x > 0;
        transform.Translate(moveSpeed * Time.deltaTime * direction);
    }

    public virtual IEnumerator AttackCoroutine()
    {
        attacking = true;
        myAnimator.SetTrigger("attack");
        yield return null;
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        attacking = false;
    }

    private IEnumerator FlashWhite()
    {
        Material originalMaterial = mySpriteRenderer.material;
        mySpriteRenderer.material = flashWhiteMat;
        yield return new WaitForSeconds(.2f);
        mySpriteRenderer.material = originalMaterial;
    }

    private IEnumerator SpawnCoroutine()
    {
        spawning = true;
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        yield return StartCoroutine(FlashWhite());
        spawning = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<PlayerController>().Invulnerable)
        {
            other.GetComponent<PlayerController>().PlayerHitThisFrame = true;
        }
    }
}
