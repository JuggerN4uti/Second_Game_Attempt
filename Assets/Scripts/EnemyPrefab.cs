using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    public int MinHealth, MaxHealth, MinShield, MaxShield, Tenacity;
    public int MovesCount;
    public Sprite UnitSprite;
    public Sprite[] MovesSprite;
    public int[] MovesChances, StatusEffects, MovesValues;
    public string[] additionalText;
    public bool[] attackIntention;
}
