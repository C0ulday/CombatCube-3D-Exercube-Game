using UnityEngine;
using UnityEngine.UI;
using Koboldgames.Primitives.Events;


/// hello can anyone see this?
/// Hendrik's git is working ?
/// divyam's too
/// Moritz(goat)

namespace Sphery.ExerCube
{
    public class TrackingExamples : MonoBehaviour
    {
        [SerializeField] protected SharedEvent onCalibrate;

        [SerializeField] protected GameObject rightHand;
        [SerializeField] protected GameObject leftHand;
        [SerializeField] protected GameObject rightAnkle;
        [SerializeField] protected GameObject leftAnkle;

        protected GameObject player;
        protected ITrackingManager trackingManager;

        #region Example Methods

        // Toggle tracker validation...
        public void ToggleTrackerValidation(Text buttonText)
        {
            trackingManager.ValidationRunning ^= true;
            buttonText.text = $"{(trackingManager.ValidationRunning ? "Stop" : "Start")} Tracker Validation";
        }

        // Toggle tracker search...
        public void ToggleTrackerSearchAndMapping(Text buttonText)
        {
            if (trackingManager.SearchAndMappingActive)
                StopTrackerSearchAndMapping();
            else
                StartTrackerSearchAndMapping();

            buttonText.text = $"{(trackingManager.SearchAndMappingActive ? "Stop" : "Start")} Tracker Search And Mapping";
        }

        // Start tracker validation...
        public void StartTrackerValidation()
        {
            trackingManager.ValidationRunning = true;
            Debug.Log("Tracker validation started...");
        }

        // Stop tracker validation...
        public void StopTrackerValidation()
        {
            trackingManager.ValidationRunning = false;
            Debug.Log("Tracker validation stopped...");
        }

        // Start tracker seacrh...
        public void StartTrackerSearchAndMapping()
        {
            trackingManager.StartActiveMapping();
            Debug.Log("Tracker search and active mapping started...");
        }

        // Stop tracker seacrh...
        public void StopTrackerSearchAndMapping()
        {
            trackingManager.StopActiveMapping();
            Debug.Log("Tracker search and active mapping stopped...");
            Debug.Log($"Found {trackingManager.TrackerCount} tracker(s)!");
        }

        // Calibrate player...
        // Make sure the player stands in the center of the cube, hands in the air
        public void CalibratePlayer()
        {
            onCalibrate.Invoke();
            Debug.Log("Player calibrated!");
            Debug.Log("Tracker search and active mapping stopped...");
            Debug.Log($"Found {trackingManager.TrackerCount} tracker(s)!");
        }

        // Chack tracker validity...
        // Disables specified gameobjects if not valid
        public void CheckTrackers()
        {
            rightHand.SetActive(trackingManager.HasValidData(Tracker.RightWrist));
            leftHand.SetActive(trackingManager.HasValidData(Tracker.LeftWrist));
            rightAnkle.SetActive(trackingManager.HasValidData(Tracker.RightAnkle));
            leftAnkle.SetActive(trackingManager.HasValidData(Tracker.LeftAnkle));
        }

        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Give me player
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                DisableWithError("[Examples] No player object found in scene!");
                return;
            }

            // Give me components
            trackingManager = player.GetComponentInChildren<ITrackingManager>();

            // Security checks
            if (trackingManager == null)
            {
                DisableWithError("[Examples] No 'pose-tracker' component found on player object!");
                return;
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // Press [1] to start tracker search and mapping
            // Press [Shift+1] to stop tracker search and mapping
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
                    StopTrackerSearchAndMapping();
                else
                    StartTrackerSearchAndMapping();
            }

            // Press [2] to let calibrate player
            if (Input.GetKeyDown(KeyCode.Alpha2))
                CalibratePlayer();

            // Press [3] to start tracker validation
            // Press [Shift+3] to stop tracker validation
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
                    StopTrackerValidation();
                else
                    StartTrackerValidation();
            }
        }

        /// <summary>
        /// Log an error to console and disable this gameobject.
        /// </summary>
        /// <param name="error">The error message.</param>
        private void DisableWithError(string error)
        {
            Debug.LogError(error);
            gameObject.SetActive(false);
        }
    }
}
