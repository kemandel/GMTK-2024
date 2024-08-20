using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private bool quitGame;
    public Canvas quitCanvas;
    private const string MAIN_SCENE = "MainScene";
    private const string MAIN_MENU = "MainMenu";

    public float gameStartDelay = 3;

    public Canvas fadeCanvas;

    /// <summary>
    /// player retries game, triggered by retry button in-game
    /// </summary>
    public void RefreshGame()
    {
        StartCoroutine(RefreshGameCoroutine());
        //FindAnyObjectByType<TimeManager>().StopAllCoroutines();
    }

    private IEnumerator RefreshGameCoroutine()
    {
        Debug.Log("Refresh Game");
        FindAnyObjectByType<TimeManager>().StopAllEffects();
        yield return null;
        //play fade out animation and soiunds
        fadeCanvas.GetComponent<Animator>().SetTrigger("unfade");
        yield return null;
        yield return new WaitForSeconds(fadeCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(MAIN_SCENE);
    }
    // Start is called before the first frame update
    void Start()
    {
        quitCanvas.gameObject.SetActive(false);
        StartCoroutine(GameplayCoroutine());
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

    public IEnumerator GameplayCoroutine()
    {
        fadeCanvas.GetComponent<Animator>().SetTrigger("unfade");
        yield return null;
        yield return new WaitForSeconds(fadeCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        while (gameStartDelay > 0)
        {
            gameStartDelay -= Time.deltaTime;
            yield return null;
        }

        FindAnyObjectByType<EnemySpawner>().StartSpawning();
        FindAnyObjectByType<CameraManager>().StartCameraZoom();
    }

    public IEnumerator QuitGameCoroutine()
    {
        //play fade out animation and soiunds
        fadeCanvas.GetComponent<Animator>().SetTrigger("fade");
        yield return null;
        yield return new WaitForSeconds(fadeCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(MAIN_MENU);
    }

    public IEnumerator ReturnToMainCoroutine()
    {
        //play fade out animation and sounds
        yield return null;
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void OnClickResume()
    {
        quitGame = false;
        quitCanvas.gameObject.SetActive(false);
    }
}
