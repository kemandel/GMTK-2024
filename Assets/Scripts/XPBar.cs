using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Image xpBar;
    //how much XP player has currently
    public float playerXP = 0;

    //how much XP player needs until they can power up
    private float maxXP = 50; //increase according to the level/teir  the player is at

  
    public IEnumerator GainXPCoroutine(float xpToAdd)
    {
        float target = xpToAdd + playerXP;
        float duration = 10f;
        float time = 0;
        float tempAmnt = playerXP;
        playerXP = target;
        while (xpBar.fillAmount < tempAmnt+xpToAdd/maxXP) 
        {
            time += Time.deltaTime;
            float percent = time / duration;
            xpBar.fillAmount = tempAmnt + xpToAdd * percent;
            yield return null;
        }
        if (playerXP >= maxXP) PowerBoost();
        yield return null;
    }

    /// <summary>
    /// when player gets enough XP to increase their power
    /// should vary depending on what stage of the game the pllayer is in
    /// </summary>
    private void PowerBoost()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        xpBar.fillAmount = 0;
        //StartCoroutine(GainXPCoroutine(50));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
