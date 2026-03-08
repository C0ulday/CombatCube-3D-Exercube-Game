using UnityEngine;

namespace Sphery.ExerCube.UI
{
    public class TrackerPointerProvider : TrackerPointerProviderBase
    {
        [SerializeField]
        protected BodyFactors bodyFactors;

        public Vector3 worldUp = Vector3.up;

        [Range(0f, 1f)]
        public float pointingFactor = 0.6f;
        [Range(0f, 1f)]
        public float directionLimit = 0.7f;

        protected ITrackingManager trackingManager;

        protected HandTrackerData activeTracker;

        protected HandTrackerData left;
        protected HandTrackerData right;

        protected virtual void Awake()
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

        protected virtual void Start()
        {
            // Calculate shoulder position
            Vector3 feetCenter = (trackingManager[Tracker.LeftAnkle].GlobalPosition + trackingManager[Tracker.RightAnkle].GlobalPosition) * 0.5f;
            Vector3 shoulderCenter = feetCenter + worldUp * bodyFactors.CalculatePlayerHeightFromCalibration(trackingManager.PlayerSizeFactor) * bodyFactors.shoulderHeight;

            // Setup hands
            left = new HandTrackerData(shoulderCenter, trackingManager[Tracker.LeftWrist].GlobalPosition);
            right = new HandTrackerData(shoulderCenter, trackingManager[Tracker.RightWrist].GlobalPosition);

            activeTracker = right;
        }

        protected virtual void Update()
        {
            // Calculate shoulder position
            Vector3 feetCenter = (trackingManager[Tracker.LeftAnkle].GlobalPosition + trackingManager[Tracker.RightAnkle].GlobalPosition) * 0.5f;
            Vector3 shoulderCenter = feetCenter + worldUp * bodyFactors.CalculatePlayerHeightFromCalibration(trackingManager.PlayerSizeFactor) * bodyFactors.shoulderHeight;

            // Update hands
            float armLength = pointingFactor * bodyFactors.CalculatePlayerHeightFromCalibration(trackingManager.PlayerSizeFactor) * bodyFactors.ArmLength;

            left.Update(shoulderCenter, trackingManager[Tracker.LeftWrist].GlobalPosition, armLength, worldUp, directionLimit);
            right.Update(shoulderCenter, trackingManager[Tracker.RightWrist].GlobalPosition, armLength, worldUp, directionLimit);

            // Get prioritized hand
            activeTracker = right;
            if (!right.IsPointing && left.IsPointing) activeTracker = left;
        }

        public override Vector3 GetPosition() => activeTracker.Position;
        public override Vector3 GetDirection() => activeTracker.Direction;

        public override bool IsPointing() => activeTracker.IsPointing;

        protected class HandTrackerData
        {
            public Vector3 Position { get; private set; }
            public Vector3 Direction { get; private set; }

            public bool IsPointing { get; private set; }

            private Vector3 previousPosition;

            public HandTrackerData(Vector3 shoulderCenter, Vector3 position)
            {
                // Initialize previous values
                previousPosition = position;
                Direction = (previousPosition - shoulderCenter).normalized;

                IsPointing = false;
            }

            public void Update(Vector3 shoulderCenter, Vector3 position, float armLength, Vector3 worldUp, float directionLimit)
            {
                // Update data
                Position = position;
                Direction = (Position - shoulderCenter).normalized;

                // Calculate pointing
                IsPointing =
                    Mathf.Abs(Vector3.Dot(Direction, worldUp)) <= directionLimit &&
                    Vector3.Distance(shoulderCenter, Position) >= armLength;

                // Prepare for next frame
                previousPosition = Position;
            }
        }
    }
}
