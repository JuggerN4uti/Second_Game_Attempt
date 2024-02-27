using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHoverCache : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CacheHud CacheScript;

    public int item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CacheScript.ItemHovered(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CacheScript.Unhovered();
    }
}
