using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewEnemySpawner : MonoBehaviour
{
    [SerializeField] private AudioClip enemySpawnSFX;
    public GameObject enemyPrefab;
    public float spawnDistance = 8f;
    public float spawnDelay = 4f;
    public float difficulty;
    public int TimesHitByEnemy = 0;

    [Header("Direction Status")]
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

    private bool shouldEnemiesSpawn = false;
    public static NewEnemySpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null) return;
        //startSpawning();
    }

    private Transform player;

    private void Update()
    {
        if (player == null || TimesHitByEnemy >= 3 || !shouldEnemiesSpawn) return;
        if (LevelProgress.Instance.endGame()) return;

        // Check each slot independently
        UpdateSlot(ref EnemyInFront, ref frontEnemy, frontCamera.forward);
        UpdateSlot(ref EnemyOnLeft, ref leftEnemy, leftCamera.forward);
        UpdateSlot(ref EnemyOnRight, ref rightEnemy, rightCamera.forward);
    }

    private void UpdateSlot(ref bool isOccupied, ref Enemy enemyRef, Vector3 direction)
    {
        // If the slot is empty and no spawn process is currently active for THIS specific slot
        if (enemyRef == null)
        {
            isOccupied = false;
            // Start a separate cooldown/spawn routine for this direction only
            StartCoroutine(IndividualSpawnRoutine(direction));
            // Set a temporary "dummy" to prevent starting multiple coroutines for the same slot
            enemyRef = enemyPrefab.GetComponent<Enemy>();
        }
    }

    private IEnumerator IndividualSpawnRoutine(Vector3 direction)
    {
        // The core of the randomness: Each slot waits a different time
        float individualWait = Random.Range(0.5f, spawnDelay) - (difficulty * 0.1f);
        yield return new WaitForSeconds(Mathf.Max(0.1f, individualWait));

        SpawnEnemy(direction);
    }

    private void SpawnEnemy(Vector3 direction)
    {
        Vector3 spawnPos = player.position + direction * spawnDistance;
        spawnPos.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(player.position - spawnPos, Vector3.up);

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, lookRot);
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.setDifficulty(difficulty);

            // Assign the real reference
            if (Vector3.Distance(direction, frontCamera.forward) < 0.1f) { frontEnemy = enemyScript; EnemyInFront = true; }
            else if (Vector3.Distance(direction, leftCamera.forward) < 0.1f) { leftEnemy = enemyScript; EnemyOnLeft = true; }
            else if (Vector3.Distance(direction, rightCamera.forward) < 0.1f) { rightEnemy = enemyScript; EnemyOnRight = true; }
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlayEnemySpawn();
    }

    public void setDifficulty(float diff) { difficulty = diff; }
    public void stopSpawning() { shouldEnemiesSpawn = false; }
    public void startSpawning() { shouldEnemiesSpawn = true; }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (enemy == frontEnemy) frontEnemy = null;
        if (enemy == leftEnemy) leftEnemy = null;
        if (enemy == rightEnemy) rightEnemy = null;
    }

}
