using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sphery.ExerCube;

using UnityEngine;
using Sphery.ExerCube;

public class RobotHit : MonoBehaviour
{
    public Animator animator;
    public Transform hitPoint;                    
    public float hitDistance = 0.35f;

    private ITrackingManager tracking;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        GameObject player = GameObject.FindWithTag("Player");
        tracking = player.GetComponentInChildren<ITrackingManager>();
    }

    void Update()
    {
        if (tracking == null) return;

        Vector3 leftHand  = tracking[Tracker.LeftWrist].Position;
        Vector3 rightHand = tracking[Tracker.RightWrist].Position;

        // Distance from hitPoint
        float leftDist  = Vector3.Distance(hitPoint.position, leftHand);
        float rightDist = Vector3.Distance(hitPoint.position, rightHand);

        // Debug
        Debug.DrawLine(hitPoint.position, leftHand, Color.red);
        Debug.DrawLine(hitPoint.position, rightHand, Color.blue);

        if (leftDist < hitDistance || rightDist < hitDistance)
        {
            animator.SetTrigger("isHit");
        }
    }
}
