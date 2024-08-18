using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Custom Object/Power Up Card")]
public class PowerUpCard : ScriptableObject
{
    public string cardName;
    public string cardDescription;
    public CardManager.CardID cardID;
    public float cardParameter;
    public Sprite cardImage;
}