using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private AudioClip enemySpawnSFX;
    public GameObject enemyPrefab;
    public float spawnDistance = 8f;
    public float spawnDelay = 2f;
    public float difficulty;
    public int TimesHitByEnemy = 0;
    public bool EnemyInFront = false;
    public bool EnemyOnRight = false;
    public bool EnemyOnLeft = false;
    private Enemy frontEnemy;
    private Enemy rightEnemy;
    private Enemy leftEnemy;

    [Header("Spawn Points (from CameraRig)")]
    public Transform frontCamera;
    public Transform leftCamera;
    public Transform rightCamera;

    private int FrameCounter = 0;

    private bool stopGame = false;

    private Transform player;
    private Enemy lastEnemy;
    private bool isSpawning = false;

    private bool shouldEnemiesSpawn = false;

    public static EnemySpawner Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {


        // Find the player in the scene
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("[EnemySpawner] No object with tag 'Player' found!");
            return;
        }

        Debug.Log("[EnemySpawner] Player detected: " + player.name);

        // Check if an enemy already exists in the scene
        lastEnemy = FindObjectOfType<Enemy>();

        if (!shouldEnemiesSpawn) { return; }

        if (lastEnemy == null)
        {
            Debug.Log("[EnemySpawner] No enemy found, spawning first enemy...");
            // Choose a random direction (front, left, right)
            Vector3 spawnDirection = GetRandomSpawnDirection2();
            StartCoroutine(SpawnEnemyCoroutine(spawnDirection));
        }
        else
        {
            Debug.Log("[EnemySpawner] Enemy already exists: " + lastEnemy.name);
        }
    }

    private void Update()
       
    {
        FrameCounter++;
        // Prevent multiple coroutines from running at the same time
        if (player == null || TimesHitByEnemy >= 3 || !shouldEnemiesSpawn) return;

        // Spawn a new enemy if the previous one was destroyed or died
        /*
        if (lastEnemy == null)
        {
            Debug.Log("[EnemySpawner] Enemy is null — starting spawn coroutine.");
            StartCoroutine(SpawnEnemyCoroutine());
        }
        */

        if (frontEnemy == null) { EnemyInFront = false;}
        if (rightEnemy == null) { EnemyOnRight=false; }
        if (leftEnemy == null) { EnemyOnLeft=false; }

        // stop if boss level is finished
        stopGame = LevelProgress.Instance.endGame();
        if (stopGame)
        {
            StopCoroutine(SpawnEnemyCoroutine(new Vector3()));
            return;
        }

        /*
        //Spawns a new enemy every 60 frames
        if (FrameCounter >= 60) 
        {
            FrameCounter = 0;
            // Choose a random direction (front, left, right)
            Vector3 spawnDirection = GetRandomSpawnDirection();
            if (!spawnDirection.Equals(new Vector3(-1000,-1000,-1000))) 
            { 
                StartCoroutine(SpawnEnemyCoroutine(spawnDirection)); 
            }
            
        }
        */

        // Choose a random direction (front, left, right)
        /*
        Vector3 spawnDirection = GetRandomSpawnDirection();
        if (!spawnDirection.Equals(new Vector3(-1000, -1000, -1000)))
        {
            StartCoroutine(SpawnEnemyCoroutine(spawnDirection));
        }
        */

        if (!isSpawning)
        {
            //List<Vector3> spawnDirections = GetRandomSpawnDirection();
            List<Vector3> spawnDirections = GetRandomSpawnDirection();
            foreach (Vector3 spawnDirection in spawnDirections)
            {

                // Wenn eine freie Richtung gefunden wurde (nicht -1000...)
                if (!spawnDirection.Equals(new Vector3(-1000, -1000, -1000)))
                {
                    StartCoroutine(SpawnEnemyCoroutine(spawnDirection));
                }
            }
        }

    }

    public void setDifficulty(float diff) { difficulty = diff; }

    private IEnumerator SpawnEnemyCoroutine(Vector3 spawnDirection)
    {
        isSpawning = true;

        Debug.Log("[EnemySpawner] Waiting " + spawnDelay + " - " + difficulty + " seconds before spawning...");

        // Wait before spawning a new enemy (gives time for death animation, etc.)
        // wait additional random time
        float randWait = Random.Range(0f, 1f);
        yield return new WaitForSeconds(spawnDelay - difficulty + randWait);

        
        
       
        Debug.Log("[EnemySpawner] Chosen spawn direction: " + spawnDirection);

        // Calculate spawn position based on random direction and distance from the player
        Vector3 spawnPos = player.position + spawnDirection * spawnDistance;
        spawnPos.y = 0; // Ensure enemy stays on the ground

        // IMPORTANT:
        // By default, Instantiate(..., Quaternion.identity) spawns the enemy with NO rotation.
        // This causes the enemy to appear facing the wrong direction (often backwards).
        //
        // We fix this by creating a rotation that makes the enemy LOOK AT the player.
        // 'LookRotation' generates a rotation pointing from the enemy toward the player.
        Quaternion lookRot = Quaternion.LookRotation(player.position - spawnPos, Vector3.up);

        // Spawn the enemy using the corrected rotation so it always faces the player
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, lookRot);
        enemyObj.GetComponent<Enemy>().setDifficulty(difficulty);
        if (enemySpawnSFX != null)
        {
            AudioManager.Instance.PlayEnemySpawn();
        }
        lastEnemy = enemyObj.GetComponent<Enemy>();
        if (spawnDirection.Equals(player.forward))
        {
            frontEnemy = enemyObj.GetComponent<Enemy>();
        }
        if (spawnDirection.Equals(rightCamera.forward))
        {
            rightEnemy = enemyObj.GetComponent<Enemy>();
        }
        if(spawnDirection.Equals(leftCamera.forward))
        {
            leftEnemy = enemyObj.GetComponent<Enemy>();
        }

        if (lastEnemy != null)
            Debug.Log("[EnemySpawner] Enemy spawned successfully: " + lastEnemy.name);
        else
            Debug.LogError("[EnemySpawner] Spawned object is missing Enemy component!");

        isSpawning = false;
    }

    private Vector3 GetRandomSpawnDirection2()
    {
        // Randomly select one of the three camera directions
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0: // Front pb with camera position so player
                if(EnemyInFront == true) { break; }
                Debug.Log("[EnemySpawner] Direction selected: FRONT");
                EnemyInFront = true;
                return player.forward;

            case 1: // Left
                if (EnemyOnLeft == true) { break; }
                Debug.Log("[EnemySpawner] Direction selected: LEFT");
                EnemyOnLeft = true;
                return leftCamera.forward;

            case 2: // Right
                if (EnemyOnRight == true) { break; }
                Debug.Log("[EnemySpawner] Direction selected: RIGHT");
                EnemyOnRight = true;
                return rightCamera.forward;
        }

        return new Vector3(-1000,-1000,-1000);
    }

    private List<Vector3> GetRandomSpawnDirection()
    {
        // 1. Eine Liste erstellen, in der wir alle aktuell freien Richtungen speichern
        List<Vector3> availableDirections = new List<Vector3>();

        // 2. Prüfen, welche Plätze frei sind und in die Liste packen
        if (!EnemyInFront) availableDirections.Add(player.forward);
        if (!EnemyOnLeft) availableDirections.Add(leftCamera.forward);
        if (!EnemyOnRight) availableDirections.Add(rightCamera.forward);

        // 3. Wenn absolut kein Platz frei ist, gib den Fehler-Vektor zurück
        if (availableDirections.Count == 0)
        {
            List<Vector3> errorList = new List<Vector3>();
            errorList.Add(new Vector3(-1000,-1000,-1000));
            return errorList;
        }

        // Random amount of spawns
        int randSpawnAmount = Random.Range(1, availableDirections.Count+1);
        List<Vector3> returned = new List<Vector3>();
        for (int i = 0; i <= randSpawnAmount; i++) 
        {
            int randomIndex = Random.Range(0, availableDirections.Count);
            Vector3 chosenDir = availableDirections[randomIndex];
            if (chosenDir == player.forward) EnemyInFront = true;
            else if (chosenDir == leftCamera.forward) EnemyOnLeft = true;
            else if (chosenDir == rightCamera.forward) EnemyOnRight = true;
            returned.Add(chosenDir);

            availableDirections.RemoveAt(randomIndex);
        }
        // 4. Nur aus den TATSÄCHLICH freien Richtungen zufällig eine wählen
        //int randomIndex = Random.Range(0, availableDirections.Count);
        //Vector3 chosenDir = availableDirections[randomIndex];

        // 5. Den gewählten Platz sofort als besetzt markieren
        //if (chosenDir == player.forward) EnemyInFront = true;
        //else if (chosenDir == leftCamera.forward) EnemyOnLeft = true;
        //else if (chosenDir == rightCamera.forward) EnemyOnRight = true;

        return returned;
    }

    
}
