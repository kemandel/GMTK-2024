using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float startingScale = 3;
    public float endingScale = 15;
    public float zoomTime = 60;

    private float currentTime;

    public static Vector2 EdgeExtents { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        currentTime = 0;
        transform.localScale = new Vector3(startingScale, startingScale, 1);
        UpdateEdgeExtents();
    }

    public void StartCameraZoom()
    {
        currentTime = 0;
        StartCoroutine(CameraZoomCoroutine());
    }

    private IEnumerator CameraZoomCoroutine()
    {
        while (currentTime <= zoomTime)
        {
            float ratio = Mathf.Pow(Mathf.Clamp01(currentTime / zoomTime), 1.5f);
            float newScale = Mathf.Lerp(startingScale, endingScale, ratio);

            transform.localScale = new Vector3(newScale, newScale, 1);
            UpdateEdgeExtents();
            yield return null;

            currentTime += Time.deltaTime;
        }
        transform.localScale = new Vector3(endingScale, endingScale, 1);
    }

    private void UpdateEdgeExtents()
    {
        float aspectRatio = Screen.width / ((float)Screen.height);
        EdgeExtents = new Vector2(Camera.main.orthographicSize * aspectRatio, Camera.main.orthographicSize) * transform.localScale.x;
    }
}
