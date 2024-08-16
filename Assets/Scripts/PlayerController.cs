using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float basePlayerSpeed;
    [SerializeField]
    private float basePlayerAcceleration;
    [SerializeField]
    private float decelerationScalar = 1;

    private float currentVelocity;
    private Vector2 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0;
        currentDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        /// Player Movement
        MovePlayer();
    }

    private void MovePlayer()
    {
        /// Player Movement
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float playerSpeed = GetCurrentPlayerSpeed();

        if (direction != Vector2.zero)
        {
            currentDirection = direction;
            currentVelocity += basePlayerAcceleration * Time.deltaTime;
        }
        else currentVelocity -= basePlayerAcceleration * decelerationScalar * Time.deltaTime;

        if (currentVelocity > playerSpeed) currentVelocity = playerSpeed;
        if (currentVelocity < 0) currentVelocity = 0;

        if (currentVelocity != 0) Debug.Log("Current Speed: " + currentVelocity); 
        transform.Translate(currentVelocity * Time.deltaTime * currentDirection);
    }

    private float GetCurrentPlayerSpeed()
    {
        return basePlayerSpeed;
    }
}
