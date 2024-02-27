using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerMovement PlayerScript;

    [Header("Spawns")]
    public Transform SpawnPoint;
    public GameObject[] MobsT1, MobsT2, MobsT3;
    public GameObject Cache, GoldCache;
    public int GoldCacheCount;
    public int[] CacheCount;
    public float Radius;
    public float spawnTimer, spawnFrequency;
    public int totalSpawned, NotT2, NotT3;
    int tempi;
    float temp, rollX, rollY;

    void Start()
    {
        totalSpawned = 0; NotT2 = 0; NotT3 = 0;

        tempi = Random.Range(CacheCount[0], CacheCount[1] + 1) + GoldCacheCount;
        for (int i = 0; i < tempi; i++)
        {
            do
            {
                rollX = Random.Range(-Radius, Radius);
                rollY = Random.Range(-Radius, Radius);

                temp = rollX * rollX + rollY * rollY;
            } while (temp > Radius * Radius || temp <= Radius * Radius / 5);

            SpawnPoint.position = new Vector2(rollX, rollY);

            if (i < GoldCacheCount) Instantiate(GoldCache, SpawnPoint.position, SpawnPoint.rotation);
            else Instantiate(Cache, SpawnPoint.position, SpawnPoint.rotation);
        }
        Summon();
        Summon();
    }

    void Update()
    {
        if (PlayerScript.freeToMove)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
                Spawn();
        }
    }

    void Spawn()
    {
        spawnTimer += spawnFrequency;

        if (Random.Range(0f, 100f) <= spawnFrequency)
            spawnFrequency *= 0.9f;

        Summon();
    }

    void Summon()
    {
        do
        {
            rollX = Random.Range(-Radius * 2, Radius * 2);
            rollY = Random.Range(-Radius * 2, Radius * 2);
        } while (rollX * rollX + rollY * rollY <= Radius * Radius);

        SpawnPoint.position = new Vector2(rollX, rollY);

        temp = totalSpawned * 0.2f + (NotT3 * (NotT3 + 2) * 0.2f);
        if (temp >= Random.Range(0f, 100f))
        {
            Instantiate(MobsT3[Random.Range(0, MobsT3.Length)], SpawnPoint.position, SpawnPoint.rotation);
            NotT3 = 0;
        }
        else
        {
            NotT3++;

            temp = totalSpawned * 0.5f + (NotT2 * (NotT2 + 4) * 0.6f);
            if (temp >= Random.Range(0f, 100f))
            {
                Instantiate(MobsT2[Random.Range(0, MobsT2.Length)], SpawnPoint.position, SpawnPoint.rotation);
                NotT2 = 0;
            }
            else
            {
                NotT2++;
                Instantiate(MobsT1[Random.Range(0, MobsT1.Length)], SpawnPoint.position, SpawnPoint.rotation);
            }
        }

        totalSpawned++;
    }
}
