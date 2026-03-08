using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using static Valve.VR.SteamVR_TrackedObject;

public class BossMovement : MonoBehaviour, IDamageable
{
    public Transform leftPosition;   // left position on the front display
    public Transform rightPosition;  // right position on the front display
    public float moveSpeed = 5f;     // movement speed
    public float waitTime = 5f;      // waiting time at a position

    private Animator animator;
    private string currentAnimation = "";

    private Transform targetPosition;
    private float waitTimer;

    private AudioSource audioSource;

    public GameObject warningPrefab; //WarningVisual Cube
    private GameObject currentWarning;

    public PlayerMovTesting playerState; //playerstate for testing whether the play is safe or not
    public PlayerHealth player;

    public int health;
    public int maxHealth = 1000;
    public GameObject gameOverCanvas;

    public List<BossAttackData> attackData; // all the available attacks of the boss

    //Sets up the positions. This will be called by the BossSpawner Object
    public void SetupWaypoints(Transform p1, Transform p2)
    {
        leftPosition = p1;
        rightPosition = p2;
    }

    private void Awake()
    {
        health = 1000;
        maxHealth = health;    }

    void Reset()
    {
        health = 1000;
        moveSpeed = 8;
        waitTime = 5;
        
        var renderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0,2); // select random position(0=left,1=right,...)
        switch (rand) 
        {
            case 0: targetPosition = leftPosition; break;
            case 1: targetPosition = rightPosition; break;
            default: targetPosition = leftPosition; break;
        }
        if (playerState == null)
        {
            playerState = Object.FindObjectOfType<PlayerMovTesting>();
            player = Object.FindObjectOfType<PlayerHealth>();
        }

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        waitTimer = 0f;
        health = 1000;

        StartCoroutine(SpawnAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosition == null) return;

        // 1. Movement
        if (Vector3.Distance(transform.position, targetPosition.position) > 0.01f)
        {
            StartCoroutine(DashAnimation());
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );

            // Boss always looks towards the player
            Vector3 dir = Vector3.zero - transform.position;
            dir.x = -13.51f;
            if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
            StartCoroutine(DashFinishAnimation());
            return; // Does nothing else while walking (Anything he might do while walking??)
        }

        /*
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
        {
            waitTimer += Time.deltaTime;

            // switch to other position after waiting
            if (waitTimer >= waitTime)
            {
                targetPosition = (targetPosition == leftPosition) ? rightPosition : leftPosition;
                waitTimer = 0f;
            }
            else if (waitTimer >= waitTime / 2)
            {
                ExecuteRandomAttack();
                waitTimer = waitTime;
            }
        }
        */

        // 2. Waiting and Attacking
        waitTimer += Time.deltaTime;

        // At half the waiting time, the boss does a random attack
        if (waitTimer >= waitTime / 2 && waitTimer < (waitTime / 2) + Time.deltaTime)
        {
            ExecuteRandomAttack();
        }

        // If wait time is done, change position
        if (waitTimer >= waitTime)
        {
            targetPosition = (targetPosition == leftPosition) ? rightPosition : leftPosition;
            // for more positions, make a new function, call it here and delete the line on top
            waitTimer = 0f;
        }
    }

    IEnumerator SpawnAnimation()
    {
        ChangeAnimation("Spawn");
        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        ChangeAnimation("Idle");
    }

    IEnumerator AttackAnimation(string attackType)
    {
        ChangeAnimation(attackType);
        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(attackType))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        ChangeAnimation("Idle");
    }

    IEnumerator DashAnimation()
    {
        ChangeAnimation("LeftDashStart");
        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("LeftDashStart"))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        ChangeAnimation("LeftDash");
    }

    IEnumerator DashFinishAnimation()
    {
        ChangeAnimation("LeftDashEnd");
        yield return null;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("LeftDashEnd"))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        ChangeAnimation("Idle");
    }

    IEnumerator DamageAnimation()
    {
        ChangeAnimation("Damage");
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(stateInfo.length);

    }

    void ExecuteRandomAttack() 
    {
        int randAttack = weightedRandom();
        /*
        switch (randAttack) 
        {
            case 0: Debug.Log("Boss used: Horizontal Slash"); break;
            case 1: Debug.Log("Boss used: Stab"); break;
            case 2: Debug.Log("Boss used: Vertical Slashes"); break;
            case 3: Debug.Log("Boss used: Laser"); break;
            default: Debug.Log("Boss used: !wrong number!"); break;
        }
        */

        if (randAttack < attackData.Count)
        {
            StartCoroutine(PerformAttack(attackData[randAttack]));
        }
    }

    private void ChangeAnimation(string animation, float crossfade = 0.2f)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.CrossFade(animation, crossfade);
        }
    }

    IEnumerator PerformAttack(BossAttackData attack)
    {
        // Telegraphing the attack
        Debug.Log($"<color=orange>BOSS USES: {attack.attackName}</color> (Zone: {attack.zone})");
        yield return AttackAnimation(attack.animationID);
        /*
        // here could be an animation to telegraph attacks
        yield return new WaitForSeconds(attack.telegraphDuration);
        */
        currentWarning = hitWarning(attack);
        ChangeAnimation(attack.animationID);
        

        if (attack.telegraphSound != null)
        {
            audioSource.PlayOneShot(attack.telegraphSound);
        }

        //(Impact)
        yield return new WaitForSeconds(attack.telegraphDuration); //wait for the telegraph duration
        Destroy(currentWarning);

        float timer = 0;
        bool hasHitPlayer = false;



        while (timer < attack.impactDuration)
        {
            
            // check, if player is safe
            if (!playerState.IsSafe(attack.zone) && !hasHitPlayer)
            {
                ApplyDamage(attack);
                hasHitPlayer = true; // ensures that the player will only be hit once
               

                if (attack.impactSound != null)
                {
                    audioSource.PlayOneShot(attack.impactSound);
                }
            }
            else if (!hasHitPlayer) { Debug.Log("<color=green>Dodged!</color>"); }

            timer += Time.deltaTime;
            yield return null; // wait
            



        }

        Debug.Log("<color=white>Attack finished.</color>");
    }

    void ApplyDamage(BossAttackData attack)
    {
        
        player.TakeDamage(attack.damageAmount);
        Debug.Log($"<color=red><b>PLAYER HIT FOR {attack.damageAmount}!</b></color>");
        // hit animation etc.
    }

    GameObject hitWarning(BossAttackData attack) 
    {
        Vector3 playerPos = playerState.transform.position; //player position
        currentWarning = Instantiate(warningPrefab, playerPos, Quaternion.identity);

        //Positions of the zones

        if (attack.zone == AttackZone.Low)
        {

            currentWarning.transform.position = new Vector3(playerPos.x + 1.0f, playerPos.y - 0.73f, playerPos.z); // Floor
            currentWarning.transform.localScale = new Vector3(5, 0.2f, 5);
        }
        else if (attack.zone == AttackZone.High)
        {
            currentWarning.transform.position = new Vector3(playerPos.x, playerPos.y + 0.5f, playerPos.z);
            currentWarning.transform.localScale = new Vector3(5, 0.5f, 1);
        }
        return currentWarning;
    }

    int weightedRandom() 
    {
        float value = Random.value;
        if (value <= 0.3f) return 0;
        if (value <= 0.6f) return 1;
        if (value <= 0.85f) return 2;
        return 3;
    }

    public void takeDamage(int amount, bool combo) 
    {
        

        if (combo == true)
        {
            StartCoroutine(DamageAnimation());
        }
        health -= amount;
        
        float ratio = (float) health / maxHealth;
        LevelProgress.Instance.updateBoss(ratio);
        if (health <= 0)
        {
            ResultUI.Instance.ShowWin();
            //gameOverCanvas.SetActive(true);
            //LevelProgress.Instance.endGame();
            //GameOverScript.Instance.ShowGameOver(false);
        }
        Debug.Log("HP: " + health);
    }
}

[System.Serializable]
public class BossAttackData
{
    public string attackName;
    public AttackZone zone;         // Low, Mid, Left, Right
    public float telegraphDuration; // time the attack is telegraphed beforehand
    public float impactDuration;    // time, the attack will do damage
    public string animationID;
    public AudioClip telegraphSound;
    public AudioClip impactSound;
    public int damageAmount;
}
