using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuneDisplay : MonoBehaviour
{
    private Image runeImg;
    public RuneCard runeCard;
    // Start is called before the first frame update
    void Start()
    {
        runeImg = GetComponentsInChildren<Image>()[2];

        //disable glow
        GetComponentsInChildren<Image>()[0].enabled = false;
        runeImg.enabled = false;
    }


    public void UpdateRune(RuneCard rune)
    {
        if (!runeImg.isActiveAndEnabled)
        {
            runeImg.enabled = true;

            //enable glow
            GetComponentsInChildren<Image>()[0].enabled = true;
        }
        runeImg.sprite = rune.runeGlowImage;
        runeCard = rune;
    }
}
