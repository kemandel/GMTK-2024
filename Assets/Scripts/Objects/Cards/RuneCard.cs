using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Object/Rune Card")]
public class RuneCard : PowerUpCard
{
    public Sprite runeImage;
    public float runeCooldown;
    public CardManager.RuneID runeID;
}
