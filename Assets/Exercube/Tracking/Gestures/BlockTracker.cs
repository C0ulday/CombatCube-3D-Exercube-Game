using UnityEngine;
using UnityEngine.Events;

namespace Sphery.ExerCube
{
    public class BlockTracker : MonoBehaviour
    {
        public Vector3 worldUp = Vector3.up;

        public float minHeight = 0.4f; // Reduced - more lenient
        public float maxHeight = 0.9f; // Increased - allow higher blocks
        public float maxDistance = 0.5f; // Increased - hands can be further from body
        public float maxHandDistance = 0.5f; // Increased - hands don't need to be super close
        public float blockCooldown = 0.2f; // Increased to reduce spam

        public UnityEvent blockDetected;
        public Vector3 Direction { get; private set; }

        public UnityEngine.UI.Text debugText; // Optional debug text display

        private float lastBlockTime = -10f;
        private bool wasBlocking = false;

        private ITrackingManager trackingManager;

        private HandTrackerData hands;
        private FootTrackerData feet;

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

            if (trackingManager == null)
            {
                Debug.LogError("No <i>" + nameof(ITrackingManager) + "</i> component found on player object!");
                return;
            }
        }

        private void Start()
        {
            hands = new HandTrackerData();
            hands.Update(trackingManager[Tracker.LeftWrist].Position, trackingManager[Tracker.RightWrist].Position, worldUp);

            feet = new FootTrackerData();
            feet.Update(trackingManager[Tracker.LeftAnkle].Position, trackingManager[Tracker.RightAnkle].Position, worldUp);

            Direction = (hands.Center - feet.Center).normalized;
        }

        private void Update()
        {
            hands.Update(trackingManager[Tracker.LeftWrist].Position, trackingManager[Tracker.RightWrist].Position, worldUp);
            feet.Update(trackingManager[Tracker.LeftAnkle].Position, trackingManager[Tracker.RightAnkle].Position, worldUp);

            // Calculate direction from feet to hands
            Direction = (hands.Center - feet.Center).normalized;

            // 1. Check if hands are close together (blocking stance, not arms spread)
            float handDistance = Vector3.Distance(hands.Left, hands.Right);
            bool handsTogetherEnough = handDistance <= maxHandDistance * trackingManager.PlayerSizeFactor;

            // 2. Check if hands are close to body center horizontally
            float horizontalDistance = Vector3.Distance(feet.Center, hands.Center);
            bool withinBlockDistance = horizontalDistance <= maxDistance * trackingManager.PlayerSizeFactor;

            // 3. Check if hands are at defensive height
            float height =
                (trackingManager[Tracker.LeftWrist].Position.y - trackingManager[Tracker.LeftAnkle].Position.y +
                trackingManager[Tracker.RightWrist].Position.y - trackingManager[Tracker.RightAnkle].Position.y) * 0.5f;
            bool withinBlockHeight = height <= maxHeight * trackingManager.PlayerSizeFactor && height >= minHeight * trackingManager.PlayerSizeFactor;

            // 4. Check cooldown
            bool canTriggerBlock = (Time.time - lastBlockTime) >= blockCooldown;

            // Detect block: all conditions must be met
            bool isBlocking = handsTogetherEnough && withinBlockDistance && withinBlockHeight;

            if (isBlocking && canTriggerBlock)
            {
                blockDetected?.Invoke();
                lastBlockTime = Time.time;
            }

            wasBlocking = isBlocking;

            // Debug stuff
            if (debugText != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append("HandDistance:\t\t\t\t");
                builder.AppendLine(handDistance.ToString("F2"));
                builder.Append("HandsTogether:\t\t\t");
                builder.AppendLine(handsTogetherEnough.ToString());
                builder.Append("HorizontalDist:\t\t\t");
                builder.AppendLine(horizontalDistance.ToString("F2"));
                builder.Append("WithinDistance:\t\t\t");
                builder.AppendLine(withinBlockDistance.ToString());
                builder.Append("Height:\t\t\t\t\t\t");
                builder.AppendLine(height.ToString("F2"));
                builder.Append("WithinHeight:\t\t\t\t");
                builder.AppendLine(withinBlockHeight.ToString());
                builder.Append("IsBlocking:\t\t\t\t\t");
                builder.AppendLine(isBlocking.ToString());

                debugText.text = builder.ToString();
            }
        }

        private class HandTrackerData
        {
            /// <summary>
            /// The position of the left hand (flattened).
            /// </summary>
            public Vector3 Left { get; private set; }
            /// <summary>
            /// The position of the right hand (flattened).
            /// </summary>
            public Vector3 Right { get; private set; }
            /// <summary>
            /// The center point between both hands (flattened).
            /// </summary>
            public Vector3 Center { get; private set; }

            /// <summary>
            /// Update the hans tracker data.
            /// </summary>
            /// <param name="left">The left hand tracker position.</param>
            /// <param name="right">The right hand tracker position.</param>
            /// <param name="worldUp">The world up vector used for flattening.</param>
            public void Update(Vector3 left, Vector3 right, Vector3 worldUp)
            {
                // Calculate hands
                Left = Vector3.ProjectOnPlane(left, worldUp);
                Right = Vector3.ProjectOnPlane(right, worldUp);

                Center = (Left + Right) * 0.5f;
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
