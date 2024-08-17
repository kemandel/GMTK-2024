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

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0;
        currentDirection = Vector2.zero;
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        CheckPlayerOutOfBounds();

        if (Input.GetMouseButtonDown(0))
        {
            myAnimator.SetTrigger("attack");
        }
    }

    private void CheckPlayerOutOfBounds()
    {
        Vector2 boundsExtents = CameraManager.EdgeExtents;
        Vector2 playerExtents = GetComponentInChildren<BoxCollider2D>().bounds.extents;

        if (transform.position.x - playerExtents.x < -boundsExtents.x) transform.position = new Vector2(-boundsExtents.x + playerExtents.x, transform.position.y);
        else if (transform.position.x + playerExtents.x > boundsExtents.x) transform.position = new Vector2(boundsExtents.x - playerExtents.x, transform.position.y);

        if (transform.position.y - playerExtents.y < -boundsExtents.y) transform.position = new Vector2(transform.position.x, -boundsExtents.y + playerExtents.y);
        else if (transform.position.y + playerExtents.y > boundsExtents.y) transform.position = new Vector2(transform.position.x, boundsExtents.y - playerExtents.y);
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
        
        transform.Translate(currentVelocity * Time.deltaTime * currentDirection);
    }

    private float GetCurrentPlayerSpeed()
    {
        return basePlayerSpeed;
    }
}
