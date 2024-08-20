using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public int[] requiredXpTiers;

    public Image xpBar;
    //how much XP player has currently
    private float playerXP = 0;

    //how much XP player needs until they can power up
    private float maxXP; //increase according to the level/teir  the player is at

    private float currentXP;

    private float timeToFill = 1;
    private float currentTime = 0;
    private float xpFillAmount = 0;

    private CardManager cardManager;

    // Start is called before the first frame update
    void Start()
    {
        xpBar.fillAmount = 0;
        cardManager = FindAnyObjectByType<CardManager>();
        maxXP = requiredXpTiers[0];
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
        if (cardManager == null) return;
        int playerLevel = cardManager.PlayerLevel;
        if (playerLevel < requiredXpTiers.Length && xpFillAmount >= maxXP)
        {
            cardManager.IncreasePlayerLevel();
            ResetXP();
        }
    }

    public void GainXP(float xpToAdd)
    {
        currentTime = 0;

        playerXP += xpToAdd;
        currentXP = xpFillAmount;
    }

    public void ResetXP()
    {
        // reset xp
        playerXP = 0;
        currentXP = 0;
        xpFillAmount = 0;
        maxXP = cardManager.PlayerLevel > requiredXpTiers.Length - 1 ? requiredXpTiers[^1] : requiredXpTiers[cardManager.PlayerLevel];
    }

}
