using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPoint;

    private GameObject spawnedBoss;

    public bool isThere;

    //positions
    public Transform position1; 
    public Transform position2;

    public static BossSpawner Instance;

    private void Awake()
    {
        
        Instance = this;
    }



    public void SpawnBoss()
    {
        if (spawnedBoss != null)
            return; // Boss already there

        spawnedBoss = Instantiate(
            bossPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
        BossMovement movement = spawnedBoss.GetComponent<BossMovement>();
        movement.SetupWaypoints(position1, position2);
        movement.health = 500;
        isThere = true;
    }
}
