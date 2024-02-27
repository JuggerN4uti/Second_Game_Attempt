using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public Sprite Icon;

    public int[] Cost, Cooldown;
    public string[] Tooltips, RunesTooltip;
    public Sprite[] Runes;
    public bool[] cooldownReduction;
}
