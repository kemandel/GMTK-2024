using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image[] healthImages;
    public int health = 3;
    public Canvas retryCanvas;
    public AudioClip gameOverSound;

    public AudioSource musicController;
    // Start is called before the first frame update
    void Start()
    {
        retryCanvas.gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        //add sound effect for damage
        //add animation for damage by triggering animation on object healthImages[healthImages.length-1]
        healthImages[health - 1].enabled = false; //disable any hearts that are damaged after animation
        health -= 1;

        if (health <= 0) //potentially turn this into an event
        {
            StartCoroutine(DeadCoroutine());
        }
    }

    
    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > 3) health = 3;

        for (int i = 0; i < health; i++)
        {
            healthImages[i].enabled = true;
        }
    }

    private IEnumerator DeadCoroutine()
    {
        retryCanvas.gameObject.SetActive(true);

        //trigger animation of player dying
        FindAnyObjectByType<TimeManager>().ChangeSceneTime(0);
        yield return null;

        musicController.clip = gameOverSound;
        musicController.Play();
        
        gameObject.SetActive(false);
    }

}
