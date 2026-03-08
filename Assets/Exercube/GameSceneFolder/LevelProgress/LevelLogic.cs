using UnityEngine;
public class LevelLogic : MonoBehaviour
{
    public int level;
    private int lastLevel = -1; // track level changes
    public NewEnemySpawner enemySpawner;
    public BossSpawner bossSpawner;

    public float difficulty;
    private int killCount;

    public bool gameStarted = false;

    void Start()
    {
        level = LevelProgress.Instance.level;
        difficulty = 0.75f;
    }

    void Update()
    {
        if (!gameStarted) { return; }
        level = LevelProgress.Instance.level;

        // Play music when level changes
        if (level != lastLevel)
        {
            lastLevel = level;
            AudioManager.Instance.PlayLevelMusic(level);
            difficulty += 0.25f;
            NewEnemySpawner.Instance.setDifficulty(difficulty);
        }

        if (level < LevelProgress.Instance.bossLevel) { NewEnemySpawner.Instance.startSpawning(); }
        else { NewEnemySpawner.Instance.stopSpawning(); }

        if (level == LevelProgress.Instance.bossLevel && !BossSpawner.Instance.isThere)
        {
            TransitionToBoss();
            BossSpawner.Instance.SpawnBoss();
        }
    }

    void TransitionToBoss()
    {
        Enemy[] enemies = Object.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Die();
        }
    }

    public void EnemySurvived() 
    {
        difficulty = difficulty - 0.25f > 0f ? difficulty - 0.25f : 0f ;
        NewEnemySpawner.Instance.setDifficulty(difficulty);
        killCount = 0;

    }

    public void EnemyKilled() 
    {
        killCount++;
        if (killCount >= 5) { difficulty += 0.25f; killCount = 0; }
        
    }

    public void startGame() 
    {
        gameStarted = true;
    }

    public void endGame() {  gameStarted = false; NewEnemySpawner.Instance.stopSpawning(); }
}
