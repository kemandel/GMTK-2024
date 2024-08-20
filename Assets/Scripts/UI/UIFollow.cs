using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    private Transform playerTransform;
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().gameObject.transform;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = mainCam.WorldToScreenPoint(playerTransform.position);
        transform.position = new Vector3(screenPos.x, screenPos.y + 125, screenPos.z);
    }
}
