using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// each little XP object will be in charge of itself, and will be released by an enemy when they are killed
/// </summary>
public class XP : MonoBehaviour
{
    public int xp;
    private Vector2 xpBarTransform = new Vector2(0,0);

    // Minimum and maximum values for the transition.
    float minimum = 10.0f;
    float maximum = 20.0f;

    // Time taken for the transition.
    float duration = 5.0f;

    float startTime;

    void Start()
    {
        // Make a note of the time the script started.
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate the fraction of the total duration that has passed.
        float t = (Time.time - startTime) / duration;
        transform.position = new Vector3(Mathf.SmoothStep(xpBarTransform.x,xpBarTransform.y, t), 0, 0);
    }

}
