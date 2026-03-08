using System.Collections;
using System.Collections.Generic;
using Sphery.ExerCube;
using TMPro;
using UnityEngine;
public class GestureManager : MonoBehaviour
{
    public PunchIndicator leftPunchIndicator;
    public PunchIndicator rightPunchIndicator;

    private PunchTracker punchTracker;
    private KickTracker kickTracker;
    private BlockTracker blockTracker;
    private JumpTracker jumpTracker;
    private DuckTracker duckTracker;
    private ActivityConsole Console;

    [SerializeField] protected TMP_Text console;


    void Awake()
    {
        punchTracker = GetComponent<PunchTracker>();
        kickTracker = GetComponent<KickTracker>();
        blockTracker = GetComponent<BlockTracker>();
        jumpTracker = GetComponent<JumpTracker>();
        duckTracker = GetComponent<DuckTracker>();
        Console = new ActivityConsole(console);
        Console.Log("Gesture trackers initialized!");
    }

    void Start()
    {
        // Subscribe to punch events
        if (punchTracker != null)
        {
            punchTracker.leftPunchDetected.AddListener(OnLeftPunchDetected);
            punchTracker.rightPunchDetected.AddListener(OnRightPunchDetected);
            Console.Log("PunchTracker events registered!");
        }
        else
        {
            Console.Log("PunchTracker component not found!");
            Debug.LogError("PunchTracker component not found!");
        }

        // Subscribe to kick events
        if (kickTracker != null)
        {
            kickTracker.leftKickDetected.AddListener(OnLeftKickDetected);
            kickTracker.rightKickDetected.AddListener(OnRightKickDetected);
            Console.Log("KickTracker events registered!");
        }
        else
        {
            Console.Log("KickTracker component not found!");
            Debug.LogError("KickTracker component not found!");
        }

        // Subscribe to block events
        if (blockTracker != null)
        {
            blockTracker.blockDetected.AddListener(OnBlockDetected);
            Console.Log("BlockTracker events registered!");
        }
        else
        {
            Console.Log("BlockTracker component not found!");
            Debug.LogError("BlockTracker component not found!");
        }

        // Subscribe to jump events
        if (jumpTracker != null)
        {
            jumpTracker.jumpDetected.AddListener(OnJumpDetected);
            Console.Log("JumpTracker events registered!");
        }
        else
        {
            Console.Log("JumpTracker component not found!");
            Debug.LogError("JumpTracker component not found!");
        }

        // Subscribe to duck events
        if (duckTracker != null)
        {
            duckTracker.duckDetected.AddListener(OnDuckDetected);
            Console.Log("DuckTracker events registered!");
        }
        else
        {
            Console.Log("DuckTracker component not found!");
            Debug.LogError("DuckTracker component not found!");
        }
    }

    void OnLeftPunchDetected()
    {
        Console.Log("Left Punch");
        if (leftPunchIndicator != null)
        {
            leftPunchIndicator.Punch();
        }
    }

    void OnRightPunchDetected()
    {
        Console.Log("Right Punch");
        if (rightPunchIndicator != null)
        {
            rightPunchIndicator.Punch();
        }
    }

    void OnLeftKickDetected()
    {
        Console.Log("Left Kick");
        if (leftPunchIndicator != null)
        {
            leftPunchIndicator.Punch();
        }
    }

    void OnRightKickDetected()
    {
        Console.Log("Right Kick");
        if (rightPunchIndicator != null)
        {
            rightPunchIndicator.Punch();
        }
    }

    void OnBlockDetected()
    {
        Console.Log("Block detected!");
        // Add your block handling logic here
        // For example: activate shield visual, reduce incoming damage, etc.
    }

    void OnJumpDetected()
    {
        Console.Log("Jump detected!");
        // Add your jump handling logic here
        // For example: award points, trigger jump animation, etc.
    }

    void OnDuckDetected()
    {
        Console.Log("Duck detected!");
        // Add your duck handling logic here
        // For example: avoid attacks, trigger duck animation, etc.
    }

    void OnDestroy()
    {
        // Unsubscribe from punch events
        if (punchTracker != null)
        {
            punchTracker.leftPunchDetected.RemoveListener(OnLeftPunchDetected);
            punchTracker.rightPunchDetected.RemoveListener(OnRightPunchDetected);
        }

        // Unsubscribe from kick events
        if (kickTracker != null)
        {
            kickTracker.leftKickDetected.RemoveListener(OnLeftKickDetected);
            kickTracker.rightKickDetected.RemoveListener(OnRightKickDetected);
        }

        // Unsubscribe from block events
        if (blockTracker != null)
        {
            blockTracker.blockDetected.RemoveListener(OnBlockDetected);
        }

        // Unsubscribe from jump events
        if (jumpTracker != null)
        {
            jumpTracker.jumpDetected.RemoveListener(OnJumpDetected);
        }

        // Unsubscribe from duck events
        if (duckTracker != null)
        {
            duckTracker.duckDetected.RemoveListener(OnDuckDetected);
        }
    }
}

