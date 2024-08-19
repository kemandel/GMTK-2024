using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private Vector2 START_POINT = new Vector2(0, 0);
    private bool quitGame;
    public Canvas quitCanvas;
    private const string MAIN_SCENE = "MainScene";
    private const string MAIN_MENU = "MenuScene";

    /// <summary>
    /// player retries game, triggered by retry button in-game
    /// </summary>
    public void RefreshGame()
    {
        SceneManager.LoadScene(MAIN_SCENE);
        // Max - Maybe just reload scene?
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

    public IEnumerator ReturnToMainCoroutine()
    {
        //play fade out animation and soiunds
        yield return null;
        SceneManager.LoadScene(MAIN_MENU);
    }
}
