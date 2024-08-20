using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private const string SCENE_TO_LOAD = "MainScene";
    public Canvas fadeCanvas;
    public Animator audioAnim;


    public Animator caveAnim;
    public Animator playAnim;
    public Animator tutorialAnim;
    public Animator creditsAnim;

    // Start is called before the first frame update
    void Start()
    {
        fadeCanvas.GetComponent<Animator>().SetTrigger("unfade");
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
        tutorialAnim.gameObject.GetComponent<Button>().enabled = false;
        playAnim.gameObject.GetComponent<Button>().enabled = false;
        creditsAnim.gameObject.GetComponent<Button>().enabled = false;
        playAnim.SetTrigger("fadeToTransparent");
        tutorialAnim.SetTrigger("fadeToTransparent");
        creditsAnim.SetTrigger("fadeToTransparent");
        yield return new WaitForSeconds(1.5f);
        caveAnim.SetTrigger("crumble");
        audioAnim.SetTrigger("audioFadeOut");
        yield return new WaitForSeconds(3);
        //fadeCanvas.GetComponent<Animator>().SetTrigger("fade");
        //yield return null;
       // yield return new WaitForSeconds(caveAnim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        //yield return new WaitForSeconds(fadeCanvas.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SCENE_TO_LOAD);
    }
}
