using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantHud : MonoBehaviour
{
    public ItemsLibrary Lib;
    public PlayerMovement Player;
    public GameObject CacheHudObject;
    public GameObject[] ItemObjects;
    public Button RestoreButton, SPButton;
    public Button[] ItemButton;
    public int restoreCost, restoreAmount, skillPointCost, skillPointsBought;
    public int[] itemCosts;
    public int[] rolls;
    bool aviable;

    public Image[] ItemSprites;
    public TMPro.TextMeshProUGUI RestoreValue, SPValue, ItemTooltip;
    public TMPro.TextMeshProUGUI[] ItemValue;

    public void Display()
    {
        CacheHudObject.SetActive(true);

        RollItems();
    }

    public void RollItems()
    {
        aviable = false;
        do
        {
            rolls[0] = Random.Range(0, Lib.NeutralItems.Length);
            if (!Player.itemCheck[rolls[0]])
                aviable = true;
        } while (!aviable);

        aviable = false;
        do
        {
            rolls[1] = Random.Range(0, Lib.NeutralItems.Length);
            if (!Player.itemCheck[rolls[1]])
            {
                if (rolls[1] != rolls[0])
                    aviable = true;
            }
        } while (!aviable);

        aviable = false;
        do
        {
            rolls[2] = Random.Range(0, Lib.NeutralItems.Length);
            if (!Player.itemCheck[rolls[2]])
            {
                if (rolls[2] != rolls[0] && rolls[2] != rolls[1])
                    aviable = true;
            }
        } while (!aviable);

        aviable = false;
        do
        {
            rolls[3] = Random.Range(0, Lib.NeutralItems.Length);
            if (!Player.itemCheck[rolls[3]])
            {
                if (rolls[3] != rolls[0] && rolls[3] != rolls[1] && rolls[3] != rolls[2])
                    aviable = true;
            }
        } while (!aviable);

        SetItems();
        UpdateStats();
    }

    void SetItems()
    {
        for (int i = 0; i < 4; i++)
        {
            ItemObjects[i].SetActive(true);
            ItemSprites[i].sprite = Lib.NeutralItems[rolls[i]].Image;
            itemCosts[i] = Random.Range(112, 132);
            ItemValue[i].text = itemCosts[i].ToString("0");
        }
    }

    public void UpdateStats()
    {
        restoreCost = 50;
        restoreAmount = Player.maxHealth / 3;
        RestoreValue.text = restoreCost.ToString("0");
        if (Player.silver >= restoreCost)
            RestoreButton.interactable = true;
        else RestoreButton.interactable = false;

        skillPointCost = 50 + 15 * skillPointsBought;
        SPValue.text = skillPointCost.ToString("0");
        if (Player.silver >= skillPointCost)
            SPButton.interactable = true;
        else SPButton.interactable = false;

        for (int i = 0; i < 4; i++)
        {
            if (Player.silver >= itemCosts[i])
                ItemButton[i].interactable = true;
            else ItemButton[i].interactable = false;
        }
    }

    public void ItemHovered(bool restore, bool sp, int which)
    {
        if (restore)
            ItemTooltip.text = "Restore " + restoreAmount.ToString("") + " Health";
        else if (sp)
            ItemTooltip.text = "Gain 1 Skill Point";
        else ItemTooltip.text = Lib.NeutralItems[rolls[which]].tooltip;
    }

    public void Unhovered()
    {
        ItemTooltip.text = "";
    }

    public void BuyMeal()
    {
        Player.SpendSilver(restoreCost);
        Player.RestoreHealth(restoreAmount);

        UpdateStats();
    }

    public void BuySP()
    {
        Player.SpendSilver(skillPointCost);
        Player.GainSP(1);
        skillPointsBought++;

        UpdateStats();
    }

    public void BuyItem(int which)
    {
        Player.SpendSilver(itemCosts[which]);
        Player.CollectItem(rolls[which]);
        ItemObjects[which].SetActive(false);

        UpdateStats();
    }

    public void Leave()
    {
        CacheHudObject.SetActive(false);
        Player.freeToMove = true;
    }
}
