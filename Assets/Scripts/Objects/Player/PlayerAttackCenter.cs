using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAttackCenter : MonoBehaviour
{
    private Transform playerTransform;
    public float distanceFromPlayer = 1;
    
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTransform.GetComponent<PlayerController>().CanAttack) return;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
        transform.position = (Vector2)playerTransform.position + direction.normalized * distanceFromPlayer; //new Vector2(direction.normalized.x, direction.normalized.y / 2);

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;
        transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage();
        }
    }
}
