using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class CustomCursor : MonoBehaviour
{
    public Sprite cursorImage;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        playerTransform = FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

        Vector2 direction = playerTransform.position - transform.position;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * direction;
        transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    }
}
