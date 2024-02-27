using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHoverShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MerchantHud MerchantScript;

    public int item;
    public bool restore, sp;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MerchantScript.ItemHovered(restore, sp, item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MerchantScript.Unhovered();
    }
}
