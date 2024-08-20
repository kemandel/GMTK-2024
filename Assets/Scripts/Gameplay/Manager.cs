using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public float nextEnemySpawnerDelay = 30;
    public EnemyWave[] finalEncounter;
    private bool quitGame;
    public Canvas quitCanvas;
    private const string MAIN_SCENE = "MainScene";
    private const string MAIN_MENU = "MainMenu";
    public Canvas winCanvas;
    public AudioSource musicController;
    public AudioClip winSound;

    public float gameStartDelay = 3;

    public Canvas fadeCanvas;

    private EnemySpawner[] enemySpawners;

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
        fadeCanvas.GetComponent<Animator>().SetTrigger("fade");
        yield return null;
        yield return new WaitForSeconds(fadeCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(MAIN_SCENE);
    }
    // Start is called before the first frame update
    void Start()
    {
        quitCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
        enemySpawners = GetComponents<EnemySpawner>();
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
        
        FindAnyObjectByType<CameraManager>().StartCameraZoom();
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.StartSpawning();
            yield return new WaitForSeconds(nextEnemySpawnerDelay);
        }

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.StopSpawning();
        }

        while (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length > 0) yield return null;
        yield return new WaitForSeconds(3);

        foreach (EnemyWave wave in finalEncounter)
        {
            for (int i = 0; i < wave.count; i++)
            {
                Vector2 extents = CameraManager.EdgeExtents;
                Vector2 spawnLocation = new Vector2(Random.Range(-extents.x + .5f, extents.x - .5f), Random.Range(-extents.y + .5f, extents.y - .5f));
                PlayerController player = FindAnyObjectByType<PlayerController>();
                Vector2 playerLocation = player != null ? FindAnyObjectByType<PlayerController>().transform.position : Vector2.zero;
                float minimumDistanceFromPlayer = extents.x * 2 / 4; // Spawn at least 1/4 of the arena from the player

                int spawnAttempts = 0;
                while ((Vector2.Distance(spawnLocation, playerLocation) < minimumDistanceFromPlayer) && spawnAttempts < 10)
                {
                    spawnLocation = new Vector2(Random.Range(-extents.x, extents.x), Random.Range(-extents.y, extents.y));
                    spawnAttempts++;
                }
                if (spawnAttempts < 10) Instantiate(wave.enemy, spawnLocation, Quaternion.identity);
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    public IEnumerator QuitGameCoroutine()
    {
        //play fade out animation and sounds
        FindAnyObjectByType<TimeManager>().StopAllEffects();
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

    public IEnumerator WinCoroutine()
    {
        winCanvas.gameObject.SetActive(true);

        //trigger animation of player dying
        FindAnyObjectByType<TimeManager>().ChangeSceneTime(0);
        yield return null;

        musicController.clip = winSound;
        musicController.Play();
    }

    public void OnClickResume()
    {
        quitGame = false;
        quitCanvas.gameObject.SetActive(false);
    }
    

    [System.Serializable]
    public struct EnemyWave
    {
        public Enemy enemy;
        public int count;
    }
}
