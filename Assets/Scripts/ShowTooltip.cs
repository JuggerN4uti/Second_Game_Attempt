using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilitiesLoadout Loadout;
    public int order;
    public bool basic, choice, spell, rune;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Loadout.ShowTooltip(basic, choice, spell, rune, order);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Loadout.HideTooltip();
    }
}
