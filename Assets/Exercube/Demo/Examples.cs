using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Koboldgames.Primitives.Events;

namespace Sphery.ExerCube
{
    public class Examples : MonoBehaviour
    {
        [Header("Activity Console Text")]
        [SerializeField] protected TMP_Text console;

        [Header("Event Listeners")]
        [SerializeField] protected SharedEvent onPlayerCalibrated;
        [SerializeField] protected SharedEvent onTrackerBecomeInvalid;
        [SerializeField] protected SharedEvent onTrackerBecomeValid;

        [Header("Example Buttons")]
        [SerializeField] protected Button searchMapTrackers;
        [SerializeField] protected Button calibratePlayer;
        [SerializeField] protected Button validationRoutine;

        [Header("Tracker Indicators")]
        [SerializeField] protected Image rightHandIndicator;
        [SerializeField] protected Image leftHandIndicator;
        [SerializeField] protected Image rightAnkleIndicator;
        [SerializeField] protected Image leftAnkleIndicator;

        public static ActivityConsole Console { get; private set; }

        protected GameObject player;
        protected ITrackingManager poseTracker;
        protected IPlayerCalibration playerCalibration;

        private UnityAction searchMapTrackersDelegate;
        private UnityAction validationRoutineDelegate;

        #region Event Listeners (Logging)

        /// <summary>
        /// When tracker search started.
        /// </summary>
        private void OnTrackerSearchStarted() => Console.Log("[ViveControllerAssign] Start searching for Vive trackers...");

        /// <summary>
        /// When tracker search stopped.
        /// </summary>
        private void OnTrackerSearchStopped() => Console.Log("[ViveControllerAssign] Search for Vive trackers stopped!");

        /// <summary>
        /// When trackers have been indexed.
        /// </summary>
        private void OnTrackersIndexed()
        {
            int count = player.GetComponentInChildren<ViveControllerAssign>().IndexedTrackers;
            Console.Log($"[ViveControllerAssign] Indexed {count} trackers!", Color.green);
            Console.Log("[ViveControllerAssign] Search keeps running...");
        }

        /// <summary>
        /// When player has been calibrated.
        /// </summary>
        private void OnPlayerCalibrated() => Console.Log("[PlayerCalibration] Player calibrated!");

        /// <summary>
        /// When a Vive tracker becomes invalid.
        /// Detected through validation routine of the <see cref="PoseTracker"/>.
        /// </summary>
        private void OnTrackerBecomeInvalid()
        {
            Console.Log("[PoseTracker] One or more trackers send invalid data!", Color.red);
            Console.Log("              Check connection!");

            Console.Log("<i>> After having multiple reconnection attempts, the", Color.grey);
            Console.Log("> 'OnTrackerBecomeInvalid' event will be fired. The", Color.grey);
            Console.Log("> amount of retries and their delay can be", Color.grey);
            Console.Log("> specified on the players 'PoseTracker' component.", Color.grey);
            Console.Log("> 'Validate Interval' and 'Invalid After Count'</i>", Color.grey);
        }

        /// <summary>
        /// When a Vive tracker becomes valid after being detected as invalid.
        /// Detected through validation routine of the <see cref="PoseTracker"/>.
        /// </summary>
        private void OnTrackerBecomeValid()
        {
            Console.Log("[PoseTracker] Receiving valid tracker data again!", Color.green);

            Console.Log("<i>> Meaning: a tracker that lost connection or sent", Color.grey);
            Console.Log("> invalid data, is now visible again in the SteamVR", Color.grey);
            Console.Log("> API. Tracking can continue without adjusting the", Color.grey);
            Console.Log("> tracker mapping or calibration data.</i>", Color.grey);
        }

        #endregion

        #region Button Logic

        /// <summary>
        /// Toggle tracker search...
        /// </summary>
        /// <param name="buttonText">Button text object.</param>
        public void ToggleTrackerSearchAndMapping(TMP_Text buttonText)
        {
            if (poseTracker.SearchAndMappingActive)
            {
                poseTracker.StopActiveMapping();
                Console.Log("[PoseTracker] Lock tracker map!");
            }
            else
            {
                poseTracker.StartActiveMapping();
                Console.Log("[PoseTracker] Continually mapping trackers...");

                Console.Log("<i>> The players 'PoseTracker' now actuates its", Color.grey);
                Console.Log("> underlying 'ViveControllerAssign' component to", Color.grey);
                Console.Log("> actively search and index trackers provided by the", Color.grey);
                Console.Log("> SteamVR API. All the found trackers will be", Color.grey);
                Console.Log("> continuously mapped (positional assignment, e.g.", Color.grey);
                Console.Log("> right/left hand, right/left ankle). The process", Color.grey);
                Console.Log("> can be stopped at any time. Always stop this", Color.grey);
                Console.Log("> search <u>before</u> calibrating the player!</i>", Color.grey);

            }

            buttonText.text = $"{(poseTracker.SearchAndMappingActive ? "Stop" : "Start")} Tracker Search And Mapping";
        }

        /// <summary>
        /// Calibrate player...
        /// Make sure the player stands in the center of the cube, hands in the air.
        /// </summary>
        public void CalibratePlayer()
        {
            if (poseTracker.SearchAndMappingActive)
            {
                Console.Log("<i>> Ok, I'll stop the search for you ;)</i>", Color.grey);
                searchMapTrackersDelegate();
            }

            playerCalibration.Calibrate();
        }

        /// <summary>
        /// Toggle tracker validation...
        /// </summary>
        /// <param name="buttonText">Button text object.</param>
        public void ToggleTrackerValidation(TMP_Text buttonText)
        {
            if (!poseTracker.ValidationRunning)
            {
                poseTracker.ValidationRunning = true;
                buttonText.text = "Stop Tracker Validation";

                Console.Log("[PoseTracker] Started tracker validation routine...");

                if (poseTracker.SearchAndMappingActive)
                {
                    Console.Log("[PoseTracker] Tracker search and mapping still running!", Color.yellow);
                    Console.Log("              Validation routine temporarily disabled.");

                    Console.Log("<i>> It will automatically continue as soon as the", Color.grey);
                    Console.Log("> search is stopped.</i>", Color.grey);
                }

                Console.Log("<i>> You can keep this routine running as long as need", Color.grey);
                Console.Log("> to read tracking data to ensure validity of the", Color.grey);
                Console.Log("> received data. Stop it before recalibrating a", Color.grey);
                Console.Log("> (new) player.</i>", Color.grey);
            }
            else
            {
                poseTracker.ValidationRunning = false;
                buttonText.text = "Start Tracker Validation";
                Console.Log("[PoseTracker] Tracker validation routine stopped!");
            }
        }

        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Assign activity console
            Console = new ActivityConsole(console);
            Console.Log("Activity Console initialized...");
            Console.Log($"- Instance: {GetInstanceID()}");
            Console.Log($"- T: {Time.timeSinceLevelLoad:F2}s");
            Console.Log("===================================");

            // Give me player
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                DisableWithError("[Examples] No player object found in scene!");
                return;
            }

            // Give me components
            poseTracker = player.GetComponentInChildren<ITrackingManager>();
            playerCalibration = player.GetComponentInChildren<IPlayerCalibration>();

            // Security checks
            if (poseTracker == null)
            {
                DisableWithError("[Examples] No 'pose tracker' component found on player object!");
                return;
            }
            if (playerCalibration == null)
            {
                DisableWithError("[Examples] No 'play calibration' component found on player object!");
                return;
            }

            // Setup specific button delegates
            searchMapTrackersDelegate = () => ToggleTrackerSearchAndMapping(searchMapTrackers.GetComponentInChildren<TMP_Text>());
            validationRoutineDelegate = () => ToggleTrackerValidation(validationRoutine.GetComponentInChildren<TMP_Text>());
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            // Add event listeners
            ViveControllerAssign vca = player.GetComponentInChildren<ViveControllerAssign>();
            vca.OnSearchStarted += OnTrackerSearchStarted;
            vca.OnSearchStopped += OnTrackerSearchStopped;
            vca.OnTrackerIndexed += OnTrackersIndexed;
            onPlayerCalibrated.AddListener(OnPlayerCalibrated);
            onTrackerBecomeInvalid.AddListener(OnTrackerBecomeInvalid);
            onTrackerBecomeValid.AddListener(OnTrackerBecomeValid);

            // Enable example buttons
            searchMapTrackers.onClick.AddListener(searchMapTrackersDelegate);
            calibratePlayer.onClick.AddListener(CalibratePlayer);
            validationRoutine.onClick.AddListener(validationRoutineDelegate);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Remove event listeners
            if (player != null)
            {
                ViveControllerAssign vca = player.GetComponentInChildren<ViveControllerAssign>();
                vca.OnSearchStarted -= OnTrackerSearchStarted;
                vca.OnSearchStopped -= OnTrackerSearchStopped;
                vca.OnTrackerIndexed -= OnTrackersIndexed;
            }

            onPlayerCalibrated.RemoveListener(OnPlayerCalibrated);
            onTrackerBecomeInvalid.RemoveListener(OnTrackerBecomeInvalid);
            onTrackerBecomeValid.RemoveListener(OnTrackerBecomeValid);

            // Disable example buttons
            searchMapTrackers.onClick.RemoveListener(searchMapTrackersDelegate);
            calibratePlayer.onClick.RemoveListener(CalibratePlayer);
            validationRoutine.onClick.RemoveListener(validationRoutineDelegate);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // Track indicator logic:
            // This can be optimized greatly obviously. Not ideal operating on image data every frame...
            // It's fine for demonstration purposes though

            // Only if tracking enabled/running
            if (!poseTracker.TrackingEnabled)
            {
                rightHandIndicator.color = Color.grey;
                leftHandIndicator.color = Color.grey;
                rightAnkleIndicator.color = Color.grey;
                leftAnkleIndicator.color = Color.grey;
                return;
            }

            rightHandIndicator.color = poseTracker.HasValidData(Tracker.RightWrist) ? Color.green : Color.red;
            leftHandIndicator.color = poseTracker.HasValidData(Tracker.LeftWrist) ? Color.green : Color.red;

            if (poseTracker.TrackerCount > 2)
            {
                rightAnkleIndicator.color = poseTracker.HasValidData(Tracker.RightAnkle) ? Color.green : Color.red;
                leftAnkleIndicator.color = poseTracker.HasValidData(Tracker.LeftAnkle) ? Color.green : Color.red;
            }
            else
            {
                rightAnkleIndicator.color = Color.grey;
                leftAnkleIndicator.color = Color.grey;
            }
        }

        /// <summary>
        /// Log an error to console and disable this gameobject.
        /// </summary>
        /// <param name="error">The error message.</param>
        private void DisableWithError(string error)
        {
            Console.Log(error, Color.red);
            Debug.LogError(error);
            enabled = false;
        }
    }
}
