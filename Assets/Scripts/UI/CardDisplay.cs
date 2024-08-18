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

    void Start()
    {
        image = GetComponentInChildren<Image>();
        nameText = GetComponentsInChildren<TMP_Text>()[0];
        descriptionText = GetComponentsInChildren<TMP_Text>()[1];
    }

    public void UpdateCard(PowerUpCard card)
    {
        image.sprite = card.cardImage;
        descriptionText.text = card.cardDescription;
        nameText.text = card.cardName;
    }

}
