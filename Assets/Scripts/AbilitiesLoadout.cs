using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbilitiesLoadout : MonoBehaviour
{
    [Header("Scripts")]
    public AbilitiesLibrary Library;
    public PlayerMovement PlayerScript;

    [Header("UI")]
    public GameObject[] AbilitiesObject;
    public GameObject[] SpellObject;
    public GameObject AbilityPage, SpellPage;
    public TMPro.TextMeshProUGUI[] AbilityLevels, AbilityCosts, AbilityCooldownGains, SpellCosts, SpellCostGains;
    public TMPro.TextMeshProUGUI Tooltip, SkillPointsCount;
    public Image[] AbilityIcons, SpellIcons, Rune1Icons, Rune2Icons, RunePick;
    public Sprite EmptyRuneSprite;
    public GameObject LeanAbilityObject, LearnSpellObject, ChooseRuneObject;
    public Button LearnAbilityButton, LearnSpellButton;

    [Header("Slots")]
    public GameObject[] SlotIcons;
    public GameObject[] SlotIcons2;
    public Button[] SlotButtons, SlotButtons2;
    public Image[] SlotImages, SlotImages2;
    public Sprite AviableToBuySlot, ClosedSlot;
    public int slotsAviable, slotsAviable2;

    [Header("Ability Details")]
    public GameObject AbilityHud;
    public Button UpgradeAbilityButton, ForgetAbilityButton, Rune1Button, Rune2Button;
    public Image AbilityIcon, DetailRune1, DetailRune2;
    public TMPro.TextMeshProUGUI DetailLevel, DetailCost, DetailCooldown, ForgetSPGain, ForgetShardsCost;
    public int current, currentDetails, currentDetailLevel, forgetShards;

    [Header("Stats")]
    public int[] AbilityPlace;
    public int[] SpellPlace;
    public bool spellPage;
    int abilityCount, spellCount, currentRune;

    [Header("Learn Choices")]
    public GameObject AbilityChoiceHud;
    public GameObject SpellChoiceHud;
    public Image[] ChoicesIcons;
    public TMPro.TextMeshProUGUI[] ChoicesLevels, ChoicesCosts, ChoicesCooldownGains;
    public int[] rolls;
    bool viable;

    public void ChooseAbility(int which)
    {
        PlayerScript.Abilities[rolls[which]]++;
        PlayerScript.abilitiesLearned++;
        if (PlayerScript.Abilities[rolls[which]] == 3)
            RuneChoice(which);
        else LeanAbilityObject.SetActive(true);

        if (PlayerScript.Abilities[rolls[which]] == 5)
            GainRune(which);
        //PlayerScript.Tutorial.Progress();
        HideTooltip();
        Display();
        LearnSpellObject.SetActive(true);
        AbilityChoiceHud.SetActive(false);
    }

    public void ChooseSpell(int which)
    {
        PlayerScript.Spells[rolls[which]] = true;
        PlayerScript.spellsLearned++;
        HideTooltip();
        Display();
        LeanAbilityObject.SetActive(true);
        LearnSpellObject.SetActive(true);
        SpellChoiceHud.SetActive(false);
    }

    public void Display()
    {
        if (spellPage)
            DisplaySpells();
        else DisplayAbilities();
    }

    public void DisplayAbilities()
    {
        for (int i = 0; i < AbilitiesObject.Length; i++)
        {
            AbilitiesObject[i].SetActive(false);
        }
        abilityCount = 0;
        for (int i = 0; i < PlayerScript.Abilities.Length; i++)
        {
            if (PlayerScript.Abilities[i] > 0)
            {
                AbilitiesObject[abilityCount].SetActive(true);
                AbilityIcons[abilityCount].sprite = Library.LightAbilities[i].Icon;
                AbilityLevels[abilityCount].text = PlayerScript.Abilities[i].ToString("");
                AbilityCosts[abilityCount].text = Library.LightAbilities[i].Cost[PlayerScript.Abilities[i] - 1].ToString("");
                AbilityCooldownGains[abilityCount].text = Library.LightAbilities[i].Cooldown[PlayerScript.Abilities[i] - 1].ToString("");
                AbilityPlace[abilityCount] = i;
                if (PlayerScript.Rune1[i]) Rune1Icons[abilityCount].sprite = Library.LightAbilities[i].Runes[0];
                else Rune1Icons[abilityCount].sprite = EmptyRuneSprite;
                if (PlayerScript.Rune2[i]) Rune2Icons[abilityCount].sprite = Library.LightAbilities[i].Runes[1];
                else Rune2Icons[abilityCount].sprite = EmptyRuneSprite;
                abilityCount++;
            }
        }

        for (int i = 0; i < 10; i++)
        {
            SlotButtons[i].interactable = false;
        }

        SkillPointsCount.text = PlayerScript.skillPoints.ToString("0");

        if (PlayerScript.skillPoints > 0)
        {
            if (slotsAviable > abilityCount)
                LearnAbilityButton.interactable = true;
            else LearnAbilityButton.interactable = false;

            if (slotsAviable < 10) SlotButtons[slotsAviable].interactable = true;

        }
        else LearnAbilityButton.interactable = false;
    }

    public void DisplaySpells()
    {
        for (int i = 0; i < SpellObject.Length; i++)
        {
            SpellObject[i].SetActive(false);
        }

        spellCount = 0;
        for (int i = 0; i < PlayerScript.Spells.Length; i++)
        {
            if (PlayerScript.Spells[i])
            {
                SpellObject[spellCount].SetActive(true);
                SpellIcons[spellCount].sprite = Library.LightSpells[i].Icon;
                SpellCosts[spellCount].text = Library.LightSpells[i].ManaCost.ToString("");
                SpellCostGains[spellCount].text = Library.LightSpells[i].CostGain.ToString("");
                SpellPlace[spellCount] = i;
                spellCount++;
            }
        }

        for (int i = 0; i < 7; i++)
        {
            SlotButtons2[i].interactable = false;
        }

        SkillPointsCount.text = PlayerScript.skillPoints.ToString("0");

        if (PlayerScript.skillPoints > 0)
        {
            if (slotsAviable2 > spellCount)
                LearnSpellButton.interactable = true;
            else LearnSpellButton.interactable = false;

            if (slotsAviable2 < 7) SlotButtons2[slotsAviable2].interactable = true;

        }
        else LearnSpellButton.interactable = false;
    }

    public void SwitchPage()
    {
        if (spellPage)
            spellPage = false;
        else spellPage = true;

        AbilityPage.SetActive(!spellPage);
        SpellPage.SetActive(spellPage);

        Display();
    }

    public void LearnAbility()
    {
        PlayerScript.skillPoints--;
        AbilityHud.SetActive(false);
        LeanAbilityObject.SetActive(false);
        LearnSpellObject.SetActive(false);
        PlayerScript.UpdateStats();
        AbilityChoiceHud.SetActive(true);
        RollAbilities();
    }

    public void LearnSpell()
    {
        PlayerScript.skillPoints--;
        LeanAbilityObject.SetActive(false);
        LearnSpellObject.SetActive(false);
        PlayerScript.UpdateStats();
        SpellChoiceHud.SetActive(true);
        RollSpells();
    }

    public void RuneChoice(int which)
    {
        ChooseRuneObject.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            RunePick[i].sprite = Library.LightAbilities[rolls[which]].Runes[i];
        }
        currentRune = rolls[which];
    }

    public void ChooseRune(int which)
    {
        if (which == 0)
            AquireRune(currentRune, true);
        else AquireRune(currentRune, false);

        ChooseRuneObject.SetActive(false);
        HideTooltip();
        LeanAbilityObject.SetActive(true);
        Display();
    }

    public void GainRune(int which)
    {
        if (PlayerScript.Rune1[rolls[which]])
            AquireRune(rolls[which], true);
        else AquireRune(rolls[which], false);

        Display();
    }

    void AquireRune(int which, bool first)
    {
        if (first)
        {
            PlayerScript.Rune1[which] = true;
            if (Library.LightAbilities[which].cooldownReduction[0])
            {
                for (int i = 0; i < 5; i++)
                {
                    Library.LightAbilities[which].Cooldown[i]--;
                }
            }
        }
        else
        {
            PlayerScript.Rune2[which] = true;
            if (Library.LightAbilities[which].cooldownReduction[1])
            {
                for (int i = 0; i < 5; i++)
                {
                    Library.LightAbilities[which].Cooldown[i]--;
                }
            }
        }
    }

    void RollAbilities()
    {
        viable = false;
        do
        {
            rolls[0] = Random.Range(0, Library.LightAbilities.Length);
            if (PlayerScript.Abilities[rolls[0]] == 0)
                viable = true;
        } while (viable == false);

        viable = false;
        do
        {
            rolls[1] = Random.Range(0, Library.LightAbilities.Length);
            if (PlayerScript.Abilities[rolls[1]] == 0)
            {
                if (rolls[0] != rolls[1])
                    viable = true;
            }
        } while (viable == false);

        viable = false;
        do
        {
            rolls[2] = Random.Range(0, Library.LightAbilities.Length);
            if (PlayerScript.Abilities[rolls[2]] == 0)
            {
                if (rolls[0] != rolls[2] && rolls[1] != rolls[2])
                    viable = true;
            }
        } while (viable == false);

        for (int i = 0; i < 3; i++)
        {
            ChoicesIcons[i].sprite = Library.LightAbilities[rolls[i]].Icon;
            ChoicesLevels[i].text = (PlayerScript.Abilities[rolls[i]] + 1).ToString("0");
            ChoicesCosts[i].text = Library.LightAbilities[rolls[i]].Cost[PlayerScript.Abilities[rolls[i]]].ToString("");
            ChoicesCooldownGains[i].text = Library.LightAbilities[rolls[i]].Cooldown[PlayerScript.Abilities[rolls[i]]].ToString("");
        }
    }

    void RollSpells()
    {
        viable = false;
        do
        {
            rolls[0] = Random.Range(0, Library.LightSpells.Length);
            if (!PlayerScript.Spells[rolls[0]])
                viable = true;
        } while (viable == false);

        viable = false;
        do
        {
            rolls[1] = Random.Range(0, Library.LightSpells.Length);
            if (!PlayerScript.Spells[rolls[1]])
            {
                if (rolls[0] != rolls[1])
                    viable = true;
            }
        } while (viable == false);

        for (int i = 0; i < 2; i++)
        {
            ChoicesIcons[i + 3].sprite = Library.LightSpells[rolls[i]].Icon;
            //ChoicesLevels[i + 3].text = "";
            ChoicesCosts[i + 3].text = Library.LightSpells[rolls[i]].ManaCost.ToString("");
            ChoicesCooldownGains[i + 3].text = Library.LightSpells[rolls[i]].CostGain.ToString("");
        }
    }

    public void ShowTooltip(bool basic, bool choice, bool spell, bool rune, int which)
    {
        if (basic)
        {
            if (spell) Tooltip.text = "Deal 7 + 1x Strength Magic Damage";
            else
            {
                if (which == 0)
                    Tooltip.text = "Deal 10 + 1x Strength Damage";
                else Tooltip.text = "Gain 8 + 1x Resistance Block";
            }
        }
        else
        {
            if (rune)
            {
                DisplayRuneTooltip(currentRune, which);
            }
            else if (choice)
            {
                if (spell)
                    DisplaySpellTooltip(rolls[which]);
                else DisplayAbilityTooltip(rolls[which], PlayerScript.Abilities[rolls[which]] + 1);
            }
            else
            {
                if (spell)
                    DisplaySpellTooltip(SpellPlace[which]);
                else DisplayAbilityTooltip(AbilityPlace[which], PlayerScript.Abilities[AbilityPlace[which]]);
            }
        }
    }

    public void ShowDetailTooltip(bool upgrade, bool forget, bool rune1, bool rune2)
    {
        if (upgrade)
            Tooltip.text = "Upgrade this Ability, costs Skill Point";
        else if (forget)
            Tooltip.text = "Forget this Ability, regain Skill Points, costs Arcane Shards";
        else if (rune1)
            DisplayRuneTooltip(currentDetails, 0);
        else if (rune2)
            DisplayRuneTooltip(currentDetails, 1);
        else DisplayAbilityTooltip(currentDetails, PlayerScript.Abilities[currentDetails]);
    }

    public void BuyAbilitySlot()
    {
        PlayerScript.skillPoints--;
        SlotIcons[slotsAviable].SetActive(false);
        slotsAviable++;
        SlotImages[slotsAviable].sprite = AviableToBuySlot;

        Display();
    }

    public void BuySpellSlot()
    {
        PlayerScript.skillPoints--;
        SlotIcons2[slotsAviable2].SetActive(false);
        slotsAviable2++;
        SlotImages2[slotsAviable2].sprite = AviableToBuySlot;

        Display();
    }

    public void HideTooltip()
    {
        Tooltip.text = "";
    }

    void DisplayAbilityTooltip(int whichAbility, int abilityLevel)
    {
        Tooltip.text = Library.LightAbilities[whichAbility].Tooltips[abilityLevel - 1];
    }

    void DisplaySpellTooltip(int whichSpell)
    {
        Tooltip.text = Library.LightSpells[whichSpell].Tooltip;
    }

    void DisplayRuneTooltip(int whichAbility, int rune)
    {
        Tooltip.text = Library.LightAbilities[whichAbility].RunesTooltip[rune];
    }

    public void OpenAbilityDetail(int which)
    {
        AbilityHud.SetActive(true);

        current = which;
        currentDetails = AbilityPlace[which];
        currentDetailLevel = PlayerScript.Abilities[currentDetails];

        AbilityIcon.sprite = Library.LightAbilities[currentDetails].Icon;
        DetailLevel.text = currentDetailLevel.ToString("0");
        DetailCost.text = Library.LightAbilities[currentDetails].Cost[currentDetailLevel - 1].ToString("");
        DetailCooldown.text = Library.LightAbilities[currentDetails].Cooldown[currentDetailLevel - 1].ToString("");
        if (PlayerScript.Rune1[currentDetails]) DetailRune1.sprite = Library.LightAbilities[currentDetails].Runes[0];
        else
        {
            DetailRune1.sprite = EmptyRuneSprite;
            if (PlayerScript.runes > 0)
                Rune1Button.interactable = true;
            else Rune1Button.interactable = false;
        }
        if (PlayerScript.Rune2[currentDetails]) DetailRune2.sprite = Library.LightAbilities[currentDetails].Runes[1];
        else
        {
            DetailRune2.sprite = EmptyRuneSprite;
            if (PlayerScript.runes > 0)
                Rune2Button.interactable = true;
            else Rune2Button.interactable = false;
        }
        ForgetSPGain.text = "+ " + currentDetailLevel.ToString("0");
        forgetShards = 2 + currentDetailLevel;
        if (PlayerScript.Rune1[currentDetails])
            forgetShards++;
        if (PlayerScript.Rune2[currentDetails])
            forgetShards++;
        ForgetShardsCost.text = "- " + forgetShards.ToString("0");

        if (PlayerScript.skillPoints > 0 && currentDetailLevel < 6)
            UpgradeAbilityButton.interactable = true;
        else UpgradeAbilityButton.interactable = false;

        if (PlayerScript.shards >= forgetShards)
            ForgetAbilityButton.interactable = true;
        else ForgetAbilityButton.interactable = false;

        // wszystkie efekty przycisków do zrobienia
    }

    public void UpgradeAbility()
    {
        PlayerScript.Abilities[currentDetails]++;
        PlayerScript.abilitiesLearned++;

        PlayerScript.skillPoints--;
        PlayerScript.UpdateStats();

        HideTooltip();
        OpenAbilityDetail(current);
        Display();
    }

    public void ForgetAbility()
    {
        PlayerScript.Abilities[currentDetails] = 0;
        PlayerScript.abilitiesLearned -= currentDetailLevel;
        if (PlayerScript.Rune1[currentDetails])
            PlayerScript.GainRunes(1);
        if (PlayerScript.Rune2[currentDetails])
            PlayerScript.GainRunes(1);
        PlayerScript.SpendShards(forgetShards);
        PlayerScript.GainSP(currentDetailLevel);

        CloseAbilityDetial();
        Display();
    }

    public void EquipRune(bool first)
    {
        PlayerScript.runes--;
        if (first)
        {
            PlayerScript.Rune1[currentDetails] = true;
            if (Library.LightAbilities[currentDetails].cooldownReduction[0])
            {
                for (int i = 0; i < 5; i++) // 5 = iloœæ leveli spella
                {
                    Library.LightAbilities[currentDetails].Cooldown[i]--;
                }
            }
        }
        else
        {
            PlayerScript.Rune2[currentDetails] = true;
            if (Library.LightAbilities[currentDetails].cooldownReduction[1])
            {
                for (int i = 0; i < 5; i++)
                {
                    Library.LightAbilities[currentDetails].Cooldown[i]--;
                }
            }
        }

        OpenAbilityDetail(current);
        Display();
    }

    public void CloseAbilityDetial()
    {
        AbilityHud.SetActive(false);
    }
}