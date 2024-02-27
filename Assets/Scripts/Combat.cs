using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    public PlayerMovement PlayerScript;

    public PlayerCombat Player;
    public EnemyCombat[] Enemy;
    public GameObject[] EnemyObjects;
    public GameObject Hud;
    public TMPro.TextMeshProUGUI TurnValue, EffectTooltip;
    public Button endTurnButton;

    public int enemyCount, targetedEnemy, turnCount;
    public int[] enemyLevels;
    public bool[] EnemyAlive;
    public int enemyTurn;
    float delay;

    [Header("Loot")]
    public CacheHud Cache;
    public int experienceFromCombat, silverFromCombat, shardsFromCombat, bonesFromCombat;
    public bool potionFromCombat, itemFromCombat;

    public void SetOneEnemy(int Type)
    {
        enemyCount = 1;

        Enemy[0].level = enemyLevels[0];
        Enemy[0].SetType(Type);
        EnemyAlive[0] = true;
        EnemyObjects[0].SetActive(true);

        EnemyObjects[1].SetActive(false);
        EnemyObjects[2].SetActive(false);
    }

    public void SetTwoEnemies(int Type1, int Type2)
    {
        enemyCount = 2;

        Enemy[0].level = enemyLevels[0];
        Enemy[0].SetType(Type1);
        EnemyAlive[0] = true;
        EnemyObjects[0].SetActive(true);

        Enemy[1].level = enemyLevels[1];
        Enemy[1].SetType(Type2);
        EnemyAlive[1] = true;
        EnemyObjects[1].SetActive(true);

        EnemyObjects[2].SetActive(false);
    }

    public void SetThreeEnemies(int Type1, int Type2, int Type3)
    {
        enemyCount = 3;

        Enemy[0].level = enemyLevels[0];
        Enemy[0].SetType(Type1);
        EnemyAlive[0] = true;
        EnemyObjects[0].SetActive(true);

        Enemy[1].level = enemyLevels[1];
        Enemy[1].SetType(Type2);
        EnemyAlive[1] = true;
        EnemyObjects[1].SetActive(true);

        Enemy[2].level = enemyLevels[2];
        Enemy[2].SetType(Type3);
        EnemyAlive[2] = true;
        EnemyObjects[2].SetActive(true);
    }

    public void StartCombat()
    {
        Hud.SetActive(true);
        turnCount = 0;
        ChangeTarget(0);
        Player.Set();
        StartTurn();
    }

    void StartTurn()
    {
        endTurnButton.interactable = true;
        turnCount++;
        TurnValue.text = turnCount.ToString("0");

        Player.StartTurn();
        for (int i = 0; i < enemyCount; i++)
        {
            Enemy[i].StartTurn();
        }
    }

    public void EndTurn()
    {
        endTurnButton.interactable = false;
        Player.EndTurn();
        enemyTurn = 0;
        delay = 0.5f;
        for (int i = 0; i < Enemy.Length; i++)
        {
            if (EnemyAlive[i])
            {
                Invoke("EnemyTurn", delay);
                delay += 0.6f;
            }
        }
        delay += 0.5f;
        Invoke("StartTurn", delay);
    }

    void EnemyTurn()
    {
        if (EnemyAlive[enemyTurn])
        {
            Enemy[enemyTurn].EndTurn();
            enemyTurn++;
        }
        else
        {
            enemyTurn++;
            EnemyTurn();
        }
    }

    public void EnemyDeath(int order)
    {
        EnemyAlive[order] = false;
        EnemyObjects[order].SetActive(false);
        enemyCount--;
        bonesFromCombat++;
        if (enemyCount <= 0)
            WonCombat();
        else
        {
            enemyTurn = 0;
            while (targetedEnemy == order)
            {
                if (EnemyAlive[enemyTurn])
                    ChangeTarget(enemyTurn);
                else enemyTurn++;
            }
        }
    }

    void WonCombat()
    {
        PlayerScript.hitPoints = Player.hitPoints;
        PlayerScript.FinishCombat();
        Hud.SetActive(false);
        Cache.Display(silverFromCombat, shardsFromCombat, bonesFromCombat, experienceFromCombat, potionFromCombat, itemFromCombat);
    }

    public void ChangeTarget(int which)
    {
        Enemy[targetedEnemy].TargetArrow.SetActive(false);
        targetedEnemy = which;
        Enemy[targetedEnemy].TargetArrow.SetActive(true);
    }

    public void EffectHovered(bool player, int whichEnemy, int which)
    {
        if (player)
            DisplayPlayerEffect(which);
        else
            DisplayEnemyEffect(whichEnemy, which);
    }

    void DisplayPlayerEffect(int order)
    {
        switch (Player.statusEffects[order])
        {
            case 0:
                DisplayEffectTooltip("strength");
                break;
            case 1:
                DisplayEffectTooltip("resistance");
                break;
            case 2:
                DisplayEffectTooltip("valor");
                break;
            case 3:
                DisplayEffectTooltip("bleed");
                break;
            case 4:
                DisplayEffectTooltip("weak");
                break;
            case 5:
                DisplayEffectTooltip("vulnerable");
                break;
            case 6:
                DisplayEffectTooltip("freeze");
                break;
            case 7:
                DisplayEffectTooltip("storedBlock");
                break;
            case 8:
                DisplayEffectTooltip("poison");
                break;
            case 9:
                DisplayEffectTooltip("armor");
                break;
            case 10:
                DisplayEffectTooltip("frail");
                break;
            case 11:
                DisplayEffectTooltip("dexterity");
                break;
            case 12:
                DisplayEffectTooltip("power");
                break;
            case 13:
                DisplayEffectTooltip("impervious");
                break;
        }
    }

    void DisplayEnemyEffect(int enemy, int order)
    {
        switch (Enemy[enemy].statusEffects[order])
        {
            case 0:
                DisplayEffectTooltip("boneClaws");
                break;
            case 1:
                DisplayEffectTooltip("tenacity");
                break;
            case 2:
                DisplayEffectTooltip("strength");
                break;
            case 3:
                DisplayEffectTooltip("bleed");
                break;
            case 4:
                DisplayEffectTooltip("weak");
                break;
            case 5:
                DisplayEffectTooltip("vulnerable");
                break;
            case 6:
                DisplayEffectTooltip("slow");
                break;
            case 7:
                DisplayEffectTooltip("stun");
                break;
            case 8:
                DisplayEffectTooltip("flySwarm");
                break;
            case 9:
                DisplayEffectTooltip("rot");
                break;
            case 10:
                DisplayEffectTooltip("hollow");
                break;
            case 11:
                DisplayEffectTooltip("impervious");
                break;
        }
    }

    void DisplayEffectTooltip(string effect)
    {
        switch (effect)
        {
            case "strength":
                EffectTooltip.text = "Strength: \n Increase Damage Dealt by 1";
                break;
            case "dexterity":
                EffectTooltip.text = "Dexterity: \n Gives 1 Energy at the start of Turn";
                break;
            case "resistance":
                EffectTooltip.text = "Resistance: \n Increase Block Gained by 1";
                break;
            case "power":
                EffectTooltip.text = "Power: \n Gives 1 Mana at the start of Turn";
                break;
            case "valor":
                EffectTooltip.text = "Valor: \n Empowers certain Abilities";
                break;
            case "bleed":
                EffectTooltip.text = "Bleed: \n Deals 1 Damage every Trun";
                break;
            case "weak":
                EffectTooltip.text = "Weak: \n Reduce Damage Dealt by 25%";
                break;
            case "vulnerable":
                EffectTooltip.text = "Vulnerable: \n Increase Damage Taken by 25%";
                break;
            case "freeze":
                EffectTooltip.text = "Freeze: \n Increases Energy required for bonus Action by 1";
                break;
            case "storedBlock":
                EffectTooltip.text = "Stored Block: \n Grant Block Next Turn";
                break;
            case "poison":
                EffectTooltip.text = "Poison: \n Deals 1 Magic Damage every Trun";
                break;
            case "armor":
                EffectTooltip.text = "Aoison: \n Gives 1 Block every Trun";
                break;
            case "frail":
                EffectTooltip.text = "Frail: \n Reduce Block Gained by 25%";
                break;
            case "boneClaws":
                EffectTooltip.text = "Bone Claws: \n Attacks Apply 1 Bleed";
                break;
            case "tenacity":
                EffectTooltip.text = "Tenacity: \n Increases Slow Required to Stun by 1";
                break;
            case "slow":
                EffectTooltip.text = "Slow: \n 5x Slow applies Stun";
                break;
            case "stun":
                EffectTooltip.text = "Stun: \n Skips whole Turn";
                break;
            case "flySwarm":
                EffectTooltip.text = "Fly Swarm: \n Deals 1 Magic Damage to enemy every Turn";
                break;
            case "rot":
                EffectTooltip.text = "Rot: \n Every Turn Gain Strength & 1 Slow";
                break;
            case "hollow":
                EffectTooltip.text = "Hollow: \n Takes only 2/3 Damage, but gains 1 Bleed";
                break;
            case "impervious":
                EffectTooltip.text = "Impervious: \n Negate next 1 Debuff gained";
                break;
        }
    }

    public void Unhovered()
    {
        EffectTooltip.text = "";
    }
}
