using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDetailTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilitiesLoadout Loadout;
    public bool upgrade, forget, rune1, rune2;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Loadout.ShowDetailTooltip(upgrade, forget, rune1, rune2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Loadout.HideTooltip();
    }
}
