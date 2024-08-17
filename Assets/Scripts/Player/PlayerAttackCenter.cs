using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAttackCenter : MonoBehaviour
{
    private Transform playerTransform;
    
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
        transform.position = (Vector2)playerTransform.position + direction.normalized;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * direction;
        transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }
    }
}
