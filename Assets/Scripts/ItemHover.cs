using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerMovement PlayerScript;

    public int item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerScript.ItemHovered(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerScript.Unhovered();
    }
}
