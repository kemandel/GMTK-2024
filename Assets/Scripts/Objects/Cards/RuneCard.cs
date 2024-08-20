using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Object/Rune Card")]
public class RuneCard : PowerUpCard
{
    public Sprite runeImage;
    public Sprite runeGlowImage;
    public float runeCooldown;
    public CardManager.RuneID runeID;
    public AudioClip sound;
}
