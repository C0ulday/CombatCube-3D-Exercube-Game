using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovTesting : MonoBehaviour
{
    public bool isJumping;
    public bool isCrouching;

    [Header("Test-Tasten")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isJumping = Input.GetKey(KeyCode.Space);
        isCrouching = Input.GetKey(KeyCode.LeftControl);
    }

    public bool IsSafe(AttackZone zone)
    {
        if (zone == AttackZone.Low && isJumping) return true;    // jumped over horizontal
        if (zone == AttackZone.High && isCrouching) return true; // ducked under stab
        if (zone == AttackZone.Left) return false;               
        if (zone == AttackZone.Right) return false;

        return false;
    }
}

public enum AttackZone { Low, Left, Right, High }
