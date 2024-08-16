using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Image xpBar;
    //how much XP player has currently
    public static float playerXP;

    //how much XP player needs until they can power up
    public float maxXP = 50; //increase according to the level/teir  the player is at

  
    public IEnumerator GainXPCoroutine(float xpToAdd)
    {
        float target = xpToAdd + playerXP;
        float duration = 0.2f;
        float time = 0;
        float tempAmnt = playerXP;
        playerXP = target;
        while (time < duration)
        {
            time += Time.deltaTime;
            float percent = time / duration;
            xpBar.fillAmount = tempAmnt + xpToAdd * percent;
            yield return null;
        }
        if (playerXP >= maxXP) PowerBoost();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
