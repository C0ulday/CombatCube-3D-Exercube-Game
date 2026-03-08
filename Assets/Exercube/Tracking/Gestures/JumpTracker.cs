using System;
using System.Text;
using Sphery.ExerCube;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sphery.ExerCube
{
    public class JumpTracker : MonoBehaviour
    {
        public Vector3 worldUp = Vector3.up;

        public float minJumpHeight = 0.15f; // Minimum height both feet must be off ground
        public float minVerticalVelocity = 1.0f; // Minimum upward velocity to register as jump
        public float jumpCooldown = 0.8f; // Time in seconds before another jump can be detected
        public float maxFootSeparation = 0.5f; // Max horizontal distance between feet during jump

        [SerializeField] protected TMP_Text console;

        public UnityEvent jumpDetected;

        public Text debugText;

        private ITrackingManager trackingManager;
        private ActivityConsole Console;

        private JumpTrackerData jumpData;

        private void Awake()
        {
            // Find player
            GameObject player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("No player object found in scene!");
                return;
            }

            // Find tracking component
            trackingManager = player.GetComponentInChildren<ITrackingManager>();

            Console = new ActivityConsole(console);

            if (trackingManager == null)
            {
                Debug.LogError("No <i>" + nameof(ITrackingManager) + "</i> component found on player object!");
                return;
            }
        }

        private void Start()
        {
            Vector3 leftAnkle = trackingManager[Tracker.LeftAnkle].Position;
            Vector3 rightAnkle = trackingManager[Tracker.RightAnkle].Position;
            jumpData = new JumpTrackerData(leftAnkle, rightAnkle, worldUp);
        }

        private void Update()
        {
            Vector3 leftAnkle = trackingManager[Tracker.LeftAnkle].Position;
            Vector3 rightAnkle = trackingManager[Tracker.RightAnkle].Position;

            jumpData.Update(leftAnkle, rightAnkle, worldUp, trackingManager.AnkleGroundHeight,
                minJumpHeight, minVerticalVelocity, jumpCooldown, maxFootSeparation,
                jumpDetected, debugText, Console);
        }

        private class JumpTrackerData
        {
            public float AverageHeight { get; private set; }
            public float HeightAboveGround { get; private set; }
            public float VerticalVelocity { get; private set; }
            public float FootSeparation { get; private set; }

            private float lastJumpTime = -10f;
            private float previousAverageHeight;

            public JumpTrackerData(Vector3 leftAnkle, Vector3 rightAnkle, Vector3 worldUp)
            {
                AverageHeight = (leftAnkle.y + rightAnkle.y) * 0.5f;
                previousAverageHeight = AverageHeight;
                VerticalVelocity = 0f;
            }

            public void Update(Vector3 leftAnkle, Vector3 rightAnkle, Vector3 worldUp, float groundHeight,
                float minJumpHeight, float minVerticalVelocity, float jumpCooldown, float maxFootSeparation,
                UnityEvent jumpEvent, Text debugText, ActivityConsole console)
            {
                // Calculate average height of both feet
                AverageHeight = (leftAnkle.y + rightAnkle.y) * 0.5f;
                HeightAboveGround = AverageHeight - groundHeight;

                // Calculate vertical velocity
                float heightDelta = AverageHeight - previousAverageHeight;
                VerticalVelocity = heightDelta / Time.deltaTime;

                // Calculate horizontal distance between feet
                Vector3 leftFlat = Vector3.ProjectOnPlane(leftAnkle, worldUp);
                Vector3 rightFlat = Vector3.ProjectOnPlane(rightAnkle, worldUp);
                FootSeparation = Vector3.Distance(leftFlat, rightFlat);

                // Detect jump
                // A jump is detected when:
                // 1. Both feet are elevated above minimum height
                // 2. Strong upward velocity (taking off)
                // 3. Feet are relatively close together (not doing splits)
                // 4. Cooldown period has passed
                bool canJumpAgain = (Time.time - lastJumpTime) >= jumpCooldown;
                bool bothFeetElevated = HeightAboveGround >= minJumpHeight;
                bool movingUpward = VerticalVelocity >= minVerticalVelocity;
                bool feetTogether = FootSeparation <= maxFootSeparation;

                if (bothFeetElevated && movingUpward && feetTogether && canJumpAgain)
                {
                    jumpEvent?.Invoke();
                    AudioManager.Instance.PlayKick();
                    lastJumpTime = Time.time;
                    console.Log("JUMP DETECTED - Height: " + HeightAboveGround.ToString("F2") + "m, VertVel: " + VerticalVelocity.ToString("F2") + "m/s");
                }

                // Debug stuff
                if (debugText != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("AverageHeight:\t\t\t\t");
                    builder.AppendLine(AverageHeight.ToString());
                    builder.Append("HeightAboveGround:\t\t");
                    builder.AppendLine(HeightAboveGround.ToString());
                    builder.Append("VerticalVelocity:\t\t\t");
                    builder.AppendLine(VerticalVelocity.ToString());
                    builder.Append("FootSeparation:\t\t\t");
                    builder.AppendLine(FootSeparation.ToString());
                    builder.Append("TimeSinceJump:\t\t\t\t");
                    builder.AppendLine((Time.time - lastJumpTime).ToString());

                    debugText.text = builder.ToString();
                }

                // Prepare for next frame
                previousAverageHeight = AverageHeight;
            }
        }
    }
}
