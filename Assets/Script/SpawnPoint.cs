using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject graphics;
    private void Awake() // for spawnpoints capsules
    {
        graphics.SetActive(false);
    }
}
