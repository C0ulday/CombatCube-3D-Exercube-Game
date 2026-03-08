using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGestureManager : MonoBehaviour
{
    public bool isJumping;
    public bool isCrouching;
    public PlayerMovTesting playerState = Object.FindObjectOfType<PlayerMovTesting>();
    // These function will be called by the gesture trackers
    public void JumpingImpulse()
    {
        playerState.isJumping = true;
        JumpDurationRoutine();
        playerState.isJumping = false;
    }

    private IEnumerator JumpDurationRoutine()
    {
        isJumping = true;
        Debug.Log("JUMP ACTIVE");

        //makes sure that the player doesn't stay in the air forever
        yield return new WaitForSeconds(0.6f);

        isJumping = false;
        Debug.Log("JUMP ENDED");
    }



    public void DuckingImpulse()
    {
        playerState.isCrouching = true;
        DuckDurationRoutine();
        playerState.isCrouching = false;
    }

    private IEnumerator DuckDurationRoutine()
    {
        isCrouching = true;
        Debug.Log("DUCKING ACTIVE");

        //makes sure that the player doesn't duck forever
        yield return new WaitForSeconds(0.6f);

        isCrouching = false;
        Debug.Log("DUCKING ENDED");
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

