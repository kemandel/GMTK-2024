using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Dragon : Enemy
{
    Vector2 direction = Vector2.zero;
    public float horizontalOffset = 3;
    public float verticalOffset = 1;

    // Update is called once per frame
    public override void Update()
    {
        // Skip movement during anims
        if (attacking || spawning || player == null) return;

        float distanceToTarget = Vector3.Distance(transform.position + new Vector3(horizontalOffset * (direction.x > 0 ? -1 : 1),0,0), player.transform.position);
        if (distanceToTarget < attackRange && !attacking)
        {
            StartCoroutine(AttackCoroutine());
        }
        else Move();

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * ((player.transform.position - transform.position).x > 0 ? -1 : 1), transform.localScale.y, transform.localScale.x);
    }

    public override IEnumerator CheckHealth()
    {
        StartCoroutine(FlashWhite());
        // Unit has been killed
        if (health <= 0)
        {
            canDamage = false;
            yield return new WaitForSeconds(.1f);
            if (player != null) player.TriggerEnemyDefeatedEvent();
            for (int i = 0; i < xpAmount; i++)
                Instantiate(xpReference, transform.position + new Vector3(Random.Range(-.5f,.5f), Random.Range(-.5f,.5f), 0), Quaternion.identity);
            yield return FindAnyObjectByType<Manager>().WinCoroutine();
            Destroy(gameObject);
        }
    }

    public override void Move()
    {
        direction = (player.transform.position + new Vector3(horizontalOffset * (direction.x > 0 ? -1 : 1),0,0) - transform.position).normalized;
        transform.Translate((attacking ? 2f : 1) * moveSpeed * Time.deltaTime * direction);
    }
}
