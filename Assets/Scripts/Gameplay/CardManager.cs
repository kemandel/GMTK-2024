using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    public enum CardID { Heal = 0, SpeedUp = 1,  }

    private PowerUpCard[] powerUps1;
    private PowerUpCard[] powerUps2;
    private PowerUpCard[] powerUps3;

    public int PlayerLevel { get; private set; }

    private CardDisplay[] cardDisplays;
    // Start is called before the first frame update

    private void Awake()
    {
        cardDisplays = FindObjectsByType<CardDisplay>(FindObjectsSortMode.None);
    }
    void Start()
    {
        PlayerLevel = 0;

        powerUps1 = Resources.LoadAll<PowerUpCard>("PowerUps/Tier1");
        powerUps2 = Resources.LoadAll<PowerUpCard>("PowerUps/Tier2"); 
        powerUps3 = Resources.LoadAll<PowerUpCard>("PowerUps/Tier3");
    }

    private void Update()
    {
        //testing purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreasePlayerLevel();
        }
    }
    public void IncreasePlayerLevel()
    {
        //slow time down
        Time.timeScale = 0.25f;
        PlayerLevel++;
        Debug.Log("player level: " + PlayerLevel);

        //reset cards to new power-up options
        switch (PlayerLevel)
        {
            case 1:
                // tier 1 selection of cards
                List<PowerUpCard> lst = new List<PowerUpCard>(powerUps1);
                for (int i  = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, lst.Count - 1);
                    PowerUpCard chosenCard = lst[randomIndex];
                    cardDisplays[i].UpdateCard(chosenCard);
                    lst.RemoveAt(randomIndex);
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

        EventSystem.current.SetSelectedGameObject(cardDisplays[0].gameObject);
    }

    /// <summary>
    /// by clicking on a card you can get that power up
    /// will be called by a button
    /// </summary>
    public void OnClickCard(/*int cardChoice*/)
    {
        for (int i = 0; i < cardDisplays.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == cardDisplays[i].gameObject)
            {
                ApplyPowerUp(cardDisplays[i].currCard);
                break;
            }
        }
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
            case CardID.Heal:
                FindAnyObjectByType<HealthSystem>().Heal((int)card.cardParameter);
                break;
            case CardID.SpeedUp:
                StartCoroutine(FindAnyObjectByType<PlayerController>().MultiplyMoveSpeed(card.cardParameter));
                break;
        }
    }
}
