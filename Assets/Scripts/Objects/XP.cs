using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// each little XP object will be in charge of itself, and will be released by an enemy when they are killed
/// </summary>
public class XP : MonoBehaviour
{
    public int xp;
    private Vector3 target;
    private Vector3 spawnPos;

    // Time taken for the transition.
    public float duration;

    float startTime;

    private XPBar xpBar;

    void Start()
    {
        // Make a note of the time the script started.
        startTime = Time.time;
        xpBar = FindAnyObjectByType<XPBar>();//get component rect transform
        target = Camera.main.ScreenToWorldPoint(xpBar.transform.position);
        spawnPos = transform.position;
    }

    void Update()
    {
        // Calculate the fraction of the total duration that has passed.
        float t = (Time.time - startTime) / duration;

        Vector3 newPosition = Vector3.Lerp(spawnPos, target, Mathf.SmoothStep(0, 1, t));
        transform.position = new Vector3(newPosition.x, newPosition.y, 0);

        if (Vector2.Distance(transform.position, target) < .01f)
        {
            xpBar.GainXP(xp);
            Destroy(gameObject);
        }
    }

}
