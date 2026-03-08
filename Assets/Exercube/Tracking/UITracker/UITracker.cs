using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sphery.ExerCube.UI
{
    [System.Obsolete("Use " + nameof(TrackedPointerManager) + " instead.")]
    public class UITracker : MonoBehaviour
    {
        public bool useTestTracker;
        public Transform testTracker;

        public TrackedCanvas trackedCanvas;

        public RectTransform parent;
        public RectTransform indicator;

        public BodyFactors bodyFactors;
        public Vector3 worldUp = Vector3.up;

        public Canvas canvas;

        private ISubmitHandler _target;

        public Image timeIndicator;
        public float timeToActivate = 2f;
        private float _currentTimeToActivate = 0f;

        public CanvasGroup canvasGroup;
        public float canvasAlphaSpeed = 0.5f;

        [Range(0f, 1f)] public float armPunchLength = 0.6f;
        [Range(0f, 1f)] public float directionLimit = 0.7f;

        private ITrackingManager trackingManager;

        private SharedTrackerData shared;

        private HandTrackerData left;
        private HandTrackerData right;

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
            shared = new SharedTrackerData();
            shared.Update(trackingManager, bodyFactors, worldUp);

            left = new HandTrackerData(shared, trackingManager[Tracker.LeftWrist].GlobalPosition);
            right = new HandTrackerData(shared, trackingManager[Tracker.RightWrist].GlobalPosition);

        }

        private void LateUpdate()
        {
            shared.Update(trackingManager, bodyFactors, worldUp);

            float armLength = armPunchLength * bodyFactors.CalculatePlayerHeightFromCalibration(trackingManager.PlayerSizeFactor) * bodyFactors.ArmLength;

            // TODO: Calculate frustum corners? https://docs.unity3d.com/ScriptReference/Camera.CalculateFrustumCorners.html
            //Vector3[] frustumCorners = new Vector3[4];
            //canvas.worldCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), canvas.worldCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

            left.Update(shared, trackingManager[Tracker.LeftWrist].GlobalPosition, armLength, worldUp, directionLimit);
            right.Update(shared, trackingManager[Tracker.RightWrist].GlobalPosition, armLength, worldUp, directionLimit);

            if (!useTestTracker)
            {
                HandTrackerData handTracker = right;
                if (!right.IsPointing && left.IsPointing) handTracker = left;

                RaycastToUI(canvas, new Ray(handTracker.Position, handTracker.Direction), handTracker.IsPointing);
            }
            else
            {
                // Empty object based test
                RaycastToUI(canvas, new Ray(testTracker.position, testTracker.forward), true);
            }

            // Update visuals
            canvasGroup.alpha = Mathf.MoveTowards
            (
                canvasGroup.alpha,
                (left.IsPointing || right.IsPointing) ? 1f : 0f,
                canvasAlphaSpeed * Time.deltaTime
            );
            timeIndicator.fillAmount = _currentTimeToActivate / timeToActivate;
        }

        private void RaycastToUI(Canvas canvas, Ray ray, bool isPointing)
        {
            // Reset data
            ISubmitHandler oldTarget = _target;
            _target = null;

            // Check for new submit handler
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            if (RayToScreenPoint(canvas.worldCamera, ray, out Vector3 screenPoint))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, canvas.worldCamera, out Vector2 localPoint);
                indicator.anchoredPosition = localPoint;

                pointerData.position = screenPoint;
                _target = trackedCanvas.GetTrackedElement(screenPoint).SubmitHandler;
            }

            // Handle hovering
            if (isPointing && oldTarget != null && oldTarget == _target)
                _currentTimeToActivate += Time.deltaTime;
            else
                _currentTimeToActivate = 0f;

            // Handle submit
            if (_currentTimeToActivate >= timeToActivate)
            {
                _target.OnSubmit(pointerData);
                _currentTimeToActivate = 0f;
            }
        }

        private bool RayToScreenPoint(Camera camera, Ray ray, out Vector3 screenPoint)
        {
            // Calculate camera far plane
            Plane cameraFarPlane = new Plane
            (
                -camera.transform.forward,
                camera.transform.position + camera.transform.forward * 1.1f // 1.1f is approximate distance to wall
            );

            // Raycast ray onto plane and convert result to screen point
            if (cameraFarPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                screenPoint = camera.WorldToScreenPoint(worldPoint);
                return true;
            }

            // Raycast failed
            screenPoint = Vector3.zero;
            return false;
        }

        private class HandTrackerData
        {
            public Vector3 Position { get; private set; }
            public Vector3 Direction { get; private set; }

            public float DistanceToShoulder { get; private set; }

            public bool IsPointing { get; private set; }

            private Vector3 previousPosition;

            public HandTrackerData(SharedTrackerData feet, Vector3 position)
            {
                // Initialize previous values
                previousPosition = position;
                Direction = (previousPosition - feet.ShoulderCenter).normalized;

                IsPointing = false;
            }

            public void Update(SharedTrackerData shared, Vector3 position, float armLength, Vector3 worldUp, float directionLimit)
            {
                Position = position;
                Direction = (Position - shared.ShoulderCenter).normalized;

                // Calculate pointing
                DistanceToShoulder = Vector3.Distance(shared.ShoulderCenter, Position);
                IsPointing = Mathf.Abs(Vector3.Dot(Direction, worldUp)) <= directionLimit && DistanceToShoulder >= armLength;

                // Prepare for next frame
                previousPosition = Position;
            }
        }

        private class SharedTrackerData
        {
            public Vector3 LeftAnkle { get; private set; }
            public Vector3 RightAnkle { get; private set; }
            public Vector3 FeetCenter { get; private set; }

            public Vector3 ShoulderCenter { get; private set; }

            public void Update(ITrackingManager trackingManager, BodyFactors bodyFactors, Vector3 worldUp)
            {
                // Calculate feet
                LeftAnkle = trackingManager[Tracker.LeftAnkle].GlobalPosition;
                RightAnkle = trackingManager[Tracker.RightAnkle].GlobalPosition;

                FeetCenter = (LeftAnkle + RightAnkle) * 0.5f;

                ShoulderCenter = FeetCenter + worldUp * bodyFactors.CalculatePlayerHeightFromCalibration(trackingManager.PlayerSizeFactor) * bodyFactors.shoulderHeight;
            }
        }
    }
}
