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
    private List<Coroutine> timeScaleCoroutines;

    void Awake()
    {
        Time.timeScale = oldTimeScale = TimeScaleGoal = defaultTime;
        timeScaleCoroutines = new List<Coroutine>();
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0 && Time.timeScale != TimeScaleGoal) oldTimeScale = .001f;
        if (Time.timeScale != TimeScaleGoal)
        {
            float t = Mathf.Clamp01(currentTransitionTime / timeChangeDuration);
            float currentTimeScale = Mathf.Lerp(oldTimeScale, TimeScaleGoal, t);
            if (currentTimeScale is float.NaN) currentTimeScale = 0;
            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            if (currentTransitionTime < timeChangeDuration) currentTransitionTime += Time.deltaTime / (Time.timeScale > 0 ? Time.timeScale : .001f); // Independent of timeScale
            else
            {
                oldTimeScale = TimeScaleGoal;
                currentTransitionTime = 0;
            }
        }

        // Audio synced to time
        // foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) audioSource.pitch = Time.timeScale;
        TimeScaleGoal = 1; // Default time
    }

    public void StopAllEffects()
    {
        foreach (Coroutine coroutine in timeScaleCoroutines)
        {
            StopCoroutine(coroutine);
        }
        timeScaleCoroutines = new List<Coroutine>();
    }
    
    public Coroutine ChangeSceneTime(float timeScale, float duration = float.PositiveInfinity)
    {
        timeScaleCoroutines.Add(StartCoroutine(MinimumTimeScaleCoroutine(timeScale, duration)));
        return timeScaleCoroutines[^1];
    }

    private IEnumerator MinimumTimeScaleCoroutine(float timeScale, float duration)
    {
        while (duration > 0)
        {
            if (TimeScaleGoal > timeScale) TimeScaleGoal = timeScale;
            yield return null;
            duration -= Time.deltaTime / (Time.timeScale > 0 ? Time.timeScale : .001f); // Independent of timeScale
        }
    }
}
