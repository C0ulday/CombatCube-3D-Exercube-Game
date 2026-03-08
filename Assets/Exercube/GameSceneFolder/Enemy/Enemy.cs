using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    public float moveSpeed = 4f;
    public float stopDistance = 4f;
    private int EnemyStandStillCounter = 0;

    public float difficulty;

    private Transform player;
    private Animator animator;
    private bool isDead = false;

    private bool wasKilled = false;

    public LevelLogic progressLogic;
    public PlayerHealth playerHealth;

    //private bool arrived = false;

    public int EnemyHealth;
    public Healthbar Healthbar;

    private void Start()
    {
        difficulty = 0;
        // Get player reference
        player = GameObject.FindWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        GameObject container = GameObject.Find("LevelProgress");
        playerHealth = Object.FindObjectOfType<PlayerHealth>();

        if (container != null)
        {
            
            progressLogic = container.GetComponent<LevelLogic>();
        }

        if (player == null)
            Debug.LogError("[Enemy] No object with tag 'Player' found!");
        if (animator == null)
            Debug.LogError("[Enemy] No Animator component found!");

        //Assign Enemy speed & health depending on level
        moveSpeed = LevelProgress.Instance.LevelSpeed();
        EnemyHealth = LevelProgress.Instance.LevelHealth();

        Healthbar.SetMaxHealth(EnemyHealth);
    }

    private void Update()
    {
        Healthbar.SetHealth(EnemyHealth);
        if (EnemyHealth <= 0) { isDead = true; Die(); }
        if (player == null || isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTowardsPlayer();
            //MoveToASpot();
        }
        else
        {
            //arrived = true;
            StopMoving();
            EnemyStandStillCounter++;
            //if (EnemyStandStillCounter >= 360) { Die(); }

        }
    }

    public void setDifficulty(float diff) { difficulty = diff; }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        transform.position += direction * (moveSpeed + difficulty) * Time.deltaTime;

        animator.SetBool("isRunning", true);
    }

    private void StopMoving()
    {
        animator.SetBool("isRunning", false);
        
    }

    /// <summary>
    /// Enemy dies immediately.
    /// </summary>
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("isDead", true);
        moveSpeed = 0f;


        if (wasKilled)
        {
            LevelProgress.Instance.AddKill();
        }
        else { LevelProgress.Instance.AddKill();  progressLogic.EnemySurvived(); playerHealth.TakeDamage(10); Debug.Log("SMALL -10HP"); }

            //Destroy(gameObject);
            Debug.Log("[Enemy] Enemy died!");
        if (NewEnemySpawner.Instance != null)
        {
            NewEnemySpawner.Instance.UnregisterEnemy(this);
        }

        Destroy(gameObject, 2f);
    }

    private void MoveToASpot()
    {
        // Postion for the right of the player: -12,0,0
        // front middle: -12,0,4
        // front right -12,0,1
        Vector3 direction = (new Vector3(-12,0,1) - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        transform.position += direction * moveSpeed * Time.deltaTime;

        animator.SetBool("isRunning", true);
    }

    public void takeDamage(int amount, bool combo) 
    {
        wasKilled = true;
        Die();
    }
}
