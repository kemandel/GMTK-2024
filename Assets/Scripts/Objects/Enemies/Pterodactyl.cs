using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pterodactyl : Enemy
{
    public float verticalOffset = 4;

    // Update is called once per frame
    public override void Update()
    {
        // Skip movement during anims
        if (spawning || player == null) return;

        if (!attacking)
        {
            float distanceToGoal = Vector3.Distance(transform.position, player.transform.position + new Vector3(0, verticalOffset, 0));
            if (distanceToGoal < attackRange)
            {
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                Move();
            }
        }
        else
        {
            Move();
        }
    }

    public override void Move()
    {
        Vector2 direction = (player.transform.position + new Vector3(0, verticalOffset * (attacking ? -.5f : 1), 0) - transform.position).normalized;
        if (!attacking) mySpriteRenderer.flipX = direction.x > 0;
        transform.Translate((attacking ? 2 : 1) * moveSpeed * Time.deltaTime * direction);
    }
}
