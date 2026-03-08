using UnityEngine;
using UnityEngine.Events;
using Koboldgames.Primitives.Variables;
using Sphery.ExerCube.Helpers;

namespace Sphery.ExerCube
{
    public class PlayerCalibration : MonoBehaviour, IPlayerCalibration
    {
        [SerializeField] protected FlexibleFloat playerSizeFactor;
        [SerializeField] protected FlexibleFloat playerZPositionNormalized = new FlexibleFloat(0.5f);
        [SerializeField] protected FlexibleVector3 localPositionCorrection;
        [SerializeField, MinMaxRange(0f, 1.5f)] protected MinMaxRange zCorrectionRange = new MinMaxRange(0.15f, 1f);
        [SerializeField] protected UnityEvent onCalibrated;

        [Header("Input")]
        [SerializeField] protected KeyCode activateKeyCode = KeyCode.C;

        private ITrackingManager trackingManager;

        /// <summary>
        /// Gets the calibrated size factor of the player.
        /// </summary>
        /// <value>The size of the player.</value>
        public float PlayerSizeFactor => playerSizeFactor.Value;

        /// <summary>
        /// Gets the calibrated player position in the Z axis. Defines where on the Z axis the
        /// player should be standing. This value is normalized [0...1], where 0 is as close to the
        /// front wall as permitted ('zCorrectionRange.Min'), and 1 a far from the front wall away
        /// ('zCorrectionRange.Max').
        /// </summary>
        /// <value>The calibrated Z position of the player.</value>
        public float PlayerZPositionNormalized => playerZPositionNormalized.Value;

        /// <summary>
        /// Gets the resulting local position correction of the calibration.
        /// </summary>
        /// <value>The resulting local position correction.</value>
        public Vector3 LocalPositionCorrection => localPositionCorrection.Value;

        /// <summary>
        /// Manually calibrate from the current tracker height.
        /// </summary>
        public void Calibrate()
        {
            trackingManager.StopActiveMapping();
            trackingManager.FloorHeightCorrection();

            float heightR = trackingManager[Tracker.RightWrist].Position.y;
            float heightL = trackingManager[Tracker.LeftWrist].Position.y;

            // Average height between both hands
            playerSizeFactor.Value = heightR + (0.5f * (heightL - heightR));
            DefinePlayerZPosition();

            // Fire on-calibrated event
            onCalibrated?.Invoke();
        }

        /// <summary>
        /// Set player size factor to its initial value and skip calibration.
        /// </summary>
        public void SkipCalibration()
        {
            trackingManager.StopActiveMapping();
            playerSizeFactor.Value = 2f;
            DefinePlayerZPosition();

            // Fire on-calibrated event
            onCalibrated?.Invoke();
        }

        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's
        /// context menu or when adding the component the first time.
        /// </summary>
        public void Reset()
        {
            // Reset variables
            playerSizeFactor?.Reset();
            playerZPositionNormalized?.Reset();
            localPositionCorrection?.Reset();
        }

        /// <summary>
        /// Calculate Z position offset of the player.
        /// </summary>
        protected void DefinePlayerZPosition()
        {
            // These numbers resulted from a very convincing theory about arm-to-bodysize correlation
            // provided by Yasse. Tweak this if needed, there's no calcualtion behind this.
            float size = Mathf.Clamp(playerSizeFactor, 1.5f, 2.3f);
            float t = (size - 1.5f) / 0.8f;
            float z = Mathf.Lerp(zCorrectionRange.Min, zCorrectionRange.Max, t);
            Vector3 correction = localPositionCorrection.Value;

            correction = new Vector3(correction.x, correction.y, z);
            playerZPositionNormalized.Value = t;
            localPositionCorrection.Value = correction;
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Search player with pose tracker component
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj != null)
            {
                trackingManager = playerObj.GetComponentInChildren<ITrackingManager>();

                if (trackingManager == null)
                    Debug.LogError("[PlayerCalibration] No player in scene found with an assigned pose tracker!");
            }
            else
            {
                Debug.LogError("[PlayerCalibration] No player in scene found!");
            }

            // Reset variables
            Reset();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(activateKeyCode))
                Calibrate();
        }
    }
}
