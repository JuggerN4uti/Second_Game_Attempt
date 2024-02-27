using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public PlayerMovement PlayerScript;
    public Combat CombatScript;
    public AbilitiesLibrary Library;
    public ItemsLibrary ItemsLibrary;

    [Header("Stats")]
    public int playerLevel;
    public int maxHealth, actionGain;
    public int hitPoints, shield, block;
    public int actions, energy, energyRequired, mana, manaPoints, statusCount, blastCost;
    public int strength, resistance, dexterity, power, valor, bleed, poison, weak, vulnerable, freeze, armor, frail, storedBlock, impervious;
    public int[] statusEffects;
    float temp;
    int tempi, attacks, blockGained;

    [Header("UI")]
    public Image HealthBarFill;
    public Image ListImage, EnergyFill, ManaFill;
    public Sprite AbilitySprite, SpellSprite;
    public GameObject ShieldIcon, BlockIcon, AbilitiesList, SpellsList;
    public TMPro.TextMeshProUGUI HealthValue, ShieldValue, BlockValue, ActionsValue, EnergyValue, ManaValue, AbilityTooltip;
    public GameObject[] Ability, Spell;
    public Image[] AbilityIcon, CooldownFill, SpellIcon;
    public TMPro.TextMeshProUGUI BlastCostValue;
    public TMPro.TextMeshProUGUI[] AbilityCostValue, AbilityCooldownValue, SpellManaCostValue;
    public GameObject[] StatusObjects;
    public Image[] StatusImages;
    public TMPro.TextMeshProUGUI[] StatusValues;
    public Sprite[] StatusSprites;

    [Header("Abilities")]
    public Button AttackButton;
    public Button BlockButton;
    public Button BlastButton;
    public Button[] AbilityButton, SpellButton;
    public int[] Abilities, AbilityLevel, Cost, CooldownGain, Cooldown;
    public int[] Spells, SpellLevel, ManaCost, CostIncrease;
    public bool[] Rune1, Rune2;
    public int currentAbility, currentSpell;
    public bool free, spells;

    [Header("Display")]
    public GameObject DisplayObject;
    public Rigidbody2D Body;
    public Transform Origin;
    public Display Displayed;
    public Sprite DamageSprite, MagicDamageSprite, HealthSprite, ShieldSprite, BlockSprite;

    public void Set()
    {
        /* efekty
        for (int i = 0; i < statusEffects.Length; i++)
        {
            statusEffects[i] = 0;
        }*/
        playerLevel = PlayerScript.level;
        Reset();
        ItemsCheck();
        SetAbilities();
        UpdateStats();
    }

    void Reset()
    {
        actionGain = 3;
        strength = PlayerScript.baseStrength; dexterity = PlayerScript.baseDexterity; resistance = PlayerScript.baseResistance; power = PlayerScript.basePower;
        valor = 0; bleed = 0; poison = 0; weak = 0; vulnerable = 0; freeze = 0;
        armor = 0; frail = 0; storedBlock = 0; impervious = 0;
        maxHealth = PlayerScript.maxHealth;
        hitPoints = PlayerScript.hitPoints;
        attacks = 0;
        shield = 0;
        block = 0;
        actions = 0;
        energy = PlayerScript.abilitiesLearned * 2;
        mana = 0;
        manaPoints = PlayerScript.spellsLearned;
        blastCost = 3;
        blockGained = 0;
    }

    void ItemsCheck()
    {
        if (PlayerScript.itemCheck[0]) GainStrength(1 + playerLevel / 3);
        if (PlayerScript.itemCheck[1]) GainDexterity(1 + playerLevel / 3);
        if (PlayerScript.itemCheck[2]) GainResistance(1 + playerLevel / 3);
        if (PlayerScript.itemCheck[4]) storedBlock = 20;
        if (PlayerScript.itemCheck[7])
        {
            GainArmor(3);
            GainVulnerable(1);
        }
        if (PlayerScript.itemCheck[12]) GainPower(1 + playerLevel / 3);
        if (PlayerScript.itemCheck[15]) GainEnergy(4);
        if (PlayerScript.itemCheck[20]) actionGain++;
        if (PlayerScript.itemCheck[21]) actionGain++;
    }

    void SetAbilities()
    {
        currentAbility = 0;
        for (int i = 0; i < Ability.Length; i++)
        {
            Ability[i].SetActive(false);
        }
        for (int i = 0; i < Ability.Length; i++)
        {
            Ability[i].SetActive(false);
        }
        for (int i = 0; i < PlayerScript.Abilities.Length; i++)
        {
            if (PlayerScript.Abilities[i] > 0)
            {
                Ability[currentAbility].SetActive(true);
                AbilityIcon[currentAbility].sprite = Library.LightAbilities[i].Icon;
                Abilities[currentAbility] = i;
                AbilityLevel[currentAbility] = PlayerScript.Abilities[i];
                Cost[currentAbility] = Library.LightAbilities[i].Cost[AbilityLevel[currentAbility] - 1];
                CooldownGain[currentAbility] = Library.LightAbilities[i].Cooldown[AbilityLevel[currentAbility] - 1];
                if (PlayerScript.itemCheck[13]) CooldownGain[currentAbility]--;
                if (PlayerScript.itemCheck[21]) CooldownGain[currentAbility]++;
                Cooldown[currentAbility] = 0;
                AbilityCostValue[currentAbility].text = Cost[currentAbility].ToString("0");
                AbilityCooldownValue[currentAbility].text = Cooldown[currentAbility].ToString("0");
                //Rune1[currentAbility] = PlayerScript.Rune1[i];
                //Rune2[currentAbility] = PlayerScript.Rune2[i];
                currentAbility++;
            }
        }
        currentSpell = 0;
        for (int i = 0; i < PlayerScript.Spells.Length; i++)
        {
            if (PlayerScript.Spells[i])
            {
                Spell[currentSpell].SetActive(true);
                SpellIcon[currentSpell].sprite = Library.LightSpells[i].Icon;
                Spells[currentSpell] = i;
                //SpellLevel[currentSpell] = PlayerScript.Spells[i];
                ManaCost[currentSpell] = Library.LightSpells[i].ManaCost;
                CostIncrease[currentSpell] = Library.LightSpells[i].CostGain;
                SpellManaCostValue[currentSpell].text = ManaCost[currentSpell].ToString("0");
                currentSpell++;
            }
        }
    }

    void UpdateStats()
    {
        HealthBarFill.fillAmount = 1f * hitPoints / (maxHealth * 1f);
        HealthValue.text = hitPoints.ToString("0");
        if (shield > 0)
        {
            ShieldIcon.SetActive(true);
            ShieldValue.text = shield.ToString("0");
        }
        else ShieldIcon.SetActive(false);
        if (block > 0)
        {
            BlockIcon.SetActive(true);
            BlockValue.text = block.ToString("0");
        }
        else BlockIcon.SetActive(false);

        ActionsValue.text = actions.ToString("0");
        EnergyValue.text = energy.ToString("0");
        EnergyFill.fillAmount = (energy * 1f) / ((energyRequired + freeze) * 1f);
        ManaValue.text = manaPoints.ToString("0");
        ManaFill.fillAmount = (mana * 1f) / (5f);

        PlayerScript.HealthFill.fillAmount = (1f * hitPoints) / (1f * maxHealth);
        PlayerScript.CurrentHitPoints.text = hitPoints.ToString("0") + "/" + maxHealth.ToString("0");

        for (int i = 0; i < currentAbility; i++)
        {
            AbilityCooldownValue[i].text = Cooldown[i].ToString("0");
            CooldownFill[i].fillAmount = (Cooldown[i] * 1f) / (CooldownGain[i] * 1f);
        }

        BlastCostValue.text = blastCost.ToString("0");
        for (int i = 0; i < currentSpell; i++)
        {
            SpellManaCostValue[i].text = ManaCost[i].ToString("0");
        }

        for (int i = 0; i < StatusObjects.Length; i++)
        {
            StatusObjects[i].SetActive(false);
        }
        statusCount = 0;
        if (strength != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[0];
            statusEffects[statusCount] = 0;
            StatusValues[statusCount].text = strength.ToString("0");
            statusCount++;
        }
        if (resistance != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[1];
            statusEffects[statusCount] = 1;
            StatusValues[statusCount].text = resistance.ToString("0");
            statusCount++;
        }
        if (valor != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[2];
            statusEffects[statusCount] = 2;
            StatusValues[statusCount].text = valor.ToString("0");
            statusCount++;
        }
        if (bleed != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[3];
            statusEffects[statusCount] = 3;
            StatusValues[statusCount].text = bleed.ToString("0");
            statusCount++;
        }
        if (weak != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[4];
            statusEffects[statusCount] = 4;
            StatusValues[statusCount].text = weak.ToString("0");
            statusCount++;
        }
        if (vulnerable != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[5];
            statusEffects[statusCount] = 5;
            StatusValues[statusCount].text = vulnerable.ToString("0");
            statusCount++;
        }
        if (freeze != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[6];
            statusEffects[statusCount] = 6;
            StatusValues[statusCount].text = freeze.ToString("0");
            statusCount++;
        }
        if (storedBlock != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[7];
            statusEffects[statusCount] = 7;
            StatusValues[statusCount].text = storedBlock.ToString("0");
            statusCount++;
        }
        if (poison != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[8];
            statusEffects[statusCount] = 8;
            StatusValues[statusCount].text = poison.ToString("0");
            statusCount++;
        }
        if (armor != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[9];
            statusEffects[statusCount] = 9;
            StatusValues[statusCount].text = armor.ToString("0");
            statusCount++;
        }
        if (frail != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[10];
            statusEffects[statusCount] = 10;
            StatusValues[statusCount].text = frail.ToString("0");
            statusCount++;
        }
        if (dexterity != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[11];
            statusEffects[statusCount] = 11;
            StatusValues[statusCount].text = dexterity.ToString("0");
            statusCount++;
        }
        if (power != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[12];
            statusEffects[statusCount] = 12;
            StatusValues[statusCount].text = power.ToString("0");
            statusCount++;
        }
        if (impervious != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[13];
            statusEffects[statusCount] = 13;
            StatusValues[statusCount].text = impervious.ToString("0");
            statusCount++;
        }
        /*for (int i = 0; i < statusEffects.Length; i++)
        {
            if (statusEffects[i] != 0)
            {
                StatusObjects[statusCount].SetActive(true);
                StatusImages[statusCount].sprite = StatusSprites[i];
                StatusValues[statusCount].text = statusEffects[i].ToString("0");
                statusCount++;
            }
        }*/

        CheckAbilities();
    }

    public void StartTurn()
    {
        block = 0;
        if (storedBlock > 0)
            GainBlock(storedBlock);
        storedBlock = 0;
        if (vulnerable > 0)
            vulnerable--;
        GainAction(actionGain);
        GainEnergy(dexterity);
        GainManaPoint(2);
        GainMana(power);
        free = true;
        UpdateStats();
    }

    public void EndTurn()
    {
        //GainEnergy(actions * 3);
        actions = 0;
        if (armor > 0)
            GainBlock(armor);
        if (bleed > 0)
        {
            TakeDamage(bleed);
            if (PlayerScript.itemCheck[11])
                bleed--;
        }
        if (poison > 0)
        {
            TakeMagicDamage(poison);
            if (PlayerScript.itemCheck[11])
                poison--;
        }
        if (weak > 0)
            weak--;
        if (frail > 0)
            frail--;
        free = false;
        for (int i = 0; i < currentAbility; i++)
        {
            //Cooldown[i] -= 1 + Cooldown[i] / 10;
            Cooldown[i]--;
            if (Cooldown[i] < 0)
                Cooldown[i] = 0;
        }
        UpdateStats();
    }

    public void Switch()
    {
        if (spells)
        {
            spells = false;
            AbilitiesList.SetActive(true);
            SpellsList.SetActive(false);
            ListImage.sprite = AbilitySprite;
        }
        else
        {
            spells = true;
            AbilitiesList.SetActive(false);
            SpellsList.SetActive(true);
            ListImage.sprite = SpellSprite;
        }
        UpdateStats();
    }

    void GainAction(int amount)
    {
        actions += amount;

        UpdateStats();
    }

    void GainEnergy(int amount)
    {
        energy += amount;

        while (energy >= energyRequired + freeze)
        {
            energy -= energyRequired + freeze;
            actions++;
        }

        UpdateStats();
    }

    void GainManaPoint(int amount)
    {
        manaPoints += amount;

        UpdateStats();
    }

    void GainMana(int amount)
    {
        mana += amount;

        while (mana >= 5)
        {
            mana -= 5;
            GainManaPoint(1);
        }

        UpdateStats();
    }

    void RemoveCooldown(int amount)
    {
        while (amount > 0)
        {
            if (CurrentCooldown() > 0)
            {
                tempi = Random.Range(0, currentAbility);
                if (Cooldown[tempi] > 0)
                {
                    Cooldown[tempi]--;
                    amount--;
                }
            }
            else amount = 0;
        }
    }

    int CurrentCooldown()
    {
        tempi = 0;
        for (int i = 0; i < currentAbility; i++)
        {
            tempi += Cooldown[i];
        }
        return tempi;
    }

    void CheckAbilities()
    {
        if (free)
        {
            if (actions >= 1)
            {
                AttackButton.interactable = true;
                BlockButton.interactable = true;
            }
            else
            {
                AttackButton.interactable = false;
                BlockButton.interactable = false;
            }
            if (manaPoints >= blastCost)
                BlastButton.interactable = true;
            else BlastButton.interactable = false;

            for (int i = 0; i < currentAbility; i++)
            {
                if (Cooldown[i] > 0) AbilityButton[i].interactable = false;
                else
                {
                    if (actions >= Cost[i]) AbilityButton[i].interactable = true;
                    else AbilityButton[i].interactable = false;
                }
            }

            for (int i = 0; i < currentSpell; i++)
            {
                if (manaPoints >= ManaCost[i])
                    SpellButton[i].interactable = true;
                else SpellButton[i].interactable = false;
            }
        }
        else
        {
            AttackButton.interactable = false;
            BlockButton.interactable = false;
            for (int i = 0; i < currentAbility; i++)
            {
                AbilityButton[i].interactable = false;
            }
            for (int i = 0; i < currentSpell; i++)
            {
                SpellButton[i].interactable = false;
            }
        }
    }

    public int DamageDealtMultiplyer(int damage)
    {
        if (weak > 0)
            damage = (damage * 3) / 4;
        return damage;
    }

    public int DamageTakenMultiplyer(int damage)
    {
        if (vulnerable > 0)
            damage = (damage * 5) / 4;
        return damage;
    }

    void Display(int amount, Sprite sprite)
    {
        Origin.rotation = Quaternion.Euler(Origin.rotation.x, Origin.rotation.y, Body.rotation + Random.Range(-20f, 20f));
        GameObject display = Instantiate(DisplayObject, Origin.position, Origin.rotation);
        Displayed = display.GetComponent(typeof(Display)) as Display;
        Displayed.DisplayThis(amount, sprite);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * Random.Range(1.8f, 2.3f), ForceMode2D.Impulse);
    }

    public void TakeDamage(int amount)
    {
        Display(amount, DamageSprite);

        if (block > 0)
        {
            if (block > amount)
            {
                block -= amount;
                amount = 0;
            }
            else
            {
                amount -= block;
                block = 0;
            }
        }
        if (shield > 0 && amount > 0)
        {
            if (shield > amount)
            {
                shield -= amount;
                amount = 0;
            }
            else
            {
                amount -= shield;
                shield = 0;
            }
        }
        if (amount > 0)
        {
            hitPoints -= amount;
        }
        UpdateStats();
    }

    public void TakeMagicDamage(int amount)
    {
        Display(amount, MagicDamageSprite);
        hitPoints -= amount;
        UpdateStats();
    }

    void RestoreHealth(int amount)
    {
        Display(amount, HealthSprite);
        hitPoints += amount;
        if (hitPoints >= maxHealth)
            hitPoints = maxHealth;
    }

    void GainShield(int amount)
    {
        Display(amount, ShieldSprite);
        shield += amount;
        BlockGained(amount);
        UpdateStats();
    }

    void GainBlock(int amount)
    {
        if (frail > 0)
            amount = amount * 3 / 4;
        Display(amount, BlockSprite);
        block += amount;
        BlockGained(amount);
        UpdateStats();
    }

    void BlockGained(int amount)
    {
        blockGained += amount;
        while (blockGained >= 50)
        {
            blockGained -= 50;
            if (PlayerScript.itemCheck[18])
                GainStrength(1);
        }
    }

    public void GainBleed(int amount)
    {
        Display(amount, StatusSprites[3]);
        bleed += amount;
        UpdateStats();
    }

    public void GainPoison(int amount)
    {
        Display(amount, StatusSprites[8]);
        poison += amount;
        UpdateStats();
    }

    public void GainWeak(int amount)
    {
        if (amount > impervious)
        {
            amount -= impervious;
            impervious = 0;
            Display(amount, StatusSprites[4]);
            weak += amount;
        }
        else
        {
            impervious -= amount;
        }
        UpdateStats();
    }

    public void GainVulnerable(int amount)
    {
        if (amount > impervious)
        {
            amount -= impervious;
            impervious = 0;
            Display(amount, StatusSprites[5]);
            vulnerable += amount;
        }
        else
        {
            impervious -= amount;
        }
        UpdateStats();
    }

    public void GainFreeze(int amount)
    {
        if (amount > impervious)
        {
            amount -= impervious;
            impervious = 0;
            Display(amount, StatusSprites[6]);
            freeze += amount;
        }
        else
        {
            impervious -= amount;
        }
        UpdateStats();
    }

    public void GainFrail(int amount)
    {
        if (amount > impervious)
        {
            amount -= impervious;
            impervious = 0;
            Display(amount, StatusSprites[10]);
            frail += amount;
        }
        else
        {
            impervious -= amount;
        }
        UpdateStats();
    }

    public void GainArmor(int amount)
    {
        Display(amount, StatusSprites[9]);
        armor += amount;
        UpdateStats();
    }

    public void OnHit()
    {
        attacks++;

        if (attacks % 6 == 0)
        {
            if (PlayerScript.itemCheck[3])
                GainStrength(1);
            if (PlayerScript.itemCheck[17])
                GainResistance(1);
        }
    }

    public void Attack()
    {
        actions--;
        UsedAction();

        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(AttackDamage());
        OnHit();

        UpdateStats();

        free = false;
        Invoke("Unleash", 0.25f);
        UpdateStats();
    }

    int AttackDamage()
    {
        tempi = 10 + strength;
        return DamageDealtMultiplyer(tempi);
    }

    public void Block()
    {
        actions--;
        UsedAction();

        GainBlock(BlockBlock());

        free = false;
        Invoke("Unleash", 0.25f);
        UpdateStats();
    }

    int BlockBlock()
    {
        tempi = 8 + resistance;
        return tempi;
    }

    public void Blast()
    {
        manaPoints -= blastCost;
        blastCost++;

        CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(BlastDamage());

        UpdateStats();

        free = false;
        Invoke("Unleash", 0.25f);
        UpdateStats();
    }

    int BlastDamage()
    {
        tempi = 7 + strength;
        return DamageDealtMultiplyer(tempi);
    }

    public void ChooseAbility(int which)
    {
        UseAbility(Abilities[which], AbilityLevel[which]);
        actions -= Cost[which];
        UsedAction();
        Cooldown[which] += CooldownGain[which];
        free = false;
        Invoke("Unleash", 0.25f);
        UpdateStats();
    }

    public void ChooseSpell(int which)
    {
        CastSpell(Spells[which]);
        manaPoints -= ManaCost[which];
        ManaCost[which] += CostIncrease[which];
        free = false;
        Invoke("Unleash", 0.25f);
        UpdateStats();
    }

    void Unleash()
    {
        free = true;
        UpdateStats();
    }

    void UsedAction()
    {
        if (PlayerScript.itemCheck[16])
            GainMana(1);
    }

    void UseAbility(int which, int level)
    {
        switch (which)
        {
            case 0:
                SpearThrust(level);
                break;
            case 1:
                Judgement(level);
                break;
            case 2:
                BolaShot(level);
                break;
            case 3:
                CrushingBlow(level);
                break;
            case 4:
                Fortify(level);
                break;
            case 5:
                Empower(level);
                break;
            case 6:
                Inspire(level);
                break;
            case 7:
                ShieldBash(level);
                break;
            case 8:
                DesperateStand(level);
                break;
            case 9:
                RighteousHammer(level);
                break;
            case 10:
                DecisiveStrike(level);
                break;
            case 11:
                GoldenAegis(level);
                break;
            case 12:
                ShieldGlare(level);
                break;
            case 13:
                HolyLight(level);
                break;
            case 14:
                CripplingStrike(level);
                break;
        }
    }

    void CastSpell(int which)
    {
        switch (which)
        {
            case 0:
                Smite();
                break;
            case 1:
                SurgeOfLight();
                break;
            case 2:
                HolyBolt();
                break;
            case 3:
                Penance();
                break;
            case 4:
                BlindingLight();
                break;
            case 5:
                ShieldOfHope();
                break;
            case 6:
                LayOnHands();
                break;
            case 7:
                Consecration();
                break;
        }
    }

    public void AbilityHovered(bool basic, int which)
    {
        if (basic)
        {
            if (which == 0)
                AbilityTooltip.text = AttackDamage().ToString("0") + " Damage";
            else if (which == 1) AbilityTooltip.text = BlockBlock().ToString("0") + " Block";
            else AbilityTooltip.text = BlastDamage().ToString("0") + " Magic Damage";
        }
        else DisplayAbilityTooltip(Abilities[which], AbilityLevel[which]);
    }

    public void SpellHovered(int which)
    {
        DisplaySpellTooltip(Spells[which]);
    }

    public void Unhovered()
    {
        AbilityTooltip.text = "";
    }

    void DisplayAbilityTooltip(int which, int level)
    {
        switch (which)
        {
            case 0:
                AbilityTooltip.text = SpearThrustShieldBreak(level).ToString("0") + " Shield Removed \n" + SpearThrustDamage(level).ToString("0") + " Damage";
                break;
            case 1:
                AbilityTooltip.text = JudgementDamage(level).ToString("0") + " Damage";
                break;
            case 2:
                AbilityTooltip.text = BolaShotDamage(level).ToString("0") + " Damage \n" + BolaShotSlow(level).ToString("0") + " Slow";
                break;
            case 3:
                AbilityTooltip.text = CrushingBlowDamage(level).ToString("0") + " Damage \n" + CrushingBlowWeak(level).ToString("0") + " Weak \n" + CrushingBlowVulnerable(level).ToString("0") + " Vulnerable";
                break;
            case 4:
                AbilityTooltip.text = FortifyBlock(level).ToString("0") + " Block \n" + FortifyBlock(level).ToString("0") + " Block Next Turn";
                break;
            case 5:
                AbilityTooltip.text = "+ " + EmpowerAD(level).ToString("0") + " Strength \n + " + EmpowerEnergy(level).ToString("0") + " Energy";
                break;
            case 6:
                if (level == 5) AbilityTooltip.text = "+ " + InspireEnergy(level).ToString("0") + " Energy \n + 1 Dexterity";
                else AbilityTooltip.text = "+ " + InspireEnergy(level).ToString("0") + " Energy";
                break;
            case 7:
                AbilityTooltip.text = ShieldBashDamage(level).ToString("0") + " Damage \n" + ShieldBashSlow(level).ToString("0") + " Slow";
                break;
            case 8:
                AbilityTooltip.text = DesperateStandBlock(level).ToString("0") + " Block \n" + DesperateStandShield(level).ToString("0") + " Shield \n" + "- 1 Resistance";
                break;
            case 9:
                AbilityTooltip.text = RighteousHammerDamage(level).ToString("0") + " Damage \n" + RighteousHammerSlow(level).ToString("0") + " Slow \n to all Enemies";  
                break;
            case 10:
                AbilityTooltip.text = DecisiveStrikeDamage(level).ToString("0") + " Damage \n + " + DecisiveStrikeValor(level).ToString("0") + " Valor";
                break;
            case 11:
                AbilityTooltip.text = GoldenAegisBlock(level).ToString("0") + " Block \n" + GoldenAegisSlow(level).ToString("0") + " Slow \n to all Enemies";
                break;
            case 12:
                AbilityTooltip.text = ShieldGlareDamage(level).ToString("0") + " Magic Damage \n" + ShieldGlareWeak(level).ToString("0") + " Weak \n " + ShieldGlareBlock(level).ToString("0") + " Block";
                break;
            case 13:
                AbilityTooltip.text = HolyLightBlock(level).ToString("0") + " Block \n + " + HolyLightValor(level).ToString("0") + " Valor \n + " + HolyLightImpervious(level).ToString("0") + " Impervious";
                break;
            case 14:
                AbilityTooltip.text = CripplingStrikeDamage(level).ToString("0") + " Damage \n " + CripplingStrikeBleed(level).ToString("0") + " Bleed \n " + CripplingStrikeSlow(level).ToString("0") + " Slow";
                break;
        }
    }

    void DisplaySpellTooltip(int which)
    {
        switch (which)
        {
            case 0:
                AbilityTooltip.text = SmiteDamage().ToString("0") + " Magic Damage \n" + SmiteSlow().ToString("0") + " Slow";
                break;
            case 1:
                AbilityTooltip.text = "+ " + SurgeOfLightStrength().ToString("0") + " Strength \n" + SurgeOfLightResistance().ToString("0") + " Resistance \n" + SurgeOfLightRestore().ToString("0") + " Restored";
                break;
            case 2:
                AbilityTooltip.text = HolyBoltDamage().ToString("0") + " Damage \n" + HolyBoltMagicDamage().ToString("0") + " Magic Damage";
                break;
            case 3:
                AbilityTooltip.text = PenanceDamage().ToString("0") + " Magic Damage \n" + PenanceBlock().ToString("0") + " Block \n" + PenanceRestore().ToString("0") + " Restored";
                break;
            case 4:
                AbilityTooltip.text = BlindingLightDamage().ToString("0") + " Magic Damage \n" + BlindingLightWeak().ToString("0") + " Weak";
                break;
            case 5:
                AbilityTooltip.text = ShieldOfHopeBlock().ToString("0") + " Block \n" + ShieldOfHopeShield().ToString("0") + " Shield \n" + ShieldOfHopeArmor().ToString("0") + " Armor";
                break;
            case 6:
                AbilityTooltip.text = LayOnHandsBuff().ToString("0") + " Restore & Energy \n " + LayOnHandsValor().ToString("0") + " Valor \n" + LayOnHandsImpervious().ToString("0") + " Impervious";
                break;
            case 7:
                AbilityTooltip.text = ConsecrationDamage().ToString("0") + " Damage \n" + ConsecrationSlow().ToString("0") + " Slow \n to all Enemies";
                break;
        }
    }

    void GainStrength(int amount)
    {
        Display(amount, StatusSprites[0]);
        strength += amount;
        UpdateStats();
    }

    void GainResistance(int amount)
    {
        Display(amount, StatusSprites[1]);
        resistance += amount;
        UpdateStats();
    }

    void GainDexterity(int amount)
    {
        Display(amount, StatusSprites[11]);
        dexterity += amount;
        UpdateStats();
    }

    void GainPower(int amount)
    {
        Display(amount, StatusSprites[12]);
        power += amount;
        UpdateStats();
    }

    void GainValor(int amount)
    {
        Display(amount, StatusSprites[2]);
        valor += amount;
        UpdateStats();
    }

    void GainImpervious(int amount)
    {
        Display(amount, StatusSprites[13]);
        impervious += amount;
        UpdateStats();
    }

    // --- UMIEJKI ---
    void SpearThrust(int level)
    {
        if (PlayerScript.Rune2[0])
        {
            if (CombatScript.Enemy[CombatScript.targetedEnemy].ShieldAmount() > 0)
            {
                tempi = (SpearThrustShieldBreak(level) * 2) / 5;
                if (CombatScript.Enemy[CombatScript.targetedEnemy].ShieldAmount() < tempi)
                    GainBlock(CombatScript.Enemy[CombatScript.targetedEnemy].ShieldAmount());
                else GainBlock(tempi);
            }
        }
        CombatScript.Enemy[CombatScript.targetedEnemy].BreakShield(SpearThrustShieldBreak(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(SpearThrustDamage(level));
        OnHit();
    }

    int SpearThrustDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 14 + strength;
                break;
            case 2:
                tempi = 17 + strength;
                break;
            case 3:
                tempi = 19 + strength;
                break;
            case 4:
                tempi = 22 + strength;
                break;
            case 5:
                tempi = 27 + strength;
                break;
        }
        if (PlayerScript.Rune1[0])
            tempi += 1 + dexterity;
        return DamageDealtMultiplyer(tempi);
    }

    int SpearThrustShieldBreak(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 9 + strength;
                break;
            case 2:
                tempi = 12 + strength;
                break;
            case 3:
                tempi = 13 + 2 * strength;
                break;
            case 4:
                tempi = 15 + 2 * strength;
                break;
            case 5:
                tempi = 19 + 2 * strength;
                break;
        }
        return tempi;
    }

    void Judgement(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(JudgementDamage(level));
        if (PlayerScript.Rune1[1])
        {
            if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
                CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(2);
        }
        OnHit();
    }

    int JudgementDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 13 + strength;
                if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
                    tempi += 7;
                break;
            case 2:
                tempi = 16 + strength;
                if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
                    tempi += 9;
                break;
            case 3:
                tempi = 18 + strength;
                if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
                    tempi += 10 + strength;
                break;
            case 4:
                tempi = 20 + 2 * strength;
                if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
                    tempi += 13 + strength;
                break;
            case 5:
                tempi = 23 + 2 * strength;
                if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
                    tempi += 15 + strength;
                break;
        }
        if (PlayerScript.Rune2[1])
            tempi += 2 * CombatScript.Enemy[CombatScript.targetedEnemy].totalAttacks;
        return tempi;
    }

    void BolaShot(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(BolaShotDamage(level));
        OnHit();
        CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(BolaShotSlow(level));
        if (PlayerScript.Rune1[2])
            GainEnergy(2);
    }

    int BolaShotDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 12 + strength;
                break;
            case 2:
                tempi = 16 + strength;
                break;
            case 3:
                tempi = 18 + strength;
                break;
            case 4:
                tempi = 21 + strength;
                break;
            case 5:
                tempi = 22 + 2 * strength;
                break;
        }
        if (PlayerScript.Rune2[2])
            tempi += strength;
        return DamageDealtMultiplyer(tempi);
    }

    int BolaShotSlow(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 2;
                break;
            case 2:
                tempi = 3;
                break;
            case 3:
                tempi = 3;
                break;
            case 4:
                tempi = 4;
                break;
            case 5:
                tempi = 4;
                break;
        }
        if (PlayerScript.Rune2[2])
            tempi += 1;
        return tempi;
    }

    void CrushingBlow(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(CrushingBlowDamage(level));
        OnHit();
        CombatScript.Enemy[CombatScript.targetedEnemy].GainWeak(CrushingBlowWeak(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].GainVulnerable(CrushingBlowVulnerable(level));
        if (PlayerScript.Rune1[3])
            CombatScript.Enemy[CombatScript.targetedEnemy].GainBleed(4);
        if (PlayerScript.Rune2[3])
            CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(1 + strength / 4);
    }

    int CrushingBlowDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 16 + 2 * strength;
                break;
            case 2:
                tempi = 21 + 2 * strength;
                break;
            case 3:
                tempi = 23 + 2 * strength;
                break;
            case 4:
                tempi = 28 + 2 * strength;
                break;
            case 5:
                tempi = 29 + 3 * strength;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int CrushingBlowWeak(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 1;
                break;
            case 3:
                tempi = 2;
                break;
            case 4:
                tempi = 2;
                break;
            case 5:
                tempi = 3;
                break;
        }
        return tempi;
    }

    int CrushingBlowVulnerable(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 2;
                break;
            case 4:
                tempi = 3;
                break;
            case 5:
                tempi = 3;
                break;
        }
        return tempi;
    }

    void Fortify(int level)
    {
        GainBlock(FortifyBlock(level));
        storedBlock += FortifyBlock(level);
        if (PlayerScript.Rune1[4])
            GainResistance(1);
        if (PlayerScript.Rune2[4])
            GainValor(1);
    }

    int FortifyBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 12 + resistance;
                break;
            case 2:
                tempi = 15 + resistance;
                break;
            case 3:
                tempi = 17 + resistance;
                break;
            case 4:
                tempi = 21 + resistance;
                break;
            case 5:
                tempi = 24 + resistance;
                break;
        }
        if (PlayerScript.Rune2[4])
            tempi += 2 + valor;
        return tempi;
    }

    void Empower(int level)
    {
        GainStrength(EmpowerAD(level));
        GainEnergy(EmpowerEnergy(level));
        if (PlayerScript.Rune1[5])
            GainBlock(5 + resistance);
        if (PlayerScript.Rune2[5])
        {
            GainValor(1);
            GainMana(1);
        }
    }

    int EmpowerAD(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 2;
                break;
            case 4:
                tempi = 3;
                break;
            case 5:
                tempi = 3;
                break;
        }
        return tempi;
    }

    int EmpowerEnergy(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 5;
                break;
            case 2:
                tempi = 5;
                break;
            case 3:
                tempi = 6;
                break;
            case 4:
                tempi = 6;
                break;
            case 5:
                tempi = 7;
                break;
        }
        return tempi;
    }

    void Inspire(int level)
    {
        GainEnergy(InspireEnergy(level));
        if (level == 5) GainDexterity(1);
        if (PlayerScript.Rune2[6])
            GainValor(1);
    }

    int InspireEnergy(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 4;
                break;
            case 2:
                tempi = 5;
                break;
            case 3:
                tempi = 6;
                break;
            case 4:
                tempi = 8;
                break;
            case 5:
                tempi = 8;
                break;
        }
        if (PlayerScript.Rune1[6])
            tempi += 2;
        return tempi;
    }

    void ShieldBash(int level)
    {
        if (PlayerScript.Rune1[7])
            GainBlock(6 + resistance);
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(ShieldBashDamage(level));
        OnHit();
        CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(ShieldBashSlow(level));
    }

    int ShieldBashBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 15 + resistance;
                break;
            case 2:
                tempi = 19 + resistance;
                break;
            case 3:
                tempi = 24 + resistance;
                break;
        }
        return tempi;
    }

    int ShieldBashDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 6 + strength + (2 * (shield + block)) / 5;
                break;
            case 2:
                tempi = 8 + strength + (shield + block) / 2;
                break;
            case 3:
                tempi = 10 + strength + (3 * (shield + block)) / 5;
                break;
            case 4:
                tempi = 10 + strength + (4 * (shield + block)) / 5;
                break;
            case 5:
                tempi = 11 + strength + shield + block;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int ShieldBashSlow(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 1;
                break;
            case 3:
                tempi = 1;
                break;
            case 4:
                tempi = 2;
                break;
            case 5:
                tempi = 2;
                break;
        }
        if (PlayerScript.Rune2[7])
        {
            for (int i = 0; i < ShieldBashDamage(level) / 7; i += (i + 1))
            {
                tempi++;
            }
        }
        return tempi;
    }

    void DesperateStand(int level)
    {
        GainBlock(DesperateStandBlock(level));
        GainShield(DesperateStandShield(level));
        if (!PlayerScript.Rune2[8])
            GainResistance(-1);
    }

    int DesperateStandBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 18 + 2 * resistance;
                break;
            case 2:
                tempi = 19 + 2 * resistance;
                break;
            case 3:
                tempi = 23 + 2 * resistance;
                break;
            case 4:
                tempi = 25 + 2 * resistance;
                break;
            case 5:
                tempi = 27 + 3 * resistance;
                break;
        }
        if (PlayerScript.Rune1[8])
            tempi += 5;
        return tempi;
    }

    int DesperateStandShield(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 9 + resistance;
                break;
            case 2:
                tempi = 10 + resistance;
                break;
            case 3:
                tempi = 13 + resistance;
                break;
            case 4:
                tempi = 14 + resistance;
                break;
            case 5:
                tempi = 18 + resistance;
                break;
        }
        if (PlayerScript.Rune1[8])
            tempi += 5;
        return tempi;
    }

    void RighteousHammer(int level)
    {
        for (int i = 0; i < CombatScript.EnemyAlive.Length; i++)
        {
            if (CombatScript.EnemyAlive[i])
            {
                CombatScript.Enemy[i].TakeDamage(RighteousHammerDamage(level));
                CombatScript.Enemy[i].GainSlow(RighteousHammerSlow(level));
            }
            OnHit();
        }
        if (PlayerScript.Rune1[9])
            CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(HammerOfWrathDamage());
    }

    int RighteousHammerDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 19 + 2 * strength;
                break;
            case 2:
                tempi = 22 + 2 * strength;
                break;
            case 3:
                tempi = 26 + 2 * strength;
                break;
            case 4:
                tempi = 30 + 3 * strength;
                break;
            case 5:
                tempi = 33 + 3 * strength;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int RighteousHammerSlow(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 2;
                break;
            case 2:
                tempi = 3;
                break;
            case 3:
                tempi = 3;
                break;
            case 4:
                tempi = 3;
                break;
            case 5:
                tempi = 4;
                break;
        }
        if (PlayerScript.Rune2[9])
            tempi += 2;
        return tempi;
    }

    int HammerOfWrathDamage()
    {
        tempi = 8 + strength + power;
        return DamageDealtMultiplyer(tempi);
    }

    void DecisiveStrike(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(DecisiveStrikeDamage(level));
        if (PlayerScript.Rune2[10])
            CombatScript.Enemy[CombatScript.targetedEnemy].GainWeak(1);
        OnHit();
        if (PlayerScript.Rune1[10])
            GainMana(2 + valor / 3);
        GainValor(DecisiveStrikeValor(level));
    }

    int DecisiveStrikeDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 14 + strength + valor;
                break;
            case 2:
                tempi = 17 + strength + valor;
                break;
            case 3:
                tempi = 19 + strength + (13 * valor) / 10;
                break;
            case 4:
                tempi = 24 + strength + (7 * valor) / 4;
                break;
            case 5:
                tempi = 25 + strength + 2 * valor;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int DecisiveStrikeValor(int level)
    {
        if (level < 5)
            tempi = 1;
        else tempi = 2;
        return tempi;
    }

    void GoldenAegis(int level)
    {
        GainBlock(GoldenAegisBlock(level));
        for (int i = 0; i < CombatScript.EnemyAlive.Length; i++)
        {
            if (CombatScript.EnemyAlive[i])
                CombatScript.Enemy[i].GainSlow(GoldenAegisSlow(level));
        }
    }

    int GoldenAegisBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 16 + resistance;
                break;
            case 2:
                tempi = 20 + resistance;
                break;
            case 3:
                tempi = 21 + resistance;
                break;
            case 4:
                tempi = 22 + resistance;
                break;
            case 5:
                tempi = 27 + 2 * resistance;
                break;
        }
        if (PlayerScript.Rune1[11])
        {
            for (int i = 0; i < CombatScript.EnemyAlive.Length; i++)
            {
                if (CombatScript.EnemyAlive[i])
                    tempi += 4 + resistance;
            }
        }
        return tempi;
    }

    int GoldenAegisSlow(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 1;
                break;
            case 3:
                tempi = 1;
                break;
            case 4:
                tempi = 2;
                break;
            case 5:
                tempi = 2;
                break;
        }
        if (PlayerScript.Rune2[11])
            tempi++;
        return tempi;
    }

    void ShieldGlare(int level)
    {
        if (PlayerScript.Rune1[12])
        {
            for (int i = 0; i < CombatScript.EnemyAlive.Length; i++)
            {
                if (CombatScript.EnemyAlive[i])
                {
                    CombatScript.Enemy[i].TakeMagicDamage(ShieldGlareDamage(level));
                    CombatScript.Enemy[i].GainWeak(ShieldGlareWeak(level));
                }
            }
        }
        else
        {
            CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(ShieldGlareDamage(level));
            CombatScript.Enemy[CombatScript.targetedEnemy].GainWeak(ShieldGlareWeak(level));
        }
        GainBlock(ShieldGlareBlock(level));
    }

    int ShieldGlareDamage(int level)
    {
        tempi = 5 + level;
        return DamageDealtMultiplyer(tempi);
    }

    int ShieldGlareWeak(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 1;
                break;
            case 3:
                tempi = 1;
                break;
            case 4:
                tempi = 2;
                break;
            case 5:
                tempi = 2;
                break;
        }
        return tempi;
    }

    int ShieldGlareBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 9 + resistance;
                break;
            case 2:
                tempi = 12 + resistance;
                break;
            case 3:
                tempi = 14 + resistance;
                break;
            case 4:
                tempi = 15 + resistance;
                break;
            case 5:
                tempi = 18 + resistance;
                break;
        }
        if (PlayerScript.Rune2[12])
        {
            if (PlayerScript.Rune1[12])
            {
                for (int i = 0; i < CombatScript.EnemyAlive.Length; i++)
                {
                    if (CombatScript.EnemyAlive[i])
                        tempi += CombatScript.Enemy[i].currentAttackDamage / 5;
                }
            }
            else
                tempi += CombatScript.Enemy[CombatScript.targetedEnemy].currentAttackDamage / 5;
        }
        return tempi;
    }

    void HolyLight(int level)
    {
        GainBlock(HolyLightBlock(level));
        GainValor(HolyLightValor(level));
        GainImpervious(HolyLightImpervious(level));
        if (PlayerScript.Rune1[13])
            RestoreHealth(4);
    }

    int HolyLightBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 12 + resistance;
                break;
            case 2:
                tempi = 13 + resistance;
                break;
            case 3:
                tempi = 16 + resistance;
                break;
            case 4:
                tempi = 18 + resistance;
                break;
            case 5:
                tempi = 20 + resistance;
                break;
        }
        if (PlayerScript.Rune2[13])
            tempi += 2 + valor / 2;
        return tempi;
    }

    int HolyLightValor(int level)
    {
        if (level < 5)
            tempi = 1;
        else tempi = 2;
        return tempi;
    }


    int HolyLightImpervious(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 2;
                break;
            case 4:
                tempi = 3;
                break;
            case 5:
                tempi = 3;
                break;
        }
        return tempi;
    }

    void CripplingStrike(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(CripplingStrikeDamage(level));
        OnHit();
        CombatScript.Enemy[CombatScript.targetedEnemy].GainBleed(CripplingStrikeBleed(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(CripplingStrikeSlow(level));
        if (PlayerScript.Rune2[14])
            GainEnergy(3 + dexterity / 3);
    }

    int CripplingStrikeDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 11 + strength;
                break;
            case 2:
                tempi = 12 + strength;
                break;
            case 3:
                tempi = 14 + strength;
                break;
            case 4:
                tempi = 16 + strength;
                break;
            case 5:
                tempi = 18 + strength;
                break;
        }
        if (PlayerScript.Rune1[14])
        {
            if (CombatScript.Enemy[CombatScript.targetedEnemy].maxHealth > CombatScript.Enemy[CombatScript.targetedEnemy].hitPoints * 2)
                tempi += 9;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int CripplingStrikeBleed(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 2;
                break;
            case 2:
                tempi = 3;
                break;
            case 3:
                tempi = 3 + strength / 3;
                break;
            case 4:
                tempi = 4 + (2 * strength) / 3;
                break;
            case 5:
                tempi = 4 + strength;
                break;
        }
        if (PlayerScript.Rune1[14])
        {
            if (CombatScript.Enemy[CombatScript.targetedEnemy].maxHealth <= CombatScript.Enemy[CombatScript.targetedEnemy].hitPoints * 2)
                tempi += 2;
        }
        return tempi;
    }

    int CripplingStrikeSlow(int level)
    {
        if (level < 5)
            tempi = 1;
        else tempi = 2;
        return tempi;
    }

    // SPELLE ---
    void Smite()
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(SmiteDamage());
        CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(SmiteSlow());
    }

    int SmiteDamage()
    {
        tempi = 15 + strength + playerLevel;
        if (CombatScript.Enemy[CombatScript.targetedEnemy].DebuffCount() > 0)
            tempi += 5 + (4 * playerLevel) / 3;
        return DamageDealtMultiplyer(tempi);
    }

    int SmiteSlow()
    {
        tempi = 1 + playerLevel / 4;
        return tempi;
    }

    void SurgeOfLight()
    {
        GainStrength(SurgeOfLightStrength());
        GainResistance(SurgeOfLightResistance());
        RestoreHealth(SurgeOfLightRestore());
    }

    int SurgeOfLightStrength()
    {
        tempi = 1 + (2 * playerLevel + 2) / 5;
        return tempi;
    }

    int SurgeOfLightResistance()
    {
        tempi = 1 + (2 * playerLevel) / 5;
        return tempi;
    }

    int SurgeOfLightRestore()
    {
        tempi = 5 + (2 * playerLevel) / 3;
        return tempi;
    }

    void HolyBolt()
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(HolyBoltDamage());
        OnHit();
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(HolyBoltMagicDamage());
    }

    int HolyBoltDamage()
    {
        tempi = 18 + playerLevel + strength;
        return DamageDealtMultiplyer(tempi);
    }

    int HolyBoltMagicDamage()
    {
        tempi = 8 + playerLevel + strength + ((5 + playerLevel) * valor) / 5;
        return DamageDealtMultiplyer(tempi);
    }

    void Penance()
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(PenanceDamage());
        RestoreHealth(PenanceRestore());
        GainBlock(PenanceBlock());
    }

    int PenanceDamage()
    {
        tempi = 22 + strength + (11 * playerLevel) / 8;
        return DamageDealtMultiplyer(tempi);
    }

    int PenanceRestore()
    {
        tempi = 5 + (2 * playerLevel) / 3;
        return tempi;
    }

    int PenanceBlock()
    {
        tempi = 9 + resistance + playerLevel;
        return tempi;
    }

    void BlindingLight()
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(BlindingLightDamage());
        CombatScript.Enemy[CombatScript.targetedEnemy].GainWeak(BlindingLightWeak());
    }

    int BlindingLightDamage()
    {
        tempi = 21  + strength + (25 * playerLevel) / 17;
        return DamageDealtMultiplyer(tempi);
    }

    int BlindingLightWeak()
    {
        tempi = 2 + playerLevel / 3;
        return tempi;
    }

    void ShieldOfHope()
    {
        GainBlock(ShieldOfHopeBlock());
        GainShield(ShieldOfHopeShield());
        GainArmor(ShieldOfHopeArmor());
    }

    int ShieldOfHopeBlock()
    {
        tempi = 26 + 2 * resistance + (5 * playerLevel) / 4;
        return tempi;
    }

    int ShieldOfHopeShield()
    {
        tempi = 8 + (4 * playerLevel) / 5;
        return tempi;
    }

    int ShieldOfHopeArmor()
    {
        tempi = 1 + (playerLevel + 2) / 5;
        return tempi;
    }

    void LayOnHands()
    {
        RestoreHealth(LayOnHandsBuff());
        GainEnergy(LayOnHandsBuff());
        GainValor(LayOnHandsValor());
        GainImpervious(LayOnHandsImpervious());
    }

    int LayOnHandsBuff()
    {
        tempi = 3 + playerLevel / 2 + ((4 + playerLevel) * valor) / 7;
        return tempi;
    }

    int LayOnHandsValor()
    {
        tempi = 4 + (4 * playerLevel) / 9;
        return tempi;
    }

    int LayOnHandsImpervious()
    {
        tempi = 2 + playerLevel / 3;
        return tempi;
    }

    void Consecration()
    {
        for (int i = 0; i < CombatScript.EnemyAlive.Length; i++)
        {
            if (CombatScript.EnemyAlive[i])
            {
                CombatScript.Enemy[i].TakeMagicDamage(ConsecrationDamage());
                CombatScript.Enemy[i].GainSlow(ConsecrationSlow());
            }
        }
    }

    int ConsecrationDamage()
    {
        tempi = 16 + (7 * playerLevel) / 6 + strength + ((5 + playerLevel) * power) / 5;
        return DamageDealtMultiplyer(tempi);
    }

    int ConsecrationSlow()
    {
        tempi = 2 + playerLevel / 3;
        return tempi;
    }

    // OLD
    void CounterAttack(int level)
    {
        if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
            GainStrength(CounterAttackStrength(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(CounterAttackDamage(level));
        OnHit();
    }

    int CounterAttackStrength(int level)
    {
        if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
        {
            switch (level)
            {
                case 1:
                    tempi = 1;
                    break;
                case 2:
                    tempi = 2;
                    break;
                case 3:
                    tempi = 2;
                    break;
            }
        }
        else tempi = 0;
        return tempi;
    }

    int CounterAttackDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 15 + strength;
                break;
            case 2:
                tempi = 18 + strength;
                break;
            case 3:
                tempi = 22 + strength;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    void BulwardOfLight(int level)
    {
        GainBlock(BulwardOfLightBlock(level));
    }

    int BulwardOfLightBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 12 + resistance;
                tempi += CombatScript.enemyCount * 2;
                break;
            case 2:
                tempi = 14 + resistance;
                tempi += CombatScript.enemyCount * 4;
                break;
            case 3:
                tempi = 16 + resistance;
                tempi += CombatScript.enemyCount * 6;
                break;
        }
        return tempi;
    }

    void Courage(int level)
    {
        GainBlock(CourageBlock(level));
        GainValor(CourageBlockValor(level));
    }

    int CourageBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 15 + resistance + valor;
                break;
            case 2:
                tempi = 17 + resistance + valor;
                break;
            case 3:
                tempi = 22 + resistance + valor;
                break;
        }
        return tempi;
    }

    int CourageBlockValor(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 2;
                break;
        }
        return tempi;
    }

    void Punish(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(PunishDamage(level));
        OnHit();
        if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
            CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(PunishSlow(level));
    }

    int PunishDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 16 + strength;
                break;
            case 2:
                tempi = 19 + strength;
                break;
            case 3:
                tempi = 23 + strength;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int PunishSlow(int level)
    {
        if (CombatScript.Enemy[CombatScript.targetedEnemy].attackIntention)
        {
            switch (level)
            {
                case 1:
                    tempi = 2;
                    break;
                case 2:
                    tempi = 3;
                    break;
                case 3:
                    tempi = 4;
                    break;
            }
        }
        else tempi = 0;
        return tempi;
    }

    void Retaliate(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeDamage(RetaliateDamage(level));
        OnHit();
    }

    int RetaliateDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 18 + strength + 3 * CombatScript.Enemy[CombatScript.targetedEnemy].totalAttacks;
                break;
            case 2:
                tempi = 22 + strength + 4 * CombatScript.Enemy[CombatScript.targetedEnemy].totalAttacks;
                break;
            case 3:
                tempi = 27 + strength + 5 * CombatScript.Enemy[CombatScript.targetedEnemy].totalAttacks;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    void Chastise(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(ChastiseDamage(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(ChastiseSlow(level));
    }

    int ChastiseDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 20 + strength;
                break;
            case 2:
                tempi = 24 + strength;
                break;
            case 3:
                tempi = 29 + 2 * strength;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }

    int ChastiseSlow(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 2 + valor / 4;
                break;
            case 2:
                tempi = 2 + valor / 3;
                break;
            case 3:
                tempi = 3 + valor / 3;
                break;
        }
        return tempi;
    }

    void ShieldWall(int level)
    {
        GainResistance(ShieldWallResistance(level));
        GainBlock(ShieldWallBlock(level));
    }

    int ShieldWallResistance(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 2;
                break;
        }
        return tempi;
    }

    int ShieldWallBlock(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 18 + resistance;
                break;
            case 2:
                tempi = 21 + resistance;
                break;
            case 3:
                tempi = 26 + 2 * resistance;
                break;
        }
        return tempi;
    }

    void JudgeUnworthy(int level)
    {
        CombatScript.Enemy[CombatScript.targetedEnemy].GainWeak(JudgeUnworthyDebuff(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].GainVulnerable(JudgeUnworthyDebuff(level));
        CombatScript.Enemy[CombatScript.targetedEnemy].GainSlow(JudgeUnworthySlow(level));
        if (CombatScript.Enemy[CombatScript.targetedEnemy].totalDebuffs > 0)
            CombatScript.Enemy[CombatScript.targetedEnemy].TakeMagicDamage(JudgeUnworthyDamage(level));
    }

    int JudgeUnworthyDebuff(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 1;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 2;
                break;
        }
        return tempi;
    }

    int JudgeUnworthySlow(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 2;
                break;
            case 2:
                tempi = 2;
                break;
            case 3:
                tempi = 3;
                break;
        }
        return tempi;
    }

    int JudgeUnworthyDamage(int level)
    {
        switch (level)
        {
            case 1:
                tempi = 3 * CombatScript.Enemy[CombatScript.targetedEnemy].totalDebuffs;
                break;
            case 2:
                tempi = 3 * CombatScript.Enemy[CombatScript.targetedEnemy].totalDebuffs;
                break;
            case 3:
                tempi = 4 * CombatScript.Enemy[CombatScript.targetedEnemy].totalDebuffs;
                break;
        }
        return DamageDealtMultiplyer(tempi);
    }
}