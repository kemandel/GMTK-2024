using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class CardDisplay : MonoBehaviour
{
    private Image image;
    private TMP_Text nameText;
    private TMP_Text descriptionText;
    //add a slot for powerup data
    public PowerUpCard currCard { get; private set; }

    void Start()
    {
        image = GetComponentInChildren<Image>();
        nameText = GetComponentsInChildren<TMP_Text>()[0];
        descriptionText = GetComponentsInChildren<TMP_Text>()[1];

        gameObject.SetActive(false);
    }

    public void UpdateCard(PowerUpCard card)
    {
        image.sprite = card.cardImage;
        descriptionText.text = card.cardDescription;
        nameText.text = card.cardName;
        currCard = card; //need to contain this info to pass from button to ApplyPowerUp() function
    }

}
