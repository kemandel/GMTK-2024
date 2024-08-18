using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteLayerer : MonoBehaviour
{
    private int baseLayer;
    private SpriteRenderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        baseLayer = myRenderer.sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.sortingOrder = baseLayer + (1000 - (int)(transform.position.y*10));
    }
}
