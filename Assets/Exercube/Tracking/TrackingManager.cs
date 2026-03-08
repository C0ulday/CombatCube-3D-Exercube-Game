using System;
using UnityEngine;
using UnityEngine.Events;
using Koboldgames.Primitives.Variables;
using Valve.VR;
using System.Collections.Generic;

namespace Sphery.ExerCube
{
    public interface ITrackingManager
    {
        ITrackerData this[Tracker tracker] { get; }

        bool TrackingEnabled { get; }
        int TrackerCount { get; }
        bool TrackersAreValid { get; }
        float ExtraToleranceHands { get; }
        float ExtraToleranceAnkles { get; }
        float PlayerSizeFactor { get; }
        float AnkleGroundHeight { get; }
        Vector3 LocalPositionCorrection { get; }
        bool ValidationRunning { get; set; }
        bool SearchAndMappingActive { get; }

        void StartActiveMapping();
        void StopActiveMapping();
        void ApplyMap(int[] map);
        int[] MapTrackers();
        void FloorHeightCorrection();
        bool HasValidData(Tracker tracker);
        bool IsPoseMatching(Pose pose, bool useSecondary = false);
        ValueTuple<int, int, int, int> GetOscillationData(Pose pose, bool useSecondary = false);
    }

    public class TrackingManager : MonoBehaviour, ITrackingManager
    {
        [SerializeField] protected FlexibleFloat percentalMappingTime = new FlexibleFloat(2f);
        [SerializeField] protected FlexibleFloat playerSizeFactor = new FlexibleFloat(1f);
        [SerializeField] protected FlexibleFloat ankleGroundHeight = new FlexibleFloat(0.2f);
        [SerializeField] protected FlexibleVector3 localPositionCorrection;
        [SerializeField] protected FlexibleFloat extraToleranceHands = new FlexibleFloat(0f);
        [SerializeField] protected FlexibleFloat extraToleranceAnkles = new FlexibleFloat(0f);

        [SerializeField] protected FlexibleBoolean validationActive = new FlexibleBoolean(false);
        [SerializeField] protected FlexibleFloat validateInterval = new FlexibleFloat(5f);
        [SerializeField] protected FlexibleInteger invalidAfterCount = new FlexibleInteger(3);
        [SerializeField] protected UnityEvent onBecomeInvalid;
        [SerializeField] protected UnityEvent onBecomeValid;

        protected int trackerCount = 0;
        protected int[] trackerMap;
        protected SteamVR_TrackedObject[] trackerList = new SteamVR_TrackedObject[4];

        protected Dictionary<Tracker, TrackerData> trackers = new Dictionary<Tracker, TrackerData>();
        public ITrackerData this[Tracker tracker] { get => trackers[tracker]; }

        private IViveControllers vive;
        private Vector4[] positionProbability;
        private bool activeMapping = false;

        private float validationIntervalTime = 0f;
        private int validationCount = 0;
        private bool previousValid = true;

        /// <summary>
        /// Gets a value indicating if the ExerCube's tracking system is enabled.
        /// </summary>
        /// <value><c>true</c> if tracking is enable; otherwise <c>false</c>.</value>
        public bool TrackingEnabled => trackerCount > 0;

        /// <summary>
        /// Gets amount of tracked devices.
        /// </summary>
        public int TrackerCount => trackerCount;

        /// <summary>
        /// Gets a value indicating the validity of the trackers.
        /// </summary>
        /// <value><c>true</c> if all trackers are currently valid; otherwise <c>false</c>.</value>
        public bool TrackersAreValid => validationActive.Value && previousValid;

        /// <summary>
        /// Gets the extra tolerance for wrists.
        /// </summary>
        /// <value>The extra tolerance for wrists.</value>
        public float ExtraToleranceHands => extraToleranceHands;

        /// <summary>
        /// Gets the extra tolerance for ankles.
        /// </summary>
        /// <value>The extra tolerance for ankles.</value>
        public float ExtraToleranceAnkles => extraToleranceAnkles;

        /// <summary>
        /// Gets the player size factor value used for calibration.
        /// </summary>
        /// <value>The player size factor value.</value>
        public float PlayerSizeFactor => playerSizeFactor.Value;

        /// <summary>
        /// Gets the defined height where the ankles should be located above the ground.
        /// </summary>
        /// <value>The height of the ankle trackers to the ground.</value>
        public float AnkleGroundHeight => ankleGroundHeight.Value;

        /// <summary>
        /// Gets the local position correction
        /// </summary>
        /// <value>The plocal osition correction.</value>
        public Vector3 LocalPositionCorrection => localPositionCorrection;

        /// <summary>
        /// Gets or sets a value indicating if the tracker validation routine is currently running.
        /// </summary>
        /// <value><c>true</c> if the validation routine is running; otherwise <c>false</c>.</value>
        public bool ValidationRunning
        {
            get { return validationActive.Value; }
            set { validationActive.Value = value; }
        }

        /// <summary>
        /// Gets a value indicating if a tracker search and mapping process is running.
        /// </summary>
        /// <value><c>true</c> if a tracker search and mapping process is running; otherwise <c>false</c>.</value>
        public bool SearchAndMappingActive => activeMapping;

        #region Tracker Mapping

        /// <summary>
        /// Start active mapping process.
        /// </summary>
        public void StartActiveMapping()
        {
            vive.StartSearch();
            ApplyMap(MapTrackers());
            activeMapping = true;
        }

        /// <summary>
        /// Stop active mapping process.
        /// </summary>
        public void StopActiveMapping()
        {
            vive.StopSearch();
            activeMapping = false;
            positionProbability = null;
        }

        /// <summary>
        /// Apply a given map to map the trackers to the right position.
        /// </summary>
        /// <param name="map">The map to apply.</param>
        public void ApplyMap(int[] map)
        {
            if (map == null)
                return;

            // Save reference to tracker map
            trackerMap = map;
            vive.GetTrackerList(ref trackerList);

            // [0] index of right hand
            // [1] index of left hand
            // [2] index of right ankle
            // [3] index of left ankle
            trackers[Tracker.RightWrist].Transform = trackerList[map[0]].transform ?? null;
            trackers[Tracker.LeftWrist].Transform = trackerList[map[1]].transform ?? null;
            trackers[Tracker.RightAnkle].Transform = trackerList[map[2]].transform ?? null;
            trackers[Tracker.LeftAnkle].Transform = trackerList[map[3]].transform ?? null;

            // Apply floor correction
            FloorHeightCorrection();
        }

        /// <summary>
        /// Map the trackers to the right limbs (arms / ankles).
        /// This mapping aussumes that the player is standing in the Exercube, facing to the front view.
        /// </summary>
        /// <returns>The amount of trackers that have been mapped (0, 2 or 4).</returns>
        public int[] MapTrackers()
        {
            // [0] index of right hand
            // [1] index of left hand
            // [2] index of right ankle
            // [3] index of left ankle
            int[] tempMap = new int[4] { 0, 1, 2, 3 };
            float[] tempVal = new float[4];

            // Swap two fields (by given indicies) of the 'map' and 'value' array
            Action<int, int> swap = (i1, i2) =>
            {
                int swapMap = tempMap[i1];
                float swapVal = tempVal[i1];

                // Swap
                tempMap[i1] = tempMap[i2];
                tempVal[i1] = tempVal[i2];
                tempMap[i2] = swapMap;
                tempVal[i2] = swapVal;
            };

            // Recursively swap the fields of the 'map' and 'value' array, until the
            // field with the highest value stays on the 'start' index.
            Action<int> swapHighest = (start) =>
            {
                for (int k = start + 1; k < 4; k++)
                {
                    if (tempVal[start] < tempVal[k])
                        swap(start, k);
                }
            };

            // Gather indexed tracker count
            trackerCount = vive.IndexedTrackers;

            // Gather tracker list
            vive.GetTrackerList(ref trackerList);

            // Check for 4 or 2 trackers
            if (trackerCount > 4)
                trackerCount = 4;
            else if ((trackerCount % 2) > 0)
                trackerCount--;

            // No trackers (or only one)?
            if (trackerCount == 0)
                return null;

            // Valid data of the indexed trackers?
            for (int i = 0; i < trackerCount; i++)
            {
                if (trackerList[i] == null)
                    return null;
            }

            if (trackerCount > 2)
            {
                // Vertical
                // Determine which two of the trackers are on the highest position (hands)
                // Prefill temp data
                for (int i = 0; i < 4; i++)
                    tempVal[i] = trackerList[i].transform.localPosition.y;

                // Swap for the highest value
                // Moving the two highest trackers to the first two indicies of the map
                swapHighest(0);
                swapHighest(1);
            }

            // Horizontal
            // Determine which two of the trackers are right and which are left
            for (int i = 0; i < trackerCount; i += 2)
            {
                float first = trackerList[tempMap[i]].transform.localPosition.x;
                float second = trackerList[tempMap[i + 1]].transform.localPosition.x;

                if (first < second)
                    swap(i, i + 1);
            }

            // Expose map
            return tempMap;
        }

        /// <summary>
        /// Returns a value indicating if the specified tracker has valid tracking data.
        /// </summary>
        /// <param name="tracker">The tracker to check.</param>
        /// <returns><c>true</c> if the tracker returns valid data; otherwise <c>false</c>.</returns>
        public bool HasValidData(Tracker tracker)
        {
            if (trackerMap == null)
                return false;

            int index = trackerMap[(int)tracker];

            if (trackerList[index] != null)
                return vive.HasValidData(trackerList[index]);

            return true;
        }

        /// <summary>
        /// Apply a tracker map with the resulting probability percentage of each trackers.
        /// Checks how likely it is that a specified tracker is on the mapped position.
        /// </summary>
        /// <param name="map">The map to apply.</param>
        protected void ApplyMapPercental(int[] map)
        {
            if (map == null)
                return;

            // [0] index of right hand
            // [1] index of left hand
            // [2] index of right ankle
            // [3] index of left ankle

            if (positionProbability == null)
                positionProbability = new Vector4[4];

            // 'positionProbability' uses the unmapped indicies for determining where the tracker might go
            // x: right hand
            // y: left hand
            // z: right ankle
            // w: left ankle

            float multi = Time.deltaTime / percentalMappingTime;

            // Calculate the probability value of the map
            for (int i = 0; i < 4; i++)
            {
                int index = Array.IndexOf<int>(map, i);
                Vector4 current = positionProbability[i];

                positionProbability[i] = new Vector4(
                    index == 0 ? Mathf.Clamp01(current.x + multi) : Mathf.Clamp01(current.x - multi),
                    index == 1 ? Mathf.Clamp01(current.y + multi) : Mathf.Clamp01(current.y - multi),
                    index == 2 ? Mathf.Clamp01(current.z + multi) : Mathf.Clamp01(current.z - multi),
                    index == 3 ? Mathf.Clamp01(current.w + multi) : Mathf.Clamp01(current.w - multi)
                );
            }

            // Manipulate map by considering the probability of each tracker
            for (int i = 0; i < 4; i++)
            {
                float maxVal = positionProbability[i].x;
                int maxIndex = 0;

                // Search the maximum percentage of vector map
                for (int j = 1; j < 4; j++)
                {
                    switch (j)
                    {
                        case 1:
                            if (positionProbability[i].y >= maxVal)
                            {
                                maxVal = positionProbability[i].y;
                                maxIndex = 1;
                            }
                            break;

                        case 2:
                            if (positionProbability[i].z >= maxVal)
                            {
                                maxVal = positionProbability[i].z;
                                maxIndex = 2;
                            }
                            break;

                        default:
                            if (positionProbability[i].w >= maxVal)
                            {
                                maxVal = positionProbability[i].w;
                                maxIndex = 3;
                            }
                            break;
                    }
                }

                // 'maxIndex' has now the index that describes the tracker location
                map[maxIndex] = i;
            }

            // Apply percental map
            ApplyMap(map);
        }

        #endregion

        #region Continuous Tracker Validation

        /// <summary>
        /// Checks the assigned trackers for valid data.
        /// </summary>
        /// <returns><c>true</c> if they all provide valid data; otherwise <c>false</c>.</returns>
        protected bool ValidationRoutine()
        {
            if (activeMapping)
                return true;

            bool valid = true;
            int count = trackerCount >= 4 ? 4 : (trackerCount < 2 ? 0 : 2);

            // Check validity of tracker data
            for (int i = 0; i < count; i++)
            {
                if (!HasValidData((Tracker)i))
                {
                    Debug.LogWarning($"[PoseTracker] Tracking | SteamVR API is sending error signal for tracker '{(Tracker)i}'. Position data is not guaranteed to be valid!");
                    valid = false;
                }
            }

            return valid;
        }

        #endregion

        #region Pose Detection

        /// <summary>
        /// Set the height of the local position correction according to the position of the ankle
        /// trackers. This reduces tracking/calibration errors for the floor.
        /// </summary>
        public void FloorHeightCorrection()
        {
            Vector3 tempLocalCorrection = localPositionCorrection.Value;

            if (trackerCount < 4)
            {
                tempLocalCorrection.y = 0f;
                localPositionCorrection.Value = tempLocalCorrection;
            }
            else
            {
                float averageHeight = (trackers[Tracker.RightAnkle].Position.y + trackers[Tracker.LeftAnkle].Position.y) / 2f;

                tempLocalCorrection.y = ankleGroundHeight - averageHeight;
                localPositionCorrection.Value = tempLocalCorrection;
            }
        }

        /// <summary>
        /// Checks if the pose is matching the trackers position.
        /// </summary>
        /// <param name="pose">The pose to check.</param>
        /// <param name="useSecondary">Use the secondary pose.</param>
        /// <returns><c>true</c> if pose matches; otherwise <c>false</c>.</returns>
        public bool IsPoseMatching(Pose pose, bool useSecondary = false)
        {
            Pose p = useSecondary ? pose?.Secondary : pose;

            if (p == null)
                return true;

            if (trackerCount == 0)
                return false;

            float size = Mathf.Clamp(playerSizeFactor, 1.5f, 2.3f);
            float posMulti = p.IgnorePlayerSizeFactor ? 1f : (size - 1.5f) * 0.25f + 0.8f;

            Bounds rightHandArea = p.HandRight;
            Bounds leftHandArea = p.HandLeft;
            Bounds rightAnkleArea = p.AnkleRight;
            Bounds leftAnkleArea = p.AnkleLeft;
            float handDist = p.HandDistance;
            float ankleDist = p.AnkleDistance;
            float handAnkleDist = p.HandAnkleDistance;

            // Adapt positions according to players size and set tolerance
            rightHandArea.center = new Vector3(rightHandArea.center.x * posMulti, rightHandArea.center.y * posMulti, rightHandArea.center.z);
            rightHandArea.extents = new Vector3(rightHandArea.extents.x, rightHandArea.extents.y * posMulti, rightHandArea.extents.z);
            rightHandArea.Expand(extraToleranceHands);

            leftHandArea.center = new Vector3(leftHandArea.center.x * posMulti, leftHandArea.center.y * posMulti, leftHandArea.center.z);
            leftHandArea.extents = new Vector3(leftHandArea.extents.x, leftHandArea.extents.y * posMulti, leftHandArea.extents.z);
            leftHandArea.Expand(extraToleranceHands);

            rightAnkleArea.center = new Vector3(rightAnkleArea.center.x * posMulti, rightAnkleArea.center.y * posMulti, rightAnkleArea.center.z);
            rightAnkleArea.extents = new Vector3(rightAnkleArea.extents.x, rightAnkleArea.extents.y * posMulti, rightAnkleArea.extents.z);
            rightAnkleArea.Expand(extraToleranceAnkles);

            leftAnkleArea.center = new Vector3(leftAnkleArea.center.x * posMulti, leftAnkleArea.center.y * posMulti, leftAnkleArea.center.z);
            leftAnkleArea.extents = new Vector3(leftAnkleArea.extents.x, leftAnkleArea.extents.y * posMulti, leftAnkleArea.extents.z);
            leftAnkleArea.Expand(extraToleranceAnkles);

            // Check position for all hands and ankles and return true if all are in tolerance area
            bool validHandPosition = rightHandArea.Contains(trackers[Tracker.RightWrist].Position) && leftHandArea.Contains(trackers[Tracker.LeftWrist].Position);
            bool validAnklePosition = (trackerCount == 2) || (rightAnkleArea.Contains(trackers[Tracker.RightAnkle].Position) && leftAnkleArea.Contains(trackers[Tracker.LeftAnkle].Position));

            bool validHandDist = Vector3.Distance(rightHandArea.center, leftHandArea.center) >= handDist;
            bool validAnkleDist = (trackerCount == 2) || Vector3.Distance(rightAnkleArea.center, leftAnkleArea.center) >= ankleDist;
            bool validHandAnkleDist = (trackerCount == 2) || Vector3.Distance(
                (rightHandArea.center + leftHandArea.center) * 0.5f,
                (rightAnkleArea.center + leftAnkleArea.center) * 0.5f
            ) >= handAnkleDist;

            return validHandPosition && validAnklePosition && validHandDist && validAnkleDist && validHandAnkleDist;
        }

        /// <summary>
        /// Returns the oscillation data for a specified pose.
        /// Return tuple: (x, y);
        /// 1  = Right is higher than left;
        /// 0  = Left is higher than right;
        /// -1 = Too little difference (invalid data);
        /// </summary>
        /// <param name="pose">The pose getting the oscillation data from.</param>
        /// <param name="useSecondary">Use the secondary pose.</param>
        /// <returns>A tuple defying the oscillation.</returns>
        public ValueTuple<int, int, int, int> GetOscillationData(Pose pose, bool useSecondary = false)
        {
            Pose p = useSecondary ? pose?.Secondary : pose;
            ValueTuple<int, int, int, int> result = new ValueTuple<int, int, int, int>(-1, -1, -1, -1);

            if (p?.IsHeldPose == true)
            {
                ValueTuple<int, int> tmp;

                tmp = GetOscillationData(trackers[Tracker.RightWrist].Position, trackers[Tracker.LeftWrist].Position, p.Oscillate);
                result.Item1 = tmp.Item1;
                result.Item2 = tmp.Item2;

                if (trackerCount == 4)
                {
                    tmp = GetOscillationData(trackers[Tracker.RightAnkle].Position, trackers[Tracker.LeftAnkle].Position, p.OscillateAnkles);
                    result.Item3 = tmp.Item1;
                    result.Item4 = tmp.Item2;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the oscillation data in between two positions.
        /// 1  = Right is higher than left;
        /// 0  = Left is higher than right;
        /// -1 = Too little difference (invalid data);
        /// </summary>
        /// <param name="pos1">Position 1</param>
        /// <param name="pos2">Position 2</param>
        /// <param name="minDiff">Minimum oscillation offset</param>
        /// <returns>A tuple defining the oscillation.</returns>
        private ValueTuple<int, int> GetOscillationData(Vector3 pos1, Vector3 pos2, Vector2 minDiff)
        {
            int x = -1;
            int y = -1;

            Vector2 diff = pos1 - pos2;

            if (!Mathf.Approximately(minDiff.x, 0f))
            {
                if (diff.x >= minDiff.x)
                    x = 1;
                else if (-diff.x >= minDiff.x)
                    x = 0;
            }

            if (!Mathf.Approximately(minDiff.y, 0f))
            {
                if (diff.y >= minDiff.y)
                    y = 1;
                else if (-diff.y >= minDiff.y)
                    y = 0;
            }

            return new ValueTuple<int, int>(x, y);
        }

        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Reset validation routine to initial state
            validationActive.Reset();
            validateInterval.Reset();
            invalidAfterCount.Reset();

            vive = GetComponent<IViveControllers>();

            if (vive == null)
                Debug.LogError("[PoseTracker] PoseTracker component needs to have an IViveController object data with at least 4 tracker objects assigned!");

            // Ensure all tracker data is available
            trackers.Clear();
            foreach (Tracker tracker in Enum.GetValues(typeof(Tracker)))
                trackers.Add(tracker, new TrackerData());
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (activeMapping)
                ApplyMapPercental(MapTrackers());

            // Validation
            if (validationActive.Value)
            {
                // Validation interval
                if (validationIntervalTime >= validateInterval)
                {
                    // Trackers are invalid
                    if (!ValidationRoutine())
                    {
                        validationCount++;
                    }
                    // Trackers became valid again
                    else if (!previousValid)
                    {
                        previousValid = true;
                        validationCount = 0;
                        onBecomeValid.Invoke();
                        Debug.Log("[PoseTracker] Tracking | Reading valid data from SteamVR API. Reset error counter...");
                    }

                    validationIntervalTime = 0f;

                    // If invalid for the specified amount of time
                    if (validationCount >= invalidAfterCount)
                    {
                        validationCount = 0;

                        // Fire event
                        if (previousValid)
                        {
                            onBecomeInvalid.Invoke();
                            previousValid = false;
                        }
                    }
                }
                else
                {
                    validationIntervalTime += Time.deltaTime;
                }
            }

            // Update data
            foreach (TrackerData tracker in trackers.Values)
                tracker.Update(localPositionCorrection.Value);
        }
    }
}
