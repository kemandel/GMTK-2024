using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardDisplay : MonoBehaviour
{

    private Image img;
    private TMP_Text descTxt;
    private TMP_Text titleTxt;
    //add a slot for powerup data

    public void UpdateCard(string title, string desc, Sprite art)
    {
        img.sprite = art;
        descTxt.text = desc;
        titleTxt.text = title;
    }

}
