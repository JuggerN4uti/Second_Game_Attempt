using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CacheHud : MonoBehaviour
{
    public ItemsLibrary Lib;
    public PlayerMovement Player;
    public GameObject CacheHudObject, ItemsChoiceObject, SilverObject, ShardsObject, BonesObject, ExperienceObject;
    int aviableSilver, aviableShards, aviableBones, aviableExperience, pickUpsLeft;
    public int[] rolls;
    bool aviable;

    public TMPro.TextMeshProUGUI CurrentSilver, CurrentShards, CurrentBones, CurrentExperience, ItemTooltip;
    public Image[] ItemSprites;

    public void Display(int silver, int shards, int bones, int experience, bool potion, bool item) //potem dodaæ potionki?
    {
        CacheHudObject.SetActive(true);

        aviableSilver = silver;
        SilverObject.SetActive(true);
        aviableShards = shards;
        ShardsObject.SetActive(true);
        aviableBones = bones;
        BonesObject.SetActive(true);
        aviableExperience = experience;
        ExperienceObject.SetActive(true);

        CurrentSilver.text = aviableSilver.ToString("0");
        CurrentShards.text = aviableShards.ToString("0");
        CurrentBones.text = aviableBones.ToString("0");
        CurrentExperience.text = aviableExperience.ToString("0");

        if (item)
        {
            RollItems();
            pickUpsLeft = 5;
        }
        else
        {
            ItemsChoiceObject.SetActive(false);
            pickUpsLeft = 4;
        }
    }

    public void FoundStash()
    {
        pickUpsLeft = 2;

        CacheHudObject.SetActive(true);
        aviableSilver = Random.Range(10, 21);
        SilverObject.SetActive(true);
        aviableShards = Random.Range(2, 4);
        ShardsObject.SetActive(true);
        if (Player.itemCheck[14])
        {
            aviableBones = 1;
            BonesObject.SetActive(true);
            pickUpsLeft++;
        }
        else
        {
            aviableBones = 0;
            BonesObject.SetActive(false);
        }
        aviableExperience = 0;
        ExperienceObject.SetActive(false);

        for (int i = 0; i < 2; i++)
        {
            if (Random.Range(0, 2) == 0)
                aviableSilver += 5;
            else aviableShards++;
        }

        CurrentSilver.text = aviableSilver.ToString("0");
        CurrentShards.text = aviableShards.ToString("0");
        CurrentBones.text = aviableBones.ToString("0");
        CurrentExperience.text = "";

        ItemsChoiceObject.SetActive(false);

        // potion
    }

    public void FoundTreasure()
    {
        pickUpsLeft = 3;

        CacheHudObject.SetActive(true);
        aviableSilver = Random.Range(32, 57);
        SilverObject.SetActive(true);
        aviableShards = Random.Range(6, 10);
        ShardsObject.SetActive(true);
        aviableBones = 0;
        BonesObject.SetActive(false);
        aviableExperience = 0;
        ExperienceObject.SetActive(false);

        CurrentSilver.text = aviableSilver.ToString("0");
        CurrentShards.text = aviableShards.ToString("0");
        CurrentExperience.text = "";

        RollItems();
    }

    void RollItems()
    {
        ItemsChoiceObject.SetActive(true);

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

        SetItems();
    }

    void SetItems()
    {
        for (int i = 0; i < 3; i++)
        {
            ItemSprites[i].sprite = Lib.NeutralItems[rolls[i]].Image;
        }
    }

    public void ItemHovered(int which)
    {
        ItemTooltip.text = Lib.NeutralItems[rolls[which]].tooltip;
    }

    public void Unhovered()
    {
        ItemTooltip.text = "";
    }

    public void SilverPick()
    {
        Player.GainSilver(aviableSilver);
        SilverObject.SetActive(false);
        pickUpsLeft--;
        if (pickUpsLeft == 0)
            Leave();
    }

    public void ShardsPick()
    {
        Player.GainShards(aviableShards);
        ShardsObject.SetActive(false);
        pickUpsLeft--;
        if (pickUpsLeft == 0)
            Leave();
    }

    public void BonesPick()
    {
        Player.GainBones(aviableBones);
        BonesObject.SetActive(false);
        pickUpsLeft--;
        if (pickUpsLeft == 0)
            Leave();
    }

    public void ExperiencePick()
    {
        Player.GainExperience(aviableExperience);
        ExperienceObject.SetActive(false);
        pickUpsLeft--;
        if (pickUpsLeft == 0)
            Leave();
    }

    public void ItemPick(int which)
    {
        Player.CollectItem(rolls[which]);
        ItemTooltip.text = "";
        ItemsChoiceObject.SetActive(false);
        pickUpsLeft--;
        if (pickUpsLeft == 0)
            Leave();
    }

    public void Leave()
    {
        CacheHudObject.SetActive(false);
        Player.freeToMove = true;
    }
}
