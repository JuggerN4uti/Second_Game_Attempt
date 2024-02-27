using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerCombat Player;
    public int spell;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Player.SpellHovered(spell);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.Unhovered();
    }
}
