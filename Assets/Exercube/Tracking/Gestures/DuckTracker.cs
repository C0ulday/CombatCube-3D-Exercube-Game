using System;
using System.Text;
using Sphery.ExerCube;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sphery.ExerCube
{
    public class DuckTracker : MonoBehaviour
    {
        public Vector3 worldUp = Vector3.up;

        public float minDuckDepth = 0.15f; // Minimum distance body must lower from standing (reduced)
        public float duckCooldown = 1.0f; // Time in seconds before another duck can be detected
        public float standingThreshold = 0.15f; // How close to standing height to reset
        public float minDownwardVelocity = 0.15f; // Minimum downward velocity to register as ducking (reduced)
        public float maxHandHeight = 0.65f; // Maximum hand height in world space to register as valid duck

        [SerializeField] protected TMP_Text console;

        public UnityEvent duckDetected;

        public Text debugText;

        private ITrackingManager trackingManager;
        private ActivityConsole Console;

        private DuckTrackerData duckData;

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
            Vector3 leftWrist = trackingManager[Tracker.LeftWrist].Position;
            Vector3 rightWrist = trackingManager[Tracker.RightWrist].Position;
            Vector3 leftAnkle = trackingManager[Tracker.LeftAnkle].Position;
            Vector3 rightAnkle = trackingManager[Tracker.RightAnkle].Position;

            duckData = new DuckTrackerData(leftWrist, rightWrist, leftAnkle, rightAnkle, worldUp);
        }

        private void Update()
        {
            Vector3 leftWrist = trackingManager[Tracker.LeftWrist].Position;
            Vector3 rightWrist = trackingManager[Tracker.RightWrist].Position;
            Vector3 leftAnkle = trackingManager[Tracker.LeftAnkle].Position;
            Vector3 rightAnkle = trackingManager[Tracker.RightAnkle].Position;

            duckData.Update(leftWrist, rightWrist, leftAnkle, rightAnkle, worldUp,
                minDuckDepth, duckCooldown, standingThreshold, minDownwardVelocity, maxHandHeight,
                duckDetected, debugText, Console);
        }

        private class DuckTrackerData
        {
            public float BodyHeight { get; private set; } // Distance from feet to hands
            public float DuckDepth { get; private set; } // How much lower than standing
            public float VerticalVelocity { get; private set; } // Negative when ducking down

            private float standingHeight; // Reference height when standing
            private float previousBodyHeight;
            private bool hasDucked = false;
            private float lastDuckTime = -10f;

            public DuckTrackerData(Vector3 leftWrist, Vector3 rightWrist, Vector3 leftAnkle, Vector3 rightAnkle, Vector3 worldUp)
            {
                float avgWristHeight = (leftWrist.y + rightWrist.y) * 0.5f;
                float avgAnkleHeight = (leftAnkle.y + rightAnkle.y) * 0.5f;
                BodyHeight = avgWristHeight - avgAnkleHeight;
                standingHeight = BodyHeight;
                previousBodyHeight = BodyHeight;
            }

            public void Update(Vector3 leftWrist, Vector3 rightWrist, Vector3 leftAnkle, Vector3 rightAnkle, Vector3 worldUp,
                float minDuckDepth, float duckCooldown, float standingThreshold, float minDownwardVelocity, float maxHandHeight,
                UnityEvent duckEvent, Text debugText, ActivityConsole console)
            {
                // Calculate current body height (distance from feet to hands)
                float avgWristHeight = (leftWrist.y + rightWrist.y) * 0.5f;
                float avgAnkleHeight = (leftAnkle.y + rightAnkle.y) * 0.5f;
                BodyHeight = avgWristHeight - avgAnkleHeight;

                // Calculate duck depth relative to standing height
                DuckDepth = standingHeight - BodyHeight;

                // Calculate vertical velocity (negative when going down)
                float heightDelta = BodyHeight - previousBodyHeight;
                VerticalVelocity = heightDelta / Time.deltaTime;

                // Update standing height when upright (more lenient)
                if (DuckDepth <= standingThreshold)
                {
                    // Gradually adjust standing height to current when upright
                    standingHeight = Mathf.Lerp(standingHeight, BodyHeight, 0.1f);
                }

                // Detect duck
                // A duck is detected when:
                // 1. Body has lowered significantly from standing
                // 2. Moving downward (negative vertical velocity)
                // 3. Cooldown period has passed
                // 4. Haven't ducked yet in this motion
                // 5. Hands are low enough (below maximum height)
                bool canDuckAgain = (Time.time - lastDuckTime) >= duckCooldown;
                bool deepEnough = DuckDepth >= minDuckDepth;
                bool movingDown = VerticalVelocity <= -minDownwardVelocity;
                bool handsLowEnough = avgWristHeight <= maxHandHeight;

                // Trigger duck event when lowering body
                if (deepEnough && movingDown && canDuckAgain && !hasDucked && handsLowEnough)
                {
                    duckEvent?.Invoke();
                    AudioManager.Instance.PlayKick();
                    lastDuckTime = Time.time;
                    hasDucked = true;
                    console.Log("DUCK DETECTED - Depth: " + DuckDepth.ToString("F2") + "m, VertVel: " + VerticalVelocity.ToString("F2") + "m/s");
                }

                // Reset ducking state when standing back up
                if (DuckDepth <= standingThreshold)
                {
                    hasDucked = false;
                }

                // Debug stuff
                if (debugText != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("BodyHeight:\t\t\t\t\t");
                    builder.AppendLine(BodyHeight.ToString("F2"));
                    builder.Append("StandingHeight:\t\t\t");
                    builder.AppendLine(standingHeight.ToString("F2"));
                    builder.Append("DuckDepth:\t\t\t\t\t");
                    builder.AppendLine(DuckDepth.ToString("F2"));
                    builder.Append("VerticalVelocity:\t\t\t");
                    builder.AppendLine(VerticalVelocity.ToString("F2"));
                    builder.Append("AvgWristHeight:\t\t\t");
                    builder.AppendLine(avgWristHeight.ToString("F2"));
                    builder.Append("DeepEnough:\t\t\t\t\t");
                    builder.AppendLine(deepEnough.ToString());
                    builder.Append("MovingDown:\t\t\t\t\t");
                    builder.AppendLine(movingDown.ToString());
                    builder.Append("HandsLowEnough:\t\t\t");
                    builder.AppendLine(handsLowEnough.ToString());
                    builder.Append("HasDucked:\t\t\t\t\t");
                    builder.AppendLine(hasDucked.ToString());
                    builder.Append("TimeSinceDuck:\t\t\t\t");
                    builder.AppendLine((Time.time - lastDuckTime).ToString("F2"));

                    debugText.text = builder.ToString();
                }

                // Prepare for next frame
                previousBodyHeight = BodyHeight;
            }
        }
    }
}
