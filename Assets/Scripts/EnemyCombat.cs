using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour
{
    public Combat CombatScript;
    public EnemiesLibrary Library;

    [Header("Stats")]
    public int Type;
    public int order, level;
    public int power, maxHealth, hitPoints, shield, block, statusCount, totalAttacks, totalDebuffs;
    public int boneClaws, flySwarm, rot, hollow, tenacity, impervious, strength, resistance, bleed, weak, vulnerable, slow, stun;
    public int[] statusEffects;
    float temp;
    public int currentAttackDamage;
    int tempi;
    public bool dead;

    [Header("UI")]
    public GameObject TargetArrow;
    public GameObject ShieldIcon, BlockIcon;
    public Image HealthBarFill, IntentionImage, UnitImage;
    public TMPro.TextMeshProUGUI HealthValue, ShieldValue, BlockValue, IntentionValue;
    public GameObject[] StatusObjects;
    public Image[] StatusImages;
    public TMPro.TextMeshProUGUI[] StatusValues;
    public Sprite[] StatusSprites;
    public Sprite StunIcon;

    [Header("Display")]
    public GameObject DisplayObject;
    public Rigidbody2D Body;
    public Transform Origin;
    public Display Displayed;
    public Sprite DamageSprite, MagicDamageSprite, HealthSprite, BlockSprite, ShieldSprite;

    [Header("Moves")]
    public bool attackIntention;
    public int moves, currentMove, lastMove;
    public int[] moveWeight;
    public int roll;

    public void SetType(int enemyType)
    {
        Type = enemyType;
        UnitImage.sprite = Library.EnemiesInfo[Type].UnitSprite;
        maxHealth = Random.Range(Library.EnemiesInfo[Type].MinHealth, Library.EnemiesInfo[Type].MaxHealth + 1);
        shield = Random.Range(Library.EnemiesInfo[Type].MinShield, Library.EnemiesInfo[Type].MaxShield + 1);
        tenacity = Library.EnemiesInfo[Type].Tenacity;
        moves = Library.EnemiesInfo[Type].MovesCount;
        for (int i = 0; i < moves; i++)
        {
            moveWeight[i] = Library.EnemiesInfo[Type].MovesChances[i];
        }
        /*for (int i = 0; i < statusEffects.Length; i++)
        {
            statusEffects[i] = Library.EnemiesInfo[Type].StatusEffects[i];
        }*/
        power = 0;
        boneClaws = 0; flySwarm = 0; rot = 0; hollow = 0; strength = 0; bleed = 0; weak = 0; vulnerable = 0; slow = 0; stun = 0;
        totalAttacks = 0; totalDebuffs = 0; impervious = 0;

        lastMove = 100;
        switch (Type)
        {
            case 0:
                for (int i = 0; i < level; i++)
                {
                    maxHealth += 3;
                    shield += Random.Range(3, 7);
                    power += 2;
                    roll = Random.Range(0, 4);
                    switch (roll)
                    {
                        case 0:
                            strength += 2;
                            break;
                        case 1:
                            //armor += 2;
                            break;
                        case 2:
                            power += 3;
                            break;
                        case 3:
                            shield += Random.Range(7, 11);
                            tenacity++;
                            break;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < level; i++)
                {
                    maxHealth += Random.Range(8, 12);
                    power += 2;
                    roll = Random.Range(0, 3);
                    switch (roll)
                    {
                        case 0:
                            maxHealth += Random.Range(11, 14);
                            break;
                        case 1:
                            power += 3;
                            break;
                        case 2:
                            maxHealth += Random.Range(5, 14);
                            tenacity++;
                            break;
                    }
                }
                boneClaws = 1;
                break;
            case 2:
                for (int i = 0; i < level; i++)
                {
                    maxHealth += Random.Range(8, 17);
                    power += 2;
                    roll = Random.Range(0, 4);
                    switch (roll)
                    {
                        case 0:
                            maxHealth += Random.Range(11, 20);
                            break;
                        case 1:
                            power += 3;
                            break;
                        case 2:
                            maxHealth += Random.Range(3, 14);
                            //regen += 2;
                            break;
                        case 3:
                            maxHealth += Random.Range(7, 14);
                            power++;
                            break;
                    }
                }
                flySwarm = 1;
                break;
            case 3:
                for (int i = 0; i < level; i++)
                {
                    maxHealth += Random.Range(9, 20);
                    power += 2;
                    roll = Random.Range(0, 2);
                    switch (roll)
                    {
                        case 0:
                            maxHealth += Random.Range(13, 18);
                            tenacity++;
                            break;
                        case 1:
                            power += 3;
                            break;
                    }
                }
                rot = 1;
                break;
            case 4:
                for (int i = 0; i < level; i++)
                {
                    maxHealth += Random.Range(17, 23);
                    power += 2;
                }
                hollow = 1;
                lastMove = 1;
                break;
        }
        hitPoints = maxHealth;
        //shield = 0;
        block = 0;
        ItemChecks();
        dead = false;
        currentMove = 0;
        currentMove = 0;
        UpdateStats();
    }

    void ItemChecks()
    {
        if (CombatScript.PlayerScript.itemCheck[8])
            GainWeak(1);
        if (CombatScript.PlayerScript.itemCheck[9])
            GainVulnerable(1);
        if (CombatScript.PlayerScript.itemCheck[10])
            GainSlow(3);
        if (CombatScript.PlayerScript.itemCheck[20])
        {
            tenacity += 2;
            impervious += 2;
        }
    }

    void UpdateStats()
    {
        HealthBarFill.fillAmount = 1f * hitPoints / maxHealth;
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

        for (int i = 0; i < StatusObjects.Length; i++)
        {
            StatusObjects[i].SetActive(false);
        }
        statusCount = 0;
        if (boneClaws != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[0];
            statusEffects[statusCount] = 0;
            StatusValues[statusCount].text = boneClaws.ToString("0");
            statusCount++;
        }
        if (tenacity != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[1];
            statusEffects[statusCount] = 1;
            StatusValues[statusCount].text = tenacity.ToString("0");
            statusCount++;
        }
        if (strength != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[2];
            statusEffects[statusCount] = 2;
            StatusValues[statusCount].text = strength.ToString("0");
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
        if (slow != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[6];
            statusEffects[statusCount] = 6;
            StatusValues[statusCount].text = slow.ToString("0");
            statusCount++;
        }
        if (stun != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[7];
            statusEffects[statusCount] = 7;
            StatusValues[statusCount].text = stun.ToString("0");
            statusCount++;
        }
        if (flySwarm != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[8];
            statusEffects[statusCount] = 8;
            StatusValues[statusCount].text = flySwarm.ToString("0");
            statusCount++;
        }
        if (rot != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[9];
            statusEffects[statusCount] = 9;
            StatusValues[statusCount].text = "";
            statusCount++;
        }
        if (hollow != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[10];
            statusEffects[statusCount] = 10;
            StatusValues[statusCount].text = "";
            statusCount++;
        }
        if (impervious != 0)
        {
            StatusObjects[statusCount].SetActive(true);
            StatusImages[statusCount].sprite = StatusSprites[11];
            statusEffects[statusCount] = 11;
            StatusValues[statusCount].text = impervious.ToString("0");
            statusCount++;
        }
        /*for (int i = 0; i < statusEffects.Length; i++)
        {
            if (statusEffects[i] > 0)
            {
                StatusObjects[statusCount].SetActive(true);
                StatusImages[statusCount].sprite = StatusSprites[i];
                StatusValues[statusCount].text = statusEffects[i].ToString("0");
                statusCount++;
            }
        }*/
        if (stun <= 0 && attackIntention)
            DisplayMove(currentMove);
    }

    public void StartTurn()
    {
        if (weak > 0)
            weak--;
        if (vulnerable > 0)
            vulnerable--;
        do
        {
            roll = Random.Range(1, 101);
            currentMove = 0;
            while (roll > moveWeight[currentMove])
            {
                roll -= moveWeight[currentMove];
                currentMove++;
            }
        } while (currentMove == lastMove);

        lastMove = currentMove;
        if (stun < 1)
            SetMove(currentMove);
        UpdateStats();
    }

    public void EndTurn()
    {
        if (!dead)
        {
            if (bleed > 0)
                TakeDamage(bleed);
            if (flySwarm > 0)
                CombatScript.Player.TakeMagicDamage(CombatScript.Player.DamageTakenMultiplyer(flySwarm));
            block = 0;

            if (stun > 0)
                stun--;
            else
            {
                if (!dead)
                    ExecuteMove(currentMove);
            }

            if (rot > 0)
            {
                GainStrength(2 + (strength * 2 + power * 3) / 24);
                GainSlow(1);
            }
            UpdateStats();
        }
    }

    void SetMove(int which)
    {
        IntentionImage.sprite = Library.EnemiesInfo[Type].MovesSprite[currentMove];
        currentAttackDamage = Library.EnemiesInfo[Type].MovesValues[currentMove];
        attackIntention = Library.EnemiesInfo[Type].attackIntention[currentMove];
        if (attackIntention)
            DisplayMove(currentMove);
        else IntentionValue.text = "";
    }

    void ExecuteMove(int which)
    {
        switch (Type)
        {
            case 0:
                switch (which)
                {
                    case 0:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Risen_Strike_Damage()));
                        OnHit();
                        break;
                    case 1:
                        Risen_Block();
                        break;
                    case 2:
                        Risen_Enrage();
                        break;
                }
                break;
            case 1:
                switch (which)
                {
                    case 0:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Ghoul_Only_Purpose_Damage()));
                        OnHit();
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Ghoul_Only_Purpose_Damage()));
                        OnHit();
                        break;
                    case 1:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Ghoul_Slash_Damage()));
                        OnHit();
                        CombatScript.Player.GainBleed(Ghoul_Slash_Bleed());
                        break;
                    case 2:
                        if (CombatScript.Player.DamageTakenMultiplyer(Ghoul_Cannibalize_Damage()) > CombatScript.Player.block + CombatScript.Player.shield)
                            RestoreHealth(CombatScript.Player.DamageTakenMultiplyer(Ghoul_Cannibalize_Damage()) - CombatScript.Player.block - CombatScript.Player.shield);
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Ghoul_Cannibalize_Damage()));
                        OnHit();
                        break;
                }
                break;
            case 2:
                switch (which)
                {
                    case 0:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Husk_Strike_Damage()));
                        OnHit();
                        break;
                    case 1:
                        Husk_Flies_Nest();
                        UpdateStats();
                        break;
                    case 2:
                        CombatScript.Player.TakeMagicDamage(CombatScript.Player.DamageTakenMultiplyer(Husk_Haunting_Wail_Damage()));
                        CombatScript.Player.GainWeak(1);
                        CombatScript.Player.GainFreeze(2);
                        // sanity drop
                        OnHit();
                        break;
                }
                break;
            case 3:
                switch (which)
                {
                    case 0:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Rotfiend_Tear_Flesh_Damage()));
                        OnHit();
                        RestoreHealth(Rotfiend_Tear_Flesh_Restore());
                        break;
                    case 1:
                        CombatScript.Player.TakeMagicDamage(CombatScript.Player.DamageTakenMultiplyer(Rotfiend_Decay_Damage()));
                        CombatScript.Player.GainPoison(1 + power / 25);
                        break;
                    case 2:
                        CombatScript.Player.GainWeak(1);
                        CombatScript.Player.GainFreeze(Rotfiend_Wither_Freeze());
                        CombatScript.Player.GainPoison(1);
                        break;
                    case 3:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Rotfiend_Strike_Damage()));
                        OnHit();
                        break;
                }
                break;
            case 4:
                switch (which)
                {
                    case 0:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Shambler_Eye_for_adn_Eye_Damage()));
                        CombatScript.Player.GainBleed(Shambler_Eye_for_adn_Eye_Bleed());
                        OnHit();
                        break;
                    case 1:
                        Shambler_Flesh_Hide();
                        break;
                    case 2:
                        Shambler_Empty_Stare();
                        break;
                    case 3:
                        CombatScript.Player.TakeDamage(CombatScript.Player.DamageTakenMultiplyer(Shambler_No_Regrets_Damage()));
                        OnHit();
                        GainSlow(1);
                        break;
                }
                break;
        }
    }

    void DisplayMove(int which)
    {
        switch (Type) // IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer().ToString("0");
        {
            case 0:
                switch (which)
                {
                    case 0:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Risen_Strike_Damage()).ToString("0");
                        break;
                }
                break;
            case 1:
                switch (which)
                {
                    case 0:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Ghoul_Only_Purpose_Damage()).ToString("0") + "x2";
                        break;
                    case 1:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Ghoul_Slash_Damage()).ToString("0");
                        break;
                    case 2:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Ghoul_Cannibalize_Damage()).ToString("0");
                        break;
                }
                break;
            case 2:
                switch (which)
                {
                    case 0:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Husk_Strike_Damage()).ToString("0");
                        break;
                    case 2:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Husk_Haunting_Wail_Damage()).ToString("0");
                        break;
                }
                break;
            case 3:
                switch (which)
                {
                    case 0:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Rotfiend_Tear_Flesh_Damage()).ToString("0");
                        break;
                    case 1:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Rotfiend_Decay_Damage()).ToString("0");
                        break;
                    case 3:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Rotfiend_Strike_Damage()).ToString("0");
                        break;
                }
                break;
            case 4:
                switch (which)
                {
                    case 0:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Shambler_Eye_for_adn_Eye_Damage()).ToString("0");
                        break;
                    case 3:
                        IntentionValue.text = CombatScript.Player.DamageTakenMultiplyer(Shambler_No_Regrets_Damage()).ToString("0");
                        break;
                }
                break;
        }
    }

    void OnHit()
    {
        if (boneClaws > 0)
            CombatScript.Player.GainBleed(boneClaws);
        totalAttacks++;
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

    void RestoreHealth(int amount)
    {
        Display(amount, HealthSprite);
        hitPoints += amount;
        if (hitPoints > maxHealth)
            hitPoints = maxHealth;
        UpdateStats();
    }

    void GainBlock(int amount)
    {
        Display(amount, BlockSprite);
        block += amount;
        UpdateStats();
    }

    void GainShield(int amount)
    {
        Display(amount, ShieldSprite);
        shield += amount;
        UpdateStats();
    }

    public void TakeDamage(int amount)
    {
        amount = DamageTakenMultiplyer(amount);

        if (hollow > 0)
            GainBleed(1);

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
            if (hitPoints <= 0)
                Death();
        }
        UpdateStats();
    }

    public void TakeMagicDamage(int amount)
    {
        Display(amount, MagicDamageSprite);
        hitPoints -= amount;
        if (hitPoints <= 0)
            Death();
        UpdateStats();
    }

    void Death()
    {
        CombatScript.EnemyDeath(order);
        dead = true;
    }

    int DamageTakenMultiplyer(int damage)
    {
        if (vulnerable > 0)
            damage = (damage * 5) / 4;
        if (hollow > 0)
            damage -= damage / 3;
        return damage;
    }

    int DamageDealtMultiplyer(int damage)
    {
        damage += strength;
        if (damage < 0)
            damage = 0;
        if (weak > 0)
            damage = (damage * 3) / 4;
        return damage;
    }

    public void BreakShield(int amount)
    {
        if (block > 0)
        {
            if (block > amount)
                block -= amount;
            else
            {
                amount -= block;
                block = 0;
                if (shield > 0)
                {
                    if (shield > amount)
                        shield -= amount;
                    else shield = 0;
                }
            }
        }
        else if (shield > 0)
        {
            if (shield > amount)
                shield -= amount;
            else shield = 0;
        }
        UpdateStats();
    }

    public void GainStrength(int amount)
    {
        Display(amount, StatusSprites[2]);
        strength += amount;
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
            GainDebuff(amount);
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
            GainDebuff(amount);
        }
        else
        {
            impervious -= amount;
        }
        UpdateStats();
    }

    public void GainSlow(int amount)
    {
        if (amount > impervious)
        {
            amount -= impervious;
            impervious = 0;
            Display(amount, StatusSprites[6]);
            slow += amount;
            if (slow >= 5 + tenacity)
            {
                slow -= 5 + tenacity;
                GainStun(1);
            }
            //GainDebuff(amount);
        }
        else
        {
            impervious -= amount;
        }
        UpdateStats();
    }

    public void GainStun(int amount)
    {
        Display(amount, StatusSprites[7]);
        tenacity += amount;
        stun += amount;
        GainDebuff(amount);
        IntentionImage.sprite = StunIcon;
        IntentionValue.text = "";
        UpdateStats();
    }

    public void GainBleed(int amount)
    {
        Display(amount, StatusSprites[3]);
        bleed += amount;
        UpdateStats();
    }

    void GainDebuff(int amount)
    {
        totalDebuffs += amount;

        if (CombatScript.PlayerScript.itemCheck[6])
            TakeMagicDamage((7 + CombatScript.PlayerScript.level * 1) * amount);
    }

    public int DebuffCount()
    {
        tempi = weak + vulnerable + stun;
        return tempi;
    }

    public int ShieldAmount()
    {
        tempi = shield + block;
        return tempi;
    }

    // --- Moves ---

    // Risen
    int Risen_Strike_Damage()
    {
        tempi = 16 + (2 * power) / 5;
        return DamageDealtMultiplyer(tempi);
    }

    void Risen_Block()
    {
        tempi = 18 + resistance + power / 2;
        if (tempi > 0)
            GainBlock(tempi);
    }

    void Risen_Enrage()
    {
        tempi = 2 + (strength * 3 + power) / 12;
        GainStrength(tempi);

        tempi = 2 + resistance + power / 2;
        if (tempi > 0)
            GainBlock(tempi);
    }

    // Ghoul
    int Ghoul_Only_Purpose_Damage()
    {
        tempi = 8 + power / 5;
        return DamageDealtMultiplyer(tempi);
    }

    int Ghoul_Slash_Damage()
    {
        tempi = 14 + power / 6;
        return DamageDealtMultiplyer(tempi);
    }

    int Ghoul_Slash_Bleed()
    {
        tempi = 2 + power / 9;
        return tempi;
    }

    int Ghoul_Cannibalize_Damage()
    {
        tempi = 14 + (3 * power) / 7;
        return DamageDealtMultiplyer(tempi);
    }

    // Husk
    int Husk_Strike_Damage()
    {
        tempi = 17 + (6 * power) / 13;
        return DamageDealtMultiplyer(tempi);
    }

    void Husk_Flies_Nest()
    {
        tempi = 12 + resistance + (5 * power) / 9;
        if (tempi > 0)
            GainBlock(tempi);
        flySwarm++;
        Display(1, StatusSprites[8]);
    }

    int Husk_Haunting_Wail_Damage()
    {
        tempi = 4 + power / 12;
        return DamageDealtMultiplyer(tempi);
    }

    // Rotfiend
    int Rotfiend_Tear_Flesh_Damage()
    {
        tempi = 15 + (3 * power) / 8;
        return DamageDealtMultiplyer(tempi);
    }

    int Rotfiend_Tear_Flesh_Restore()
    {
        tempi = 8 + power / 5;
        return tempi;
    }

    int Rotfiend_Decay_Damage()
    {
        tempi = 5 + power / 9 - strength / 2;
        return DamageDealtMultiplyer(tempi);
    }

    int Rotfiend_Wither_Freeze()
    {
        tempi = 3 + power / 13;
        return tempi;
    }

    int Rotfiend_Strike_Damage()
    {
        tempi = 19 + (8 * power) / 13;
        return DamageDealtMultiplyer(tempi);
    }

    // Shambler
    int Shambler_Eye_for_adn_Eye_Damage()
    {
        tempi = 18 + (2 * bleed) / 3 + (3 * power) / 5;
        return DamageDealtMultiplyer(tempi);
    }

    int Shambler_Eye_for_adn_Eye_Bleed()
    {
        tempi = 1 + bleed / 8 + power / 29;
        return tempi;
    }

    void Shambler_Flesh_Hide()
    {
        tempi = 40 - (3 * power) / 8;
        GainBlock(tempi);

        tempi = 12 + (3 * power) / 2;
        GainShield(tempi);

        GainBleed(2);
    }

    void Shambler_Empty_Stare()
    {
        CombatScript.Player.GainWeak(2);
        CombatScript.Player.GainFreeze(2);
        CombatScript.Player.GainFrail(2);
        // sanity
    }

    int Shambler_No_Regrets_Damage()
    {
        tempi = 36 + (18 * power) / 17;
        return DamageDealtMultiplyer(tempi);
    }
}