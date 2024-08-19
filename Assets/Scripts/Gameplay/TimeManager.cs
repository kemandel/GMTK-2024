using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float TimeScaleGoal { get; private set; }
    private float defaultTime = 1f;

    public float timeChangeDuration = .25f;
    private float currentTransitionTime = 0;

    private float oldTimeScale;

    void Awake()
    {
        oldTimeScale = TimeScaleGoal = 1;
    }

    void LateUpdate()
    {
        if (oldTimeScale != TimeScaleGoal)
        {
            float t = Mathf.Clamp01(currentTransitionTime / timeChangeDuration);
            float currentTimeScale = Mathf.Lerp(oldTimeScale, TimeScaleGoal, t);
            Time.timeScale = currentTimeScale;
            if (currentTransitionTime < timeChangeDuration) currentTransitionTime += Time.deltaTime / Time.timeScale; // Independent of timeScale
            else
            {
                oldTimeScale = TimeScaleGoal;
                currentTransitionTime = 0;
            }
        }

        TimeScaleGoal = 1; // Default time
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
