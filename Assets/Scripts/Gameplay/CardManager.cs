using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public enum CardID { HealToFull = 0, SpeedUp = 1 }

    private GameObject[] powerUps1 = Resources.LoadAll<GameObject>("PowerUps/Tier1"); //change to type of scriptable pbjects later
    private GameObject[] powerUps2 = Resources.LoadAll<GameObject>("PowerUps/Tier2");
    private GameObject[] powerUps3 = Resources.LoadAll<GameObject>("PowerUps/Tier3");

    public int playerLevel = 0;

    private CardDisplay[] cardDisplays;
    // Start is called before the first frame update
    void Start()
    {
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
        playerLevel++;

        //reset cards to new power-up options
        switch (playerLevel)
        {
            case 1:
                // teir 1 selection of cards
                List<GameObject> lst = new List<GameObject>(powerUps1);
                for (int i  = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, lst.Count);
                    GameObject chosenCard = lst[randomIndex];
                    //cardDisplays[i].UpdateCard(chosenCard.title, chosenCard.desc, chosenCard.art); fill in once scriptable object exists
                    lst.RemoveAt(randomIndex);
                }
                break;
            case 2:
                // teir 2 selection of cards
                break;
            case 3:
                // teir 3 selection of cards
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
    public void OnClickCard()
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
