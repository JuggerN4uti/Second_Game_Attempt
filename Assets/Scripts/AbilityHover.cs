using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerCombat Player;
    public bool basic;
    public int ability;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Player.AbilityHovered(basic, ability);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.Unhovered();
    }
}
