using System;
using System.Text;
using Sphery.ExerCube;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Sphery.ExerCube
{

    public class KickTracker : MonoBehaviour
    {
        public Vector3 worldUp = Vector3.up;

        public float minKickHeight = 0.3f; // Minimum height above ground to register as a kick
        public float minVelocity = 2.5f; // Increased from 1.5 - kicks are faster than foot shifts during punches
        public float kickCooldown = 0.5f; // Time in seconds before another kick can be detected
        public float minForwardVelocity = 1.0f; // Increased from 0.5 - actual kick has strong forward motion

        [SerializeField] protected TMP_Text console;


        public UnityEvent leftKickDetected;
        public UnityEvent rightKickDetected;

        [Header("Directional Events")]
        public Vector3Event leftKickWithDir;
        public Vector3Event rightKickWithDir;

        public Text leftDebugText;
        public Text rightDebugText;

        private ITrackingManager trackingManager;

        private FootTrackerData left;
        private FootTrackerData right;
        private ActivityConsole Console;

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
            left = new FootTrackerData(true, trackingManager[Tracker.LeftAnkle].Position, worldUp);
            right = new FootTrackerData(false, trackingManager[Tracker.RightAnkle].Position, worldUp);
        }

        private void Update()
        {
            left.PreUpdate(trackingManager[Tracker.LeftAnkle].Position, worldUp, trackingManager.AnkleGroundHeight);
            right.PreUpdate(trackingManager[Tracker.RightAnkle].Position, worldUp, trackingManager.AnkleGroundHeight);

            left.Update(minKickHeight, minVelocity, minForwardVelocity, kickCooldown, leftKickDetected, leftDebugText, Console);
            right.Update(minKickHeight, minVelocity, minForwardVelocity, kickCooldown, rightKickDetected, rightDebugText, Console);
        }

        private class FootTrackerData
        {
            public bool IsLeft { get; private set; }

            /// <summary>
            /// The current trackers position (flattened).
            /// </summary>
            public Vector3 Position { get; private set; }

            /// <summary>
            /// The current motion direction of the tracker (flattened).
            /// </summary>
            public Vector3 Direction { get; private set; }

            /// <summary>
            /// The current velocity (extrapolated to units/second).
            /// </summary>
            public float Velocity { get; private set; }

            /// <summary>
            /// The Y position (height) of the foot.
            /// </summary>
            public float HeightPosition { get; private set; }

            /// <summary>
            /// Height above ground level.
            /// </summary>
            public float HeightAboveGround { get; private set; }

            /// <summary>
            /// The current offset (in units) to the previous position.
            /// </summary>
            public float MovementDelta { get; private set; }

            /// <summary>
            /// Forward velocity component (in the direction of the kick).
            /// </summary>
            public float ForwardVelocity { get; private set; }

            private float lastKickTime = -10f;

            private Vector3 previousPosition;
            private Vector3 previousDirection;
            private float previousVelocity;

            /// <summary>
            /// Create a new instance of foot tracker data.
            /// </summary>
            /// <param name="isLeft">Whether this is the left foot.</param>
            /// <param name="position">The position of the tracker.</param>
            /// <param name="worldUp">The world up vector used for flattening.</param>
            public FootTrackerData(bool isLeft, Vector3 position, Vector3 worldUp)
            {
                IsLeft = isLeft;

                // Initialize previous values
                previousPosition = Vector3.ProjectOnPlane(position, worldUp);
                Direction = Vector3.zero;
                previousVelocity = 0f;

                MovementDelta = 0f;
            }

            public void PreUpdate(Vector3 position, Vector3 worldUp, float groundHeight)
            {
                Position = Vector3.ProjectOnPlane(position, worldUp);
                HeightPosition = position.y;
                HeightAboveGround = HeightPosition - groundHeight;
            }

            /// <summary>
            /// Update the foot tracker data and detect kicks.
            /// </summary>
            /// <param name="minKickHeight">Minimum height above ground to register as a kick.</param>
            /// <param name="minVelocity">Minimum overall velocity required.</param>
            /// <param name="minForwardVelocity">Minimum forward velocity component required.</param>
            /// <param name="kickCooldown">Time in seconds before another kick can be detected.</param>
            /// <param name="kickEvent">Event to invoke when kick is detected.</param>
            /// <param name="debugText">Optional debug text display.</param>
            /// <param name="console">Optional console for logging.</param>
            public void Update(float minKickHeight, float minVelocity, float minForwardVelocity, float kickCooldown, UnityEvent kickEvent, Text debugText, ActivityConsole console)
            {
                Direction = (Position - previousPosition).normalized;

                MovementDelta = Vector3.Distance(previousPosition, Position);
                Velocity = MovementDelta / Time.deltaTime; // Extrapolate to unit per second

                // Calculate forward velocity (Z-axis in Unity, assuming forward is positive Z)
                Vector3 movementVector = Position - previousPosition;
                ForwardVelocity = movementVector.z / Time.deltaTime;

                // Detect kick
                // A kick is detected when:
                // 1. Foot is lifted above minimum height
                // 2. Foot is moving fast enough (higher threshold than punch-induced foot movement)
                // 3. Foot has strong forward velocity (actual kicking motion, not just body shifting)
                // 4. Cooldown period has passed since last kick
                // 5. Foot must be elevated WHILE moving fast (not just standing on toes)
                bool canKickAgain = (Time.time - lastKickTime) >= kickCooldown;

                // Additional validation: foot must be moving upward or forward, not just shifted
                bool isElevatedAndMoving = HeightAboveGround >= minKickHeight && Velocity >= minVelocity;

                if (isElevatedAndMoving &&
                    Velocity >= minVelocity &&
                    ForwardVelocity >= minForwardVelocity &&
                    canKickAgain)
                {
                    kickEvent?.Invoke();

                    var mainTracker = GameObject.FindObjectOfType<KickTracker>();
                    if (mainTracker != null)
                    {
                        if (IsLeft) mainTracker.leftKickWithDir?.Invoke(Direction);
                        else mainTracker.rightKickWithDir?.Invoke(Direction);
                    }

                    lastKickTime = Time.time;

                    if (IsLeft)
                    {
                        //GMBodyCombat.Instance.combatMechanics.TrackerImpulse(HitType.kickLeft, Direction);
                        console.Log("KICK DETECTED: LEFT - Height: " + HeightAboveGround.ToString("F2") + "m, Vel: " + Velocity.ToString("F2") + "m/s, FwdVel: " + ForwardVelocity.ToString("F2") + "m/s");
                    }
                    else
                    {
                        //GMBodyCombat.Instance.combatMechanics.TrackerImpulse(HitType.kickRight, Direction);
                        console.Log("KICK DETECTED: RIGHT - Height: " + HeightAboveGround.ToString("F2") + "m, Vel: " + Velocity.ToString("F2") + "m/s, FwdVel: " + ForwardVelocity.ToString("F2") + "m/s");
                    }
                }

                // Debug stuff
                StringBuilder builder = new StringBuilder();

                builder.Append("Position:\t\t\t\t\t\t");
                builder.AppendLine(Position.ToString());
                builder.Append("HeightPosition:\t\t\t\t");
                builder.AppendLine(HeightPosition.ToString());
                builder.Append("HeightAboveGround:\t\t");
                builder.AppendLine(HeightAboveGround.ToString());
                builder.Append("Direction:\t\t\t\t\t\t");
                builder.AppendLine(Direction.ToString());
                builder.Append("MovementDelta:\t\t\t\t");
                builder.AppendLine(MovementDelta.ToString());
                builder.Append("Velocity:\t\t\t\t\t\t");
                builder.AppendLine(Velocity.ToString());
                builder.Append("ForwardVelocity:\t\t\t");
                builder.AppendLine(ForwardVelocity.ToString());
                builder.Append("TimeSinceKick:\t\t\t\t");
                builder.AppendLine((Time.time - lastKickTime).ToString());

                if (debugText != null)
                    debugText.text = builder.ToString();

                // Prepare for next frame
                previousPosition = Position;
                previousDirection = Direction;
                previousVelocity = Velocity;
            }
        }
    }
}
