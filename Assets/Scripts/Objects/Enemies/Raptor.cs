using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Enemy
{
    private Vector2 direction;

    // Update is called once per frame
    public override void Update()
    {
        // Skip movement during anims
        if (spawning || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < attackRange && !attacking)
        {
            StartCoroutine(AttackCoroutine());
        }
        Move();
    }

    public override void Move()
    {
        if (!attacking) direction = (player.transform.position - transform.position).normalized;
        if (!attacking) mySpriteRenderer.flipX = direction.x > 0;
        transform.Translate((attacking ? 2f : 1) * moveSpeed * Time.deltaTime * direction);
    }
}
