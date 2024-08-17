using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private const string SCENE_TO_LOAD = "MainScene";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlay()
    {
        StartCoroutine(ClickPlayCoroutine());
    }

    private IEnumerator ClickPlayCoroutine()
    {
        //do fade animation for music and screen
        yield return null;
        SceneManager.LoadScene(SCENE_TO_LOAD);
    }
}
