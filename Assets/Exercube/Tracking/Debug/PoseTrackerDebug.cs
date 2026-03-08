using UnityEngine;
using UnityEngine.Events;
using Koboldgames.Primitives.Variables;

namespace Sphery.ExerCube
{
    public class PoseTrackerDebug : MonoBehaviour
    {
        [Header("Available Poses")]
        [SerializeField] protected Pose[] poses;
        [SerializeField] protected SharedPose currentPose;

        [Header("Tracker Visualisation")]
        [SerializeField] protected Transform trackerRightHand;
        [SerializeField] protected Transform trackerLeftHand;
        [SerializeField] protected Transform trackerRightAnkle;
        [SerializeField] protected Transform trackerLeftAnkle;

        [Header("Distance Visualisation")]
        [SerializeField] protected LineRenderer distanceHands;
        [SerializeField] protected LineRenderer distanceAnkles;
        [SerializeField] protected LineRenderer distanceHandsAnkles;

        [Header("AABB Tracking Boxes")]
        [SerializeField] protected Transform areaRightHand;
        [SerializeField] protected Transform areaLeftHand;
        [SerializeField] protected Transform areaRightAnkle;
        [SerializeField] protected Transform areaLeftAnkle;

        [Header("Calibrated Position Correction")]
        [SerializeField] protected SharedVector3 localPositionCorrection;

        [Header("Rotatable Camera Rig")]
        [SerializeField] protected Transform cameraRig;

        [Header("Tracker Indicator Colors")]
        [SerializeField] protected Color validColor = Color.green;
        [SerializeField] protected Color invalidColor = Color.red;

        [Header("Tracking Boxes Colors")]
        [SerializeField] protected Color colorRightHand = new Color(0f, 0.8333333f, 1f);
        [SerializeField] protected Color colorLeftHand = new Color(0f, 0.8333333f, 1f);
        [SerializeField] protected Color colorRightAnkle = new Color(0f, 0.8333333f, 1f);
        [SerializeField] protected Color colorLeftAnkle = new Color(0f, 0.8333333f, 1f);

        [Header("Events")]
        [SerializeField] protected UnityEvent onActivate;
        [SerializeField] protected UnityEvent onDeactivate;
        [SerializeField] protected OnChangeEvent onPoseChange;

        [Header("Input")]
        [SerializeField] protected KeyCode activateKeyCode = KeyCode.F3;
        [SerializeField] protected KeyCode deactivateKeyCode = KeyCode.Tab;
        [SerializeField] protected KeyCode leftKeyCode = KeyCode.LeftArrow;
        [SerializeField] protected KeyCode rightKeyCode = KeyCode.RightArrow;
        [SerializeField] protected KeyCode rotateLeftKeyCode = KeyCode.Comma;
        [SerializeField] protected KeyCode rotateRightKeyCode = KeyCode.Period;

        protected ITrackingManager poseTracker;

        protected MeshRenderer rightHandIndicator;
        protected MeshRenderer leftHandIndicator;
        protected MeshRenderer rightAnkleIndicator;
        protected MeshRenderer leftAnkleIndicator;

        protected Bounds rightHandArea;
        protected Bounds leftHandArea;
        protected Bounds rightAnkleArea;
        protected Bounds leftAnkleArea;

        private bool active;
        private int currentPoseIndex;

        /// <summary>
        /// Setup AABB tracking boxes visualisation.
        /// </summary>
        /// <param name="pose">The pose to read the tracking boxes data from.</param>
        public void SetupAreas(Pose pose = null)
        {
            if (!active)
                return;

            if (pose == null)
                pose = currentPose.Value as Pose;

            // Player size calculation
            // Mirrored frompose tracker logic, adapt if necessary
            float size = Mathf.Clamp(poseTracker.PlayerSizeFactor, 1.5f, 2.3f);
            float posMulti = (size - 1.5f) * 0.25f + 0.8f;

            rightHandArea = pose.HandRight;
            leftHandArea = pose.HandLeft;
            rightAnkleArea = pose.AnkleRight;
            leftAnkleArea = pose.AnkleLeft;

            // Adapt positions according to players size and set tolerance
            rightHandArea.center = new Vector3(rightHandArea.center.x * posMulti, rightHandArea.center.y * posMulti, rightHandArea.center.z);
            rightHandArea.center -= localPositionCorrection;
            rightHandArea.extents = new Vector3(rightHandArea.extents.x, rightHandArea.extents.y * posMulti, rightHandArea.extents.z);
            rightHandArea.Expand(poseTracker.ExtraToleranceHands);

            leftHandArea.center = new Vector3(leftHandArea.center.x * posMulti, leftHandArea.center.y * posMulti, leftHandArea.center.z);
            leftHandArea.center -= localPositionCorrection;
            leftHandArea.extents = new Vector3(leftHandArea.extents.x, leftHandArea.extents.y * posMulti, leftHandArea.extents.z);
            leftHandArea.Expand(poseTracker.ExtraToleranceHands);

            rightAnkleArea.center = new Vector3(rightAnkleArea.center.x * posMulti, rightAnkleArea.center.y * posMulti, rightAnkleArea.center.z);
            rightAnkleArea.center -= localPositionCorrection;
            rightAnkleArea.extents = new Vector3(rightAnkleArea.extents.x, rightAnkleArea.extents.y * posMulti, rightAnkleArea.extents.z);
            rightAnkleArea.Expand(poseTracker.ExtraToleranceAnkles);

            leftAnkleArea.center = new Vector3(leftAnkleArea.center.x * posMulti, leftAnkleArea.center.y * posMulti, leftAnkleArea.center.z);
            leftAnkleArea.center -= localPositionCorrection;
            leftAnkleArea.extents = new Vector3(leftAnkleArea.extents.x, leftAnkleArea.extents.y * posMulti, leftAnkleArea.extents.z);
            leftAnkleArea.Expand(poseTracker.ExtraToleranceAnkles);

            // Apply on debug area boxes
            areaRightHand.localPosition = rightHandArea.center;
            areaRightHand.localScale = rightHandArea.size;

            areaLeftHand.localPosition = leftHandArea.center;
            areaLeftHand.localScale = leftHandArea.size;

            areaRightAnkle.localPosition = rightAnkleArea.center;
            areaRightAnkle.localScale = rightAnkleArea.size;

            areaLeftAnkle.localPosition = leftAnkleArea.center;
            areaLeftAnkle.localScale = leftAnkleArea.size;

            onPoseChange?.Invoke(pose);
        }

        /// <summary>
        /// Activate the pose tracking debug visualisation.
        /// </summary>
        public void Activate()
        {
            if (active || !(poses?.Length > 0))
                return;

            cameraRig.localEulerAngles = Vector3.zero;
            cameraRig.gameObject.SetActive(true);
            active = true;
            currentPoseIndex = 0;
            currentPose.Value = poses[0];

            // Automatically start active mapping when no initialized trackers detected
            if (poseTracker.TrackerCount < 2)
                poseTracker.StartActiveMapping();

            onActivate?.Invoke();
            SetupAreas();
        }

        /// <summary>
        /// Deactivate the pose tracking debug visualisation.
        /// </summary>
        public void Deactivate()
        {
            if (!active)
                return;

            cameraRig.gameObject.SetActive(false);
            active = false;

            onDeactivate?.Invoke();
        }

        /// <summary>
        /// Gets the raw position of a tracker considering the local position correction.
        /// </summary>
        /// <param name="position">The read position of a tracker.</param>
        /// <returns>The raw position, subtracting the position correction of the given position.</returns>
        protected Vector3 GetRawPosition(Vector3 position) => position - localPositionCorrection;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            bool trackerAssigned = (trackerRightHand != null) &&
                                   (trackerLeftHand != null) &&
                                   (trackerRightAnkle != null) &&
                                   (trackerLeftAnkle != null);

            bool linesAssigned = (distanceHands != null) &&
                                 (distanceAnkles != null) &&
                                 (distanceHandsAnkles != null);

            bool areasAssigned = (areaRightHand != null) &&
                                 (areaLeftHand != null) &&
                                 (areaRightAnkle != null) &&
                                 (areaLeftAnkle != null);

            if (currentPose == null)
                Debug.LogError("[PoseTrackerDebug] No 'current pose' variable assigned!");

            if (!trackerAssigned)
                Debug.LogError("[PoseTrackerDebug] Not all tracker indicator objects are assigned!");

            if (!linesAssigned)
                Debug.LogError("[PoseTrackerDebug] Not all tracker distance indicator objects are assigned!");

            if (!areasAssigned)
                Debug.LogError("[PoseTrackerDebug] Not all box area indicator objects are assigned!");

            if (localPositionCorrection == null)
                Debug.LogError("[PoseTrackerDebug] No local position correction variable assigned!");

            if (cameraRig == null)
                Debug.LogError("[PoseTrackerDebug] No camera rig reference assigned!");

            // Reference pose tracker
            GameObject player = GameObject.FindWithTag("Player");

            // Find player pose tracker
            if (player != null)
            {
                poseTracker = player.GetComponentInChildren<ITrackingManager>();

                if (poseTracker == null)
                    Debug.LogError("[PoseTrackerDebug] No pose-tracker component found on player object!");
            }
            else
            {
                Debug.LogError("[PoseTrackerDebug] No player object found in scene!");
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            rightHandIndicator = trackerRightHand.GetComponent<MeshRenderer>();
            leftHandIndicator = trackerLeftHand.GetComponent<MeshRenderer>();
            rightAnkleIndicator = trackerRightAnkle.GetComponent<MeshRenderer>();
            leftAnkleIndicator = trackerLeftAnkle.GetComponent<MeshRenderer>();

            areaRightHand.GetComponent<MeshRenderer>().material.color = colorRightHand;
            areaLeftHand.GetComponent<MeshRenderer>().material.color = colorLeftHand;
            areaRightAnkle.GetComponent<MeshRenderer>().material.color = colorRightAnkle;
            areaLeftAnkle.GetComponent<MeshRenderer>().material.color = colorLeftAnkle;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!active)
            {
                if (Input.GetKeyDown(activateKeyCode))
                    Activate();
                else
                    return;
            }

            // Tracker positions
            trackerRightHand.localPosition = GetRawPosition(poseTracker[Tracker.RightWrist].Position);
            trackerLeftHand.localPosition = GetRawPosition(poseTracker[Tracker.LeftWrist].Position);
            trackerRightAnkle.localPosition = GetRawPosition(poseTracker[Tracker.RightAnkle].Position);
            trackerLeftAnkle.localPosition = GetRawPosition(poseTracker[Tracker.LeftAnkle].Position);

            Vector3 midHands = (trackerRightHand.localPosition - trackerLeftHand.localPosition) * 0.5f + trackerLeftHand.localPosition;
            Vector3 midAnkles = (trackerRightAnkle.localPosition - trackerLeftAnkle.localPosition) * 0.5f + trackerLeftAnkle.localPosition;

            // Tracker line positions
            distanceHands.SetPosition(0, trackerRightHand.localPosition);
            distanceHands.SetPosition(1, trackerLeftHand.localPosition);
            distanceAnkles.SetPosition(0, trackerRightAnkle.localPosition);
            distanceAnkles.SetPosition(1, trackerLeftAnkle.localPosition);
            distanceHandsAnkles.SetPosition(0, midHands);
            distanceHandsAnkles.SetPosition(1, midAnkles);

            bool rightHandValid = rightHandArea.Contains(trackerRightHand.localPosition);
            bool leftHandValid = leftHandArea.Contains(trackerLeftHand.localPosition);
            bool rightAnkleValid = rightAnkleArea.Contains(trackerRightAnkle.localPosition);
            bool leftAnkleValid = leftAnkleArea.Contains(trackerLeftAnkle.localPosition);

            float handDist = poses[currentPoseIndex].HandDistance;              // Minimal distance between hands in this pose
            float ankleDist = poses[currentPoseIndex].AnkleDistance;            // Minimal distance between ankles in this pose
            float handAnkleDist = poses[currentPoseIndex].HandAnkleDistance;    // Minimal distance between hands to ankles in this pose

            // Measure distance between trackers
            float handToHand = Vector3.Distance(trackerRightHand.localPosition, trackerLeftHand.localPosition);
            float ankleToAnkle = Vector3.Distance(trackerRightAnkle.localPosition, trackerLeftAnkle.localPosition);
            float handsToAnkles = Vector3.Distance(midHands, midAnkles);

            // Tracker indicator colors
            rightHandIndicator.material.color = rightHandValid ? validColor : invalidColor;
            leftHandIndicator.material.color = leftHandValid ? validColor : invalidColor;
            rightAnkleIndicator.material.color = rightAnkleValid ? validColor : invalidColor;
            leftAnkleIndicator.material.color = leftAnkleValid ? validColor : invalidColor;

            // Tracker distance colors
            distanceHands.material.color = handDist <= handToHand ? validColor : invalidColor;
            distanceAnkles.material.color = ankleDist <= ankleToAnkle ? validColor : invalidColor;
            distanceHandsAnkles.material.color = handAnkleDist <= handsToAnkles ? validColor : invalidColor;

            // Cycle through poses
            if (Input.GetKeyDown(rightKeyCode))
            {
                currentPoseIndex++;

                if (currentPoseIndex >= poses.Length)
                    currentPoseIndex = 0;

                currentPose.Value = poses[currentPoseIndex];
                SetupAreas();
            }
            else if (Input.GetKeyDown(leftKeyCode))
            {
                currentPoseIndex--;

                if (currentPoseIndex < 0)
                    currentPoseIndex = poses.Length - 1;

                currentPose.Value = poses[currentPoseIndex];
                SetupAreas();
            }
            // Rotate right
            else if (Input.GetKey(rotateRightKeyCode))
            {
                cameraRig.localEulerAngles -= Vector3.up * Time.deltaTime * 100f;
            }
            // Rotate left
            else if (Input.GetKey(rotateLeftKeyCode))
            {
                cameraRig.localEulerAngles += Vector3.up * Time.deltaTime * 100f;
            }
            // Exit debug
            else if (Input.GetKeyDown(deactivateKeyCode))
            {
                Deactivate();
            }
        }

        [System.Serializable]
        protected class OnChangeEvent : UnityEvent<Pose> { }
    }
}
