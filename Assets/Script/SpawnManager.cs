using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] SpawnPoint[] spawnpoints;

    private void Awake()
    {
        instance = this;
        //spawnpoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }
}
