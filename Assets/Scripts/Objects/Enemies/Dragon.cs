using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public override void Move()
    {
        direction = (player.transform.position + new Vector3(horizontalOffset * (direction.x > 0 ? -1 : 1),0,0) - transform.position).normalized;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * ((player.transform.position - transform.position).x > 0 ? -1 : 1), transform.localScale.y, transform.localScale.x);
        transform.Translate((attacking ? 2f : 1) * moveSpeed * Time.deltaTime * direction);
    }
}
