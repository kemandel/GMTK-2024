using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float TimeScaleGoal { get; private set; }
    public float defaultTime = 1f;

    public float timeChangeDuration = .25f;
    private float currentTransitionTime = 0;

    private float oldTimeScale;

    void Awake()
    {
        Time.timeScale = oldTimeScale = TimeScaleGoal = defaultTime;
    }

    void LateUpdate()
    {
        if (Time.timeScale != TimeScaleGoal)
        {
            float t = Mathf.Clamp01(currentTransitionTime / timeChangeDuration);
            float currentTimeScale = Mathf.Lerp(oldTimeScale, TimeScaleGoal, t);
            if (currentTimeScale is float.NaN) currentTimeScale = 0;
            Time.timeScale = currentTimeScale;
            if (currentTransitionTime < timeChangeDuration) currentTransitionTime += Time.deltaTime / Time.timeScale; // Independent of timeScale
            else
            {
                oldTimeScale = TimeScaleGoal;
                currentTransitionTime = 0;
            }
        }

        // Audio synced to time
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) audioSource.pitch = Time.timeScale;

        TimeScaleGoal = 1; // Default time
        //Debug.Log("Time Scale: " + Time.timeScale);
        //Debug.Log("Old Time Scale: " + oldTimeScale);
    }
    
    public Coroutine ChangeSceneTime(float timeScale, float duration = float.PositiveInfinity)
    {
        return StartCoroutine(MinimumTimeScaleCoroutine(timeScale, duration));
    }

    private IEnumerator MinimumTimeScaleCoroutine(float timeScale, float duration)
    {
        while (duration > 0)
        {
            if (TimeScaleGoal > timeScale) TimeScaleGoal = timeScale;
            yield return null;
            duration -= Time.deltaTime / Time.timeScale; // Independent of timeScale
        }
    }
}
