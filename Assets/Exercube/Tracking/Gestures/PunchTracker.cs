using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
namespace Sphery.ExerCube
{

    [System.Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }
    public class PunchTracker : MonoBehaviour
    {
        // Tacking zone based on feet
        // Punch is hand position relative to feet tracking zone
        // Punch power

        public Vector3 worldUp = Vector3.up;

        public float armPunchLength = 0.6f;
        public float minVelocity = 1.5f;
        [SerializeField] protected TMP_Text console;

        public UnityEvent leftPunchDetected;
        public UnityEvent rightPunchDetected;

        public Text leftDebugText;
        public Text rightDebugText;

        private ActivityConsole Console;
        private ITrackingManager trackingManager;

        private FootTrackerData feet;

        private HandTrackerData left;
        private HandTrackerData right;

        [Header("Directional Events")]
        public Vector3Event leftPunchWithDir;
        public Vector3Event rightPunchWithDir;

        private void Awake()
        {
            // Find player
            GameObject player = GameObject.FindWithTag("Player");
            Console = new ActivityConsole(console);

            if (player == null)
            {
                Debug.LogError("No player object found in scene!");
                return;
            }

            // Find tracking component
            trackingManager = player.GetComponentInChildren<ITrackingManager>();

            if (trackingManager == null)
            {
                Debug.LogError("No <i>" + nameof(ITrackingManager) + "</i> component found on player object!");
                return;
            }
        }

        private void Start()
        {
            feet = new FootTrackerData();
            feet.Update(trackingManager[Tracker.LeftAnkle].Position, trackingManager[Tracker.RightAnkle].Position, worldUp);

            left = new HandTrackerData(true, feet, trackingManager[Tracker.LeftWrist].Position, worldUp);
            right = new HandTrackerData(false, feet, trackingManager[Tracker.RightWrist].Position, worldUp);
        }

        private void Update()
        {
            feet.Update(trackingManager[Tracker.LeftAnkle].Position, trackingManager[Tracker.RightAnkle].Position, worldUp);

            float armLength = armPunchLength * trackingManager.PlayerSizeFactor * 0.5f; // One meter arm length for 2 meter height = 0.5f

            //Console.Log("RightWrist: " + trackingManager[Tracker.RightWrist].Position);
            //Console.Log("LeftWrist: " + trackingManager[Tracker.LeftWrist].Position);




            left.Update(feet, trackingManager[Tracker.LeftWrist].Position, worldUp, armLength, minVelocity, leftPunchDetected, leftDebugText, Console);
            right.Update(feet, trackingManager[Tracker.RightWrist].Position, worldUp, armLength, minVelocity, rightPunchDetected, rightDebugText, Console);
        }

        private class HandTrackerData
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
            /// The current offset (in units) to the previous position.
            /// </summary>
            public float MovementDelta { get; private set; }
            /// <summary>
            /// The distance of position to feet center.
            /// </summary>
            public float DistanceToCenter { get; private set; }
            /// <summary>
            /// The accumulated distance in the current direction direction (in units).
            /// </summary>
            public float AccumulatedDistance { get; private set; }

            private bool hasPunched = false;

            private Vector3 previousPosition;
            private Vector3 previousDirection;
            private float previousVelocity;

            /// <summary>
            /// Create a new instance of hand tracker data.
            /// </summary>
            /// <param name="feet">The feet tracker data to use.</param>
            /// <param name="position">The position of the tracker.</param>
            /// <param name="worldUp">The world up vector used for flattening.</param>
            public HandTrackerData(bool isLeft, FootTrackerData feet, Vector3 position, Vector3 worldUp)
            {
                IsLeft = isLeft;

                // Initialize previous values
                previousPosition = Vector3.ProjectOnPlane(position, worldUp);
                Direction = (previousPosition - feet.Center).normalized;
                previousVelocity = 0f;

                MovementDelta = 0f;
                AccumulatedDistance = 0f;
            }

            /// <summary>
            /// Update the hand tracker data.
            /// </summary>
            /// <param name="feet">The feet tracker data to use.</param>
            /// <param name="position">The position of the tracker.</param>
            /// <param name="worldUp">The world up vector used for flattening.</param>
            /// <param name="armLength">The length to check accumulated distance against. If accumulated distance is larger a punch is registered.</param>
            public void Update(FootTrackerData feet, Vector3 position, Vector3 worldUp, float armLength, float minVelocity, UnityEvent punchEvent, Text debugText, ActivityConsole console)
            {
                Position = Vector3.ProjectOnPlane(position, worldUp);
                Direction = (Position - feet.Center).normalized;

                MovementDelta = Vector3.Distance(previousPosition, Position);
                Velocity = MovementDelta / Time.deltaTime; // Extrapolate to unit per second

                // Check for direction change (direction changes if outside +/- 45 degrees of previous direction)
                if (Vector3.Dot(Direction, previousDirection) < 0.5f)
                {
                    // Reset distance
                    AccumulatedDistance = 0f;
                }

                // Check for velocity change
                if (Velocity > previousVelocity) // Accelerated
                {
                    //console.Log("Accelerated");
                }
                else if (Velocity < previousVelocity) // Decelerated
                {
                    //console.Log("Deccelerated");
                }

                // Add to accumulated distance
                AccumulatedDistance += MovementDelta;
                DistanceToCenter = Vector3.Distance(feet.Center, Position);

                if (IsLeft) { 
                //console.Log("LeftWrist: " + Position);
                }
                else
                {
                //console.Log("RightWrist: " + Position);
                }



                // Detect punch
                if (DistanceToCenter >= armLength && Velocity >= minVelocity && !hasPunched)
                {
                    Vector3 punchVelocityDir = (Position - previousPosition).normalized;
                    punchEvent?.Invoke();
                    // Play punch sound
                    AudioManager.Instance.PlayPunch();

                    //if (IsLeft) leftPunchWithDir?.Invoke(Direction);
                    //else rightPunchWithDir?.Invoke(Direction);

                    if (IsLeft)
                    {
                        // Nutze FindObjectOfType (ohne 'Any' und 'ByType')
                        //GameObject.FindObjectOfType<PunchTracker>().leftPunchWithDir?.Invoke(Direction);
                        GameObject.FindObjectOfType<PunchTracker>().leftPunchWithDir?.Invoke(punchVelocityDir);
                    }
                    else
                    {
                        //GameObject.FindObjectOfType<PunchTracker>().rightPunchWithDir?.Invoke(Direction);
                        GameObject.FindObjectOfType<PunchTracker>().rightPunchWithDir?.Invoke(punchVelocityDir);
                    }

                   


                    if (IsLeft)
                    {
                        //console.Log("PUNCH DETECTED: LEFT");

                        //GMBodyCombat.Instance.combatMechanics.TrackerImpulse(HitType.punchLeft, Direction); // TODO: Add bool parameter to function and reference IsLeft
                    }
                    else
                    {
                        //console.Log("PUNCH DETECTED: RIGHT");

                        //GMBodyCombat.Instance.combatMechanics.TrackerImpulse(HitType.punchRight, Direction); // TODO: Add bool parameter to function and reference IsLeft
                    }

                    hasPunched = true;
                }

                // Reset punch if possible
                if (DistanceToCenter < armLength)
                    hasPunched = false;

                // Debug stuff
                StringBuilder builder = new StringBuilder();

                builder.Append("Foot left:\t\t\t\t\t\t");
                builder.AppendLine(feet.Left.ToString());
                builder.Append("Foot right:\t\t\t\t\t\t");
                builder.AppendLine(feet.Right.ToString());
                builder.Append("Foot center:\t\t\t\t\t");
                builder.AppendLine(feet.Center.ToString());

                builder.Append("Position:\t\t\t\t\t\t");
                builder.AppendLine(Position.ToString());
                builder.Append("Direction;\t\t\t\t\t\t");
                builder.AppendLine(Direction.ToString());
                builder.Append("MovementDelta;\t\t\t\t");
                builder.AppendLine(MovementDelta.ToString());
                builder.Append("Velocity;\t\t\t\t\t\t");
                builder.AppendLine(Velocity.ToString());
                builder.Append("AccumulatedDistance;\t");
                builder.AppendLine(AccumulatedDistance.ToString());
                builder.Append("DistanceToCenter;\t\t\t");
                builder.AppendLine(DistanceToCenter.ToString());

                if (debugText != null)
                    debugText.text = builder.ToString();


                // Prepare for next frame
                previousPosition = Position;
                previousDirection = Direction;
                previousVelocity = Velocity;
            }
        }

        private class FootTrackerData
        {
            /// <summary>
            /// The position of the left foot (flattened).
            /// </summary>
            public Vector3 Left { get; private set; }
            /// <summary>
            /// The position of the right foot (flattened).
            /// </summary>
            public Vector3 Right { get; private set; }
            /// <summary>
            /// The center point between both feet (flattened).
            /// </summary>
            public Vector3 Center { get; private set; }

            /// <summary>
            /// Update the foot tracker data.
            /// </summary>
            /// <param name="left">The left ankle tracker position.</param>
            /// <param name="right">The right ankle tracker position.</param>
            /// <param name="worldUp">The world up vector used for flattening.</param>
            public void Update(Vector3 left, Vector3 right, Vector3 worldUp)
            {
                // Calculate feet
                Left = Vector3.ProjectOnPlane(left, worldUp);
                Right = Vector3.ProjectOnPlane(right, worldUp);

                Center = (Left + Right) * 0.5f;
            }
        }
    }
}
