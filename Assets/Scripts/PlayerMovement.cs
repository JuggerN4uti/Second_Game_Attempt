using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //public Rigidbody2D body;
    [Header("Scripts")]
    public Combat CombatScript;
    public PlayerCombat Player;
    public AbilitiesLoadout AbilitiesScript;
    public ItemsLibrary ItemsLibrary;
    public Path path;

    [Header("Stats")]
    public int level;
    //public int dangerLevel;
    public int hitPoints, maxHealth; 
    public int baseStrength, baseResistance, baseDexterity, basePower;
    public int[] statGained;
    public int experience, requiredExperience, skillPoints, abilitiesLearned, spellsLearned, statsGained;
    public int silver, shards, runes, bones;
    public int[] Abilities, Items;
    public bool[] Spells, Rune1, Rune2;
    public bool freeToMove;
    public bool[] itemCheck;
    int itemsCollected, tempi;

    [Header("UI")]
    public GameObject AbilitiesHudObject;
    public GameObject AviableSP;
    public GameObject[] ItemObjects;
    public Image HealthFill, ExperienceFill;
    public Image[] ItemSprites;
    public TMPro.TextMeshProUGUI CurrentHitPoints, CurrentExperience, CurrentSilver, CurrentShards, CurrentRunes, CurrentBones, ItemTooltip;
    public TMPro.TextMeshProUGUI[] CurrentStats;
    public Tutorials Tutorial;
    //public TMPro.TextMeshProUGUI[] StatsText;

    float xInput, yInput;
    bool AbilitiesHudOpen;

    void Start()
    {
        /*maxHealth = PlayerPrefs.GetInt("HP");
        unitsCount[0] = PlayerPrefs.GetInt("Footmen");
        unitsCount[1] = PlayerPrefs.GetInt("Marksmen");
        unitsCount[2] = PlayerPrefs.GetInt("Cavalry");
        unitsCount[3] = PlayerPrefs.GetInt("Mages");*/
        requiredExperience = 50;
        GainExperience(0);
        UpdateStats();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            CollectItem(Random.Range(0, ItemsLibrary.NeutralItems.Length));
    }

    public void EnableMovement()
    {
        freeToMove = true;
    }

    void Move()
    {
        Vector3 tempPos = transform.position;
        tempPos += new Vector3(xInput, yInput, 0) * 5f * Time.deltaTime;
        transform.position = tempPos;
    }

    public void UpdateStats()
    {
        HealthFill.fillAmount = (1f * hitPoints) / (1f * maxHealth);
        CurrentHitPoints.text = hitPoints.ToString("0") + "/" + maxHealth.ToString("0");
        ExperienceFill.fillAmount = (1f * experience) / (1f * requiredExperience);
        if (skillPoints > 0)
            AviableSP.SetActive(true);
        else AviableSP.SetActive(false);
        CurrentExperience.text = experience.ToString("0");
        CurrentSilver.text = silver.ToString("0");
        CurrentShards.text = shards.ToString("0");
        CurrentRunes.text = runes.ToString("0");
        CurrentBones.text = bones.ToString("0");
        CurrentStats[0].text = baseStrength.ToString("0");
        CurrentStats[1].text = baseResistance.ToString("0");
        CurrentStats[2].text = baseDexterity.ToString("0");
        CurrentStats[3].text = basePower.ToString("0");
    }

    public void AbilitiesHud()
    {
        if (freeToMove)
        {
            if (!AbilitiesHudOpen)
            {
                freeToMove = false;
                AbilitiesHudObject.SetActive(true);
                AbilitiesHudOpen = true;
                AbilitiesScript.Display();
            }
        }
        else
        {
            if (AbilitiesHudOpen)
            {
                freeToMove = true;
                AbilitiesHudObject.SetActive(false);
                AbilitiesHudOpen = false;
            }
        }
    }

    public void StartCombat()
    {
        CombatScript.StartCombat();
        freeToMove = false;
    }

    public void FinishCombat()
    {
        //Cache.Display(silverFromCombat, experienceFromCombat, itemFromCombat);
        //Tutorial.Progress();
        UpdateStats();
    }

    void LevelUp()
    {
        level++;
        GainSP(1);
        if (itemCheck[5])
            GainHP(6);
        else GainHP(3);
        if (level % 3 == 0)
            StatUp();
        experience -= requiredExperience;
        requiredExperience = 50 + level * 40 + (level * (level + 1) * 4);

        UpdateStats();
    }

    public void StatUp()
    {
        tempi = statsGained;

        while (tempi >= 4)
        {
            tempi -= 4;
        }

        switch (statGained[tempi])
        {
            case 1:
                baseStrength++;
                break;
            case 2:
                baseResistance++;
                break;
            case 3:
                baseDexterity++;
                break;
            case 4:
                basePower++;
                break;
        }
        statsGained++;

        if (itemCheck[19])
            GainSP(1);
    }

    public void GainSP(int amount)
    {
        skillPoints += amount;

        UpdateStats();
    }

    public void GainHP(int amount)
    {
        maxHealth += amount;
        hitPoints += amount;

        UpdateStats();
    }

    public void RestoreHealth(int amount)
    {
        hitPoints += amount;
        if (hitPoints > maxHealth)
            hitPoints = maxHealth;

        UpdateStats();
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        while (experience >= requiredExperience)
        {
            LevelUp();
        }
        UpdateStats();
    }

    public void GainSilver(int amount)
    {
        silver += amount;
        UpdateStats();
    }

    public void SpendSilver(int amount)
    {
        silver -= amount;
        UpdateStats();
    }

    public void GainShards(int amount)
    {
        shards += amount;
        UpdateStats();
    }

    public void SpendShards(int amount)
    {
        shards -= amount;
        UpdateStats();
    }

    public void GainRunes(int amount)
    {
        runes += amount;
        UpdateStats();
    }

    public void GainBones(int amount)
    {
        bones += amount;
        UpdateStats();
    }

    public void SpendBones(int amount)
    {
        bones -= amount;
        UpdateStats();
    }

    public void CollectItem(int which)
    {
        ItemObjects[itemsCollected].SetActive(true);
        ItemSprites[itemsCollected].sprite = ItemsLibrary.NeutralItems[which].Image;
        Items[itemsCollected] = which;
        itemCheck[which] = true;
        itemsCollected++;

        if (which == 5)
            GainHP(9 + 3 * level);
        else if (which == 15)
            Player.energyRequired -= 2;
        else if (which == 19)
            GainSP(1 + level / 3);
        else if (which == 22)
            GainHP(6);

        UpdateStats();
    }

    public void ItemHovered(int which)
    {
        ItemTooltip.text = ItemsLibrary.NeutralItems[Items[which]].tooltip;
    }

    public void Unhovered()
    {
        ItemTooltip.text = "";
    }
}
