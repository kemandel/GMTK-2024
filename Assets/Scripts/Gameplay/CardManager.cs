using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public enum CardID { HealToFull = 0, SpeedUp = 1 }

    private PowerUpCard[] powerUps1 = Resources.LoadAll<PowerUpCard>("PowerUps/Tier1");
    private PowerUpCard[] powerUps2 = Resources.LoadAll<PowerUpCard>("PowerUps/Tier2");
    private PowerUpCard[] powerUps3 = Resources.LoadAll<PowerUpCard>("PowerUps/Tier3");

    public int PlayerLevel { get; private set; }

    private CardDisplay[] cardDisplays;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLevel = 0;
        cardDisplays = FindObjectsByType<CardDisplay>(FindObjectsSortMode.None);
        foreach(CardDisplay card in cardDisplays)
        {
            card.gameObject.SetActive(false);
        }
    }

    public void IncreasePlayerLevel()
    {
        //slow time down
        Time.timeScale = 0.25f;
        PlayerLevel++;

        //reset cards to new power-up options
        switch (PlayerLevel)
        {
            case 1:
                // tier 1 selection of cards
                List<PowerUpCard> lst = new List<PowerUpCard>(powerUps1);
                for (int i  = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, lst.Count);
                    PowerUpCard chosenCard = lst[randomIndex];
                    cardDisplays[i].UpdateCard(chosenCard);
                    //lst.RemoveAt(randomIndex);
                }
                break;
            case 2:
                // tier 2 selection of cards
                break;
            case 3:
                // tier 3 selection of cards
                break;
            default:
                // code block
                break;
        }

        //enable card power ups
        foreach (CardDisplay card in cardDisplays)
        {
            card.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// by clicking on a card you can get that power up
    /// will be called by a button
    /// </summary>
    public void OnClickCard(int cardChoice)
    {
        //check unity events, looping through the cardDisplays, until the currently selected game objects == the cardDisplay[i], and extract that power, pass it to ApplyPowerUp
        // ApplyPowerUp(); //adjust for scriptable objects so we are passing the power that player selects to be applied

        Time.timeScale = 1;
        //enable card power ups
        foreach (CardDisplay card in cardDisplays)
        {
            card.gameObject.SetActive(false);
        }
    }

    public void ApplyPowerUp(PowerUpCard card)
    {
        switch (card.cardID)
        {
            case CardID.HealToFull:
                FindAnyObjectByType<HealthSystem>().RefreshHealth();
                break;
            case CardID.SpeedUp:
                FindAnyObjectByType<PlayerController>().AddToMoveSpeedScalar(card.cardParameter);
                break;
        }
    }
}
