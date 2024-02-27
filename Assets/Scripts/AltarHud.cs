using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltarHud : MonoBehaviour
{
    public GameObject AltarHudObject, GiftHud;
    public PlayerMovement Player;

    //public Image GiftIcon;
    //public Sprite[] GiftSprites;
    //public TMPro.TextMeshProUGUI GiftEffect;
    //public string[] GiftTooltips;

    public Button[] SacrificeOptions;
    public Button AcceptGift;
    public Image SacrificeFill;
    public TMPro.TextMeshProUGUI BloodSacrificeValue;

    //public int[] roll;
    public int healthCost, giftsGranted;
    public float sacrifices, required, bonesValue;
    float temp;

    public void FoundAltar()
    {
        AltarHudObject.SetActive(true);

        healthCost = 3;

        UpdateStats();

        /*roll = Random.Range(0, 6);
        GiftIcon.sprite = GiftSprites[roll];
        if (roll == 0)
            GiftEffect.text = "+" + (20 + 5 * Player.level).ToString("0") + " Experniece";
        else GiftEffect.text = GiftTooltips[roll - 1];

        if (Player.hitPoints <= 14)
            SacrificeOptions[0].interactable = false;
        else SacrificeOptions[0].interactable = true;

        if (Player.maxHealth <= 4)
            SacrificeOptions[1].interactable = false;
        else SacrificeOptions[1].interactable = true;*/
    }

    void UpdateStats()
    {
        BloodSacrificeValue.text = "Sacrifice \n" + healthCost.ToString("0") + " Health";

        if (Player.hitPoints > healthCost)
            SacrificeOptions[0].interactable = true;
        else SacrificeOptions[0].interactable = false;

        if (Player.bones > 0)
            SacrificeOptions[1].interactable = true;
        else SacrificeOptions[1].interactable = false;

        if (sacrifices >= required)
        {
            SacrificeFill.fillAmount = 1f;
            AcceptGift.interactable = true;
        }
        else
        {
            SacrificeFill.fillAmount = sacrifices / required;
            AcceptGift.interactable = false;
        }
    }

    public void PayWithBlood()
    {
        Player.hitPoints -= healthCost;
        Player.UpdateStats();

        FillAltar(2.6f + healthCost * 0.1f);

        healthCost += 2;

        UpdateStats();
    }

    public void ReturnBones()
    {
        temp = 0.4f + Player.bones * 0.06f;
        Invoke("UpdateStats", temp + 0.12f);

        temp /= (Player.bones * 1f);
        for (int i = 0; i < Player.bones; i++)
        {
            Invoke("PlaceBone", i * temp);
        }

        SacrificeOptions[0].interactable = false;
        SacrificeOptions[1].interactable = false;
        AcceptGift.interactable = false;
    }

    void PlaceBone()
    {
        FillAltar(bonesValue);
        bonesValue += 0.01f;

        Player.bones--;
        Player.UpdateStats();

        if (sacrifices >= required)
            SacrificeFill.fillAmount = 1f;
        else SacrificeFill.fillAmount = sacrifices / required;
    }

    void FillAltar(float amount)
    {
        sacrifices += amount;
    }

    public void ActivateAltar()
    {
        GiftHud.SetActive(true);
        sacrifices -= required;
        required += 2f;
    }

    public void GiftofMight()
    {
        Player.StatUp();
        Player.GainHP(giftsGranted * 2);

        giftsGranted++;
        GiftHud.SetActive(false);

        UpdateStats();
    }

    public void GiftofMagic()
    {
        Player.GainSP(1);
        Player.GainExperience(giftsGranted * (3 + Player.level));

        giftsGranted++;
        GiftHud.SetActive(false);

        UpdateStats();
    }

    /*void GainGift(int which)
    {
        switch (roll[which])
        {
            case 0:
                Player.GainExperience(20 + 5 * Player.level);
                break;
            case 1:
                Player.baseStrength++;
                break;
            case 2:
                Player.baseDexterity++;
                break;
            case 3:
                Player.baseResistance++;
                break;
            case 4:
                Player.GainSP(1);
                break;
            case 5:
                Player.basePower++;
                break;
        }
    }*/

    public void Leave()
    {
        AltarHudObject.SetActive(false);
        Player.freeToMove = true;
    }
}
