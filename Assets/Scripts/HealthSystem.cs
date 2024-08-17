using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image[] healthImages;
    public static int health = 3;
    public Canvas retryCanvas;
    // Start is called before the first frame update
    void Start()
    {
        retryCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator DamageTaken()
    {
        //add sound effect for damage
        //add animation for damage by triggering animation on object healthImages[healthImages.length-1]
        yield return null;
        healthImages[healthImages.Length - 1].enabled = false; //disable any hearts that are damaged after animation
        health -= 1;

        if (health <= 0) //potentially turn this into an event
        {
            StartCoroutine(DeadCoroutine());
        }
    }

    
    public void RefreshHealth()
    {
        health = 3;
        foreach(Image img in healthImages)
        {
            img.enabled = true;
        }
    }
    private IEnumerator DeadCoroutine()
    {
        //call fade out music function
        //enemies freeze
        //trigger animation of player dying
        Time.timeScale = 0; //maybe just slow down instead of freezing
        yield return null;
        retryCanvas.gameObject.SetActive(true);
    }

}
