using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private Vector2 START_POINT = new Vector2(0, 0);
    private bool quitGame;
    public Canvas quitCanvas;

    /// <summary>
    /// player retries game, triggered by retry button in-game
    /// </summary>
    public void RefreshGame()
    {
        FindAnyObjectByType<HealthSystem>().RefreshHealth();
        //FindAnyObjectByType<PlayerController>().gameObject.transform = START_POINT;
        //call restart music function
        //call restart camera function
        //call restart enemy spawn function
        //call restart player stats function
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        quitCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!quitGame && Input.GetKeyDown(KeyCode.Escape))
        {
            quitGame = true;
            quitCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// triggered via quit button in-game
    /// </summary>
    public void OnClickQuit()
    {
        StartCoroutine(QuitGameCoroutine());
    }

    public IEnumerator QuitGameCoroutine()
    {
        //play fade out animation and soiunds
        yield return null;
        Application.Quit();
    }
}
