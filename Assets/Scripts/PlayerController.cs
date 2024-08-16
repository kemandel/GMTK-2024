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

    private float currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /// Player Movement
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float playerSpeed = GetCurrentPlayerSpeed();

        if (direction != Vector2.zero) currentVelocity += basePlayerAcceleration * Time.deltaTime;
        else currentVelocity -= basePlayerAcceleration * Time.deltaTime;

        if (currentVelocity > playerSpeed) currentVelocity = playerSpeed;
        if (currentVelocity < 0) currentVelocity = 0;

        if (direction != Vector2.zero) Debug.Log("Current Speed: " + currentVelocity); 
        transform.Translate(currentVelocity * Time.deltaTime * direction);

        
    }

    private float GetCurrentPlayerSpeed()
    {
        return basePlayerSpeed;
    }
}
