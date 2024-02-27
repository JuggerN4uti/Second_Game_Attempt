using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounters : MonoBehaviour
{
    public Combat CombatScript;
    public EnemiesLibrary EnemyLib;
    int roll;

    [Header("Encounters")]
    //public int[] MobsCount;
    //public int[] Mob1, Mob2, Mob3;
    public int[] Mobs;
    public float[] MobStrength;
    public int minValue, maxValue;
    //public float[] PowerLevel;
    //public int minPossibility, maxPossibility;
    //public bool minTaken, maxTaken;
    int mobCount;
    float currentPower;
    //int minValue, maxValue;

    [Header("Elite")]
    public int[] eliteM;

    /*void Start()
    {
        for (int i = 0; i < MobsCount.Length; i++)
        {
            for (int j = 0; j < MobsCount[i]; j++)
            {
                PowerLevel += PowerLevel[Mob1[j]]
            }
        }
    }*/

    public void SetSmallBattle(float danger)
    {
        Mobs[0] = 0;
        mobCount = 1;

        if (danger < 1f)
        {
            CombatScript.enemyLevels[0] = 0;
        }
        else CombatScript.enemyLevels[0] = 1;

        currentPower = danger;
        SetMobs();
    }

    public void SetBattle(float danger)
    {
        currentPower = 0;
        mobCount = 0;
        for (int i = 0; i < 3; i++)
        {
            CombatScript.enemyLevels[i] = 0;
        }
        AddMob();
        while (danger > currentPower * 1.25f)
        {
            if (danger > currentPower * 2.5f && mobCount < 3)
                AddMob();
            else
                LevelUpMob();
        }

        SetMobs();
    }

    void AddMob()
    {
        roll = Random.Range(0, MobStrength.Length);
        Mobs[mobCount] = roll;
        mobCount++;
        currentPower += MobStrength[roll];
        currentPower *= (0.75f + mobCount * 0.25f);
    }

    void LevelUpMob()
    {
        roll = Random.Range(0, mobCount);
        CombatScript.enemyLevels[roll]++;
        currentPower += MobStrength[Mobs[roll]] * (0.25f + CombatScript.enemyLevels[roll] * 0.05f) * (0.75f + mobCount * 0.25f);
    }

    /*public void SetEncounter(float danger)
    {
        minTaken = false; maxTaken = false;
        for (int i = 0; i < MobsCount.Length; i++)
        {
            if (!minTaken)
            {
                if (PowerLevel[i] >= danger * 0.8f - 0.4f)
                {
                    minPossibility = i;
                    minTaken = true;
                }
            }
            if (!maxTaken)
            {
                if (PowerLevel[i] >= danger * 1.16f + 0.5f)
                {
                    maxPossibility = i - 1;
                    if (maxPossibility < 0)
                        maxPossibility = 0;
                    maxTaken = true;
                }
            }
        }
        if (!maxTaken)
            maxPossibility = MobsCount.Length;

        if (!minTaken)
        {
            minPossibility = maxPossibility - 3;
            if (minPossibility < 0)
                minPossibility = 0;
        }

        roll = Random.Range(minPossibility, maxPossibility + 1);

        SetMobs(roll, danger);
    }*/

    void SetMobs()
    {
        switch (mobCount)
        {
            case 1:
                CombatScript.SetOneEnemy(Mobs[0]);
                break;
            case 2:
                CombatScript.SetTwoEnemies(Mobs[0], Mobs[1]);
                break;
            case 3:
                CombatScript.SetThreeEnemies(Mobs[0], Mobs[1], Mobs[2]);
                break;
        }

        minValue = Mathf.FloorToInt(currentPower * 0.93f);
        maxValue = Mathf.FloorToInt(currentPower * 1.42f);

        CombatScript.experienceFromCombat = Random.Range(6 + (8 * minValue / 15), 7 + (17 * maxValue) / 20) * 2;
        CombatScript.silverFromCombat = Random.Range(7 + (3 * minValue) / 4, 10 + (7 * maxValue) / 10);
        CombatScript.shardsFromCombat = Random.Range(1 + (6 * minValue) / 24, 3 + (11 * maxValue) / 39);
        CombatScript.bonesFromCombat = 0;

        CombatScript.potionFromCombat = false;
        CombatScript.itemFromCombat = false;

        for (int i = 0; i < 2; i++)
        {
            roll = Random.Range(0, 3);
            switch (roll)
            {
                case 0:
                    CombatScript.experienceFromCombat += Random.Range(6 + (8 * minValue / 15), 7 + (17 * maxValue) / 20);
                    break;
                case 1:
                    CombatScript.silverFromCombat += Random.Range(7 + (3 * minValue) / 4, 10 + (7 * maxValue) / 10);
                    break;
                case 2:
                    CombatScript.shardsFromCombat += Random.Range(1 + (6 * minValue) / 24, 3 + (11 * maxValue) / 39);
                    break;
            }
        }
    }

    /*public void Easy()
    {
        CombatScript.experienceFromCombat = Random.Range(7, 11);
        CombatScript.silverFromCombat = Random.Range(9, 16);
        CombatScript.shardsFromCombat = Random.Range(1, 2);
        CombatScript.potionFromCombat = false; // potem mo¿e 25% szansy
        CombatScript.itemFromCombat = false;

        roll = Random.Range(0, easy2.Length);
        if (easy2[roll]) CombatScript.SetTwoEnemies(easyM[roll], easyM2[roll]);
        else CombatScript.SetOneEnemy(easyM[roll]);
    }

    public void Medium()
    {
        CombatScript.experienceFromCombat = Random.Range(12, 19);
        CombatScript.silverFromCombat = Random.Range(13, 24);
        CombatScript.shardsFromCombat = Random.Range(1, 3);
        CombatScript.potionFromCombat = false; // potem mo¿e 50% szansy
        CombatScript.itemFromCombat = false;

        roll = Random.Range(0, med2.Length);
        if (med2[roll]) CombatScript.SetTwoEnemies(medM[roll], medM2[roll]);
        else CombatScript.SetOneEnemy(medM[roll]);
    }

    public void Hard()
    {
        CombatScript.experienceFromCombat = Random.Range(18, 29);
        CombatScript.silverFromCombat = Random.Range(18, 34);
        CombatScript.shardsFromCombat = Random.Range(2, 3);
        CombatScript.potionFromCombat = false; // potem mo¿e 75% szansy
        CombatScript.itemFromCombat = false;

        roll = Random.Range(0, hard2.Length);
        if (hard2[roll]) CombatScript.SetTwoEnemies(hardM[roll], hardM2[roll]);
        else CombatScript.SetOneEnemy(hardM[roll]);
    }*/

    public void Elite(int count)
    {
        CombatScript.enemyLevels[0] = count - 1;
        CombatScript.experienceFromCombat = Random.Range(25, 41);
        CombatScript.silverFromCombat = Random.Range(25, 46);
        CombatScript.shardsFromCombat = Random.Range(3, 4);
        CombatScript.bonesFromCombat = 1;
        CombatScript.potionFromCombat = false;
        CombatScript.itemFromCombat = true;

        roll = Random.Range(0, eliteM.Length);
        CombatScript.SetOneEnemy(eliteM[roll]);
    }
}
