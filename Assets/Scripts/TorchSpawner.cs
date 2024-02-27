using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSpawner : MonoBehaviour
{
    public GameObject Torch;
    public Transform[] SpawnPoints;
    public float[] positionsX, positionsY;
    public float rangeOfSpawn, chanceForSpawn;

    void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].position = new Vector2(positionsX[i] + Random.Range(-rangeOfSpawn, rangeOfSpawn), positionsY[i] + Random.Range(-rangeOfSpawn, rangeOfSpawn));
            if (chanceForSpawn >= Random.Range(0f, 1f))
                Instantiate(Torch, SpawnPoints[i].position, SpawnPoints[i].rotation);
        }
    }
}
