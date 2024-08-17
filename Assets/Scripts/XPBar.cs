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

    public float currentXP;

    private float timeToFill = 1;
    private float currentTime = 0;
    private float xpFillAmount = 0;
  
    public void GainXP(float xpToAdd)
    {
        currentTime = 0;

        playerXP += xpToAdd;
        currentXP = xpFillAmount;
    }

    // Update is called once per frame
    void Update()
    {
       if (playerXP > 0 && currentXP <= playerXP)
       {

            float t = Mathf.Clamp01(currentTime / timeToFill);
            xpFillAmount = Mathf.Lerp(currentXP, playerXP, t);
            xpBar.fillAmount = xpFillAmount / maxXP;
            if (currentTime < timeToFill) currentTime += Time.deltaTime;
            else currentXP = playerXP;
       }

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

}
