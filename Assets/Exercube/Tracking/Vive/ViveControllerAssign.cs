using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR;

namespace Sphery.ExerCube
{
    public class ViveControllerAssign : MonoBehaviour, IViveControllers
    {
        [SerializeField] protected SteamVR_TrackedObject[] trackers;
        [SerializeField] protected bool searchOnStart = false;
        [SerializeField] protected float searchTimeInterval = 1f;
        [SerializeField] protected float validationTimeInterval = 1f;
        [SerializeField] protected bool positionValidation = false;
        [SerializeField] protected int positionCacheSize = 10;

        private SearchProcedure search = SearchProcedure.Idle;
        private int validateFound = 0;
        private bool checkControllers = false;
        private float time = System.Single.MaxValue;
        private Vector3[,] positionCache;
        private bool[] validDataMap;
        private List<int> trackerMap = new List<int>(16);

#if UNITY_EDITOR
        private int hardcodedTrackerCount = -1;
#endif

        private enum SearchProcedure
        {
            Idle,
            Search,
            Validate,
            Index
        }

        /// <summary>
        /// Occurs when the tracker search has been started.
        /// </summary>
        public event Action OnSearchStarted;

        /// <summary>
        /// Occurs when the tracker search has been stopped.
        /// </summary>
        public event Action OnSearchStopped;

        /// <summary>
        /// Occurs when the tracker have been indexed and initialization is therefore completed.
        /// </summary>
        public event Action OnTrackerIndexed;

        /// <summary>
        /// Gets a value indicating if currently a search for controllers is running.
        /// </summary>
        /// <value><c>true</c> if the a search is currently running; otherwise <c>false</c>.</value>
        public bool IsSearching => search != SearchProcedure.Idle;

        /// <summary>
        /// Gets the amount of trackers that have been indexed.
        /// </summary>
        /// <value>The amount of indexed trackers.</value>
        public int IndexedTrackers { get; private set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating if a data validation trhough position tracking should be processed.
        /// </summary>
        /// <value><c>true</c> if a position validation should be considered; otherwise <c>false</c>.</value>
        public bool PositionValidation
        {
            get { return positionValidation; }
            set { positionValidation = value; }
        }

        /// <summary>
        /// Gets this gameobject instances hash value.
        /// </summary>
        /// <value>This gameobject instances hash value.</value>
        public int Hash => gameObject.GetHashCode();

        /// <summary>
        /// Starts a controller search process.
        /// </summary>
        public void StartSearch()
        {
            if (search != SearchProcedure.Idle)
                return;

            time = Single.MaxValue;
            validateFound = 0;
            IndexedTrackers = 0;
            validDataMap = new bool[trackers?.Length ?? 0];
            search = SearchProcedure.Search;
            OnSearchStarted?.Invoke();
        }

        /// <summary>
        /// Stops a currently running search.
        /// </summary>
        public void StopSearch()
        {
            search = SearchProcedure.Idle;
            OnSearchStopped?.Invoke();
        }

        /// <summary>
        /// Fills a given array with the trackers that are validated successfully.
        /// </summary>
        /// <param name="list">The prepared array for the list.</param>
        public void GetTrackerList(ref SteamVR_TrackedObject[] list)
        {
            int iMapped = 0;

            for (int i = 0; i < trackers.Length; i++)
            {
                if ((iMapped < list.Length) && trackerMap.Contains(i))
                {
                    list[iMapped] = trackers[i];
                    iMapped++;
                }
            }
        }

        /// <summary>
        /// Check if a specified tracker object has valid data.
        /// </summary>
        /// <param name="tracker">The reference to the tracker object.</param>
        /// <returns><c>true</c> if this tracker has valid data; otherwise <c>false</c>.</returns>
        public bool HasValidData(SteamVR_TrackedObject tracker) => validDataMap[Array.IndexOf(trackers, tracker)];

        /// <summary>
        /// Search for Vive trackers by iterating through the steam VR device identifiers.
        /// </summary>
        /// <param name="trackerIndicies">The array od tracker indicies.</param>
        /// <returns>The amount of trackers that have been indexed.</returns>
        protected int SearchTrackers()
        {
            bool isTracker;
            int count = 0;
            ETrackedPropertyError error = ETrackedPropertyError.TrackedProp_Success;

            time = 0f;

            for (int i = 0; i < 16; i++)
            {
                StringBuilder result = new StringBuilder(64);

                // Gather information through device property model name string
                OpenVR.System?.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
                isTracker = result.ToString().Contains("tracker");

                if (checkControllers)
                    isTracker |= result.ToString().Contains("controller");

                if ((count < trackers.Length) && isTracker)
                {
                    trackers[count].index = (SteamVR_TrackedObject.EIndex)i;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Validating the trackers.
        /// </summary>
        protected void Upkeep()
        {
#if UNITY_EDITOR
            if (hardcodedTrackerCount >= 0)
            {
                for (int i = 0; i < trackers.Length; i++)
                    validDataMap[i] = i < hardcodedTrackerCount;

                return;
            }
#endif

            // Control the validity of the tracker data
            for (int i = 0; i < trackers.Length; i++)
            {
                validDataMap[i] = trackers[i].isValid;

                // Check movement
                if (validDataMap[i] && positionValidation)
                {
                    Vector3 currentPos = trackers[i].transform.localPosition;
                    bool valid = false;

                    // Iterate through position cache (in reverse)
                    for (int j = positionCacheSize - 1; j >= 0; j--)
                    {
                        // Check for change opposed to the values in cache
                        if (!valid && (currentPos != positionCache[i, j]))
                            valid = true;

                        // Shift queue
                        positionCache[i, j] = j == 0 ? currentPos : positionCache[i, j - 1];
                    }

                    validDataMap[i] = valid;
                }
            }
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Map that indicates valid data -> relative to the trackers
            validDataMap = new bool[trackers.Length];

            // Create position cache
            positionCache = new Vector3[trackers.Length, positionCacheSize];
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            if (searchOnStart)
                StartSearch();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // Switch through search procedure
            switch (search)
            {
                // Searching trackers
                case SearchProcedure.Search:

                    // Start and check search
                    if (time >= searchTimeInterval)
                    {
                        // Search paired Vive trackers
#if UNITY_EDITOR
                        validateFound = hardcodedTrackerCount >= 0 ? hardcodedTrackerCount : SearchTrackers();
#else
                    validateFound = SearchTrackers();
#endif

                        // Move to validation step
                        if (validateFound > 0)
                            search = SearchProcedure.Validate;

                        time = 0f;
                    }
                    else
                    {
                        time += Time.deltaTime;
                    }

                    break;

                // Validating trackers
                case SearchProcedure.Validate:

                    // Wait one validation time interval
                    if (time >= validationTimeInterval)
                    {
                        Upkeep();
                        trackerMap.Clear();

                        for (int i = 0; i < trackers.Length; i++)
                        {
                            if (validDataMap[i])
                                trackerMap.Add(i);
                        }

                        // Found valid or invalid trackers?
                        if (trackerMap.Count == 0)
                            search = SearchProcedure.Search;
                        else
                            search = SearchProcedure.Index;

                        time = 0f;
                    }
                    else
                    {
                        time += Time.deltaTime;
                    }

                    break;

                // Indexing trackers
                case SearchProcedure.Index:

                    int count = 0;

                    // Count valid trackers
                    for (int i = 0; i < trackers.Length; i++)
                    {
                        if (trackerMap.Contains(i))
                            count++;
                    }

#if UNITY_EDITOR
                    IndexedTrackers = hardcodedTrackerCount >= 0 ? hardcodedTrackerCount : count;
#else
                IndexedTrackers = count;
#endif
                    OnTrackerIndexed?.Invoke();

                    // Back to search
                    search = SearchProcedure.Search;

                    break;

                // Idle / upkeep
                default:

                    Upkeep();
                    break;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Override 2 Trackers")]
        private void Override2Trackers() => hardcodedTrackerCount = 2;

        [ContextMenu("Override 4 Trackers")]
        private void Override4Trackers() => hardcodedTrackerCount = 4;

        [ContextMenu("Clear Overrides")]
        private void ClearOverrides() => hardcodedTrackerCount = -1;
#endif
    }
}
