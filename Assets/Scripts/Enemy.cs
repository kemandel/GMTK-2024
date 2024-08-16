using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float attackRange = 1;
    public float moveSpeed = 2;

    private Transform player;
    private Animator myAnimator;
    private bool attacking;
    private bool spawning;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        myAnimator = GetComponentInChildren<Animator>();
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking || spawning) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * direction);
    }

    private IEnumerator AttackCoroutine()
    {
        attacking = true;
        myAnimator.SetTrigger("attack");
        yield return null;
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        attacking = false;
    }

    private IEnumerator SpawnCoroutine()
    {
        spawning = true;
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        spawning = false;
    }
}
