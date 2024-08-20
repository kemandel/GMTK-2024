using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    public enum CardID { Heal, SpeedUp, AttackSpeedUp, ChangeInvulnerableTime, ChangeRuneCooldownScalar, AddRune, BlessingWind, BlessingEarth }
    public enum RuneID { Time, War, Death, Life }

    //cards from resources
    private PowerUpCard[] powerUps1;
    private PowerUpCard[] powerUps2;
    private PowerUpCard[] powerUps3;

    //copies of array into lists
    private List<PowerUpCard> powerUp1List = new List<PowerUpCard>();
    private List<PowerUpCard> powerUp2List = new List<PowerUpCard>();
    private List<PowerUpCard> powerUp3List = new List<PowerUpCard>();


    public int PlayerLevel { get; private set; }

    private CardDisplay[] cardDisplays;
    private Coroutine timeCoroutine;

    public HorizontalLayoutGroup cardCollectionUI;

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

        powerUp1List.AddRange(powerUps1);
        powerUp2List.AddRange(powerUps2);
        powerUp3List.AddRange(powerUps3);

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
        StartCoroutine(IncreasePlayerLevelCoroutine());
    }
    public IEnumerator IncreasePlayerLevelCoroutine()
    {

        PlayerLevel++;
        Debug.Log("player level: " + PlayerLevel);

        //reset cards to new power-up options
        List<PowerUpCard> cards = new List<PowerUpCard>();
        switch (PlayerLevel)
        {
            case 1:
                // tier 1 selection of cards
                cards.AddRange(powerUp1List);
                break;
            case 2:
                // tier 2 selection of cards
                cards.AddRange(powerUp1List);
                cards.AddRange(powerUp2List);
                break;
            default:
            case 3:
                // tier 3 selection of cards
                cards.AddRange(powerUp1List);
                cards.AddRange(powerUp2List);
                cards.AddRange(powerUp3List);
                // tier 3 selection of cards
                // code block
                break;
        }
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, cards.Count);
            PowerUpCard chosenCard = cards[randomIndex];
            cardDisplays[i].UpdateCard(chosenCard);
            cards.RemoveAt(randomIndex);
        }

        //enable card power ups
        foreach (CardDisplay card in cardDisplays)
        {
            card.gameObject.SetActive(true);
        }

        //animate cards coming in
        Animator cardsAnim = cardCollectionUI.GetComponent<Animator>();
        cardsAnim.SetTrigger("descend");
        yield return null;
        yield return new WaitForSeconds(cardsAnim.GetCurrentAnimatorStateInfo(0).length);

        //slow time down
        timeCoroutine = FindAnyObjectByType<TimeManager>().ChangeSceneTime(.25f);
    }

    /// <summary>
    /// by clicking on a card you can get that power up
    /// will be called by a button
    /// </summary>
    public void OnClickCard()
    {
        for (int i = 0; i < cardDisplays.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == cardDisplays[i].gameObject)
            {
                //loop through list of tier 1 cards to delete the card if it is not repeatable

                for (int j = 0; j < powerUp1List.Count; j++)
                {
                    if (powerUp1List[j] == cardDisplays[i].currCard && !powerUp1List[j].repeatable)
                    {
                        powerUp1List.RemoveAt(j);
                    }
                }

                //loop through list of tier 2 cards to delete the card if it is not repeatable
                for (int j = 0; j < powerUp2List.Count; j++)
                {
                    if (powerUp2List[j] == cardDisplays[i].currCard && !powerUp2List[j].repeatable)
                    {
                        powerUp2List.RemoveAt(j);
                    }
                }

                //loop through list of tier 3 cards to delete the card if it is not repeatable
                for (int j = 0; j < powerUp3List.Count; j++)
                {
                    if (powerUp3List[j] == cardDisplays[i].currCard && !powerUp3List[j].repeatable)
                    {
                        powerUp3List.RemoveAt(j);
                    }
                }

                ApplyPowerUp(cardDisplays[i].currCard);
                break;
            }
        }
        FindAnyObjectByType<TimeManager>().StopCoroutine(timeCoroutine);

        //animate cards coming in
        Animator cardsAnim = cardCollectionUI.GetComponent<Animator>();
        cardsAnim.SetTrigger("ascend");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ApplyPowerUp(PowerUpCard card)
    {
        Debug.Log("selected card: " + card.cardID);
        PlayerController player = FindAnyObjectByType<PlayerController>();
        switch (card.cardID)
        {
            case CardID.Heal:
                FindAnyObjectByType<HealthSystem>().Heal((int)card.cardParameter);
                break;
            case CardID.SpeedUp:
                StartCoroutine(player.MultiplyMoveSpeed(card.cardParameter));
                break;
            case CardID.AttackSpeedUp:
                StartCoroutine(player.MultiplyAttackSpeed(card.cardParameter));
                break;
            case CardID.ChangeInvulnerableTime:
                player.baseInvulnerableTime = card.cardParameter;
                break;
            case CardID.ChangeRuneCooldownScalar:
                player.baseRuneCooldownScalar = card.cardParameter;
                break;
            case CardID.AddRune:
                player.SetRune(card as RuneCard);
                break;
            case CardID.BlessingWind:
                player.EnemyDefeatedEvent += WindBlessingEffect;
                break;
            case CardID.BlessingEarth:
                player.TookDamageEvent += EarthBlessingEffect;
                break;
        }
    }

    private void WindBlessingEffect()
    {
        Debug.Log("Wind Blessing Effect");
        PlayerController player = FindAnyObjectByType<PlayerController>();
        StartCoroutine(player.MultiplyMoveSpeed(1.05f, 3));
        StartCoroutine(player.MultiplyAttackSpeed(1.05f, 3));
    }

    private void EarthBlessingEffect()
    {
        // Earth Blessing Logic
    }
}
