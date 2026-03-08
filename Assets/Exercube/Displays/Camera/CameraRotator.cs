using UnityEngine;

namespace Sphery.ExerCube
{
    public class CameraRotator : MonoBehaviour, ICameraRotator
    {
        [SerializeField] protected Camera frontCamera;
        [SerializeField] protected Camera rightCamera;
        [SerializeField] protected Camera leftCamera;

        protected float horizontalFrontFOV;
        protected float horizontalRightFOV;
        protected float horizontalLeftFOV;

        /// <summary>
        /// Gets the camera assigned for the front screen.
        /// </summary>
        /// <value>The front facing camera.</value>
        public Camera Front => frontCamera;

        /// <summary>
        /// Gets the camera assigned for the right screen.
        /// </summary>
        /// <value>The camera facing to the right.</value>
        public Camera Right => rightCamera;

        /// <summary>
        /// Gets the camera assigned for the left screen.
        /// </summary>
        /// <value>The camera facing to the left.</value>
        public Camera Left => leftCamera;

        /// <summary>
        /// Gets a hash representating this instance.
        /// </summary>
        /// <value>The hash code for this instance.</value>
        public int Hash => GetHashCode();

        /// <summary>
        /// Occurs when the field-of-view calculation has been completed.
        /// </summary>
        public event System.Action OnRecalculatedFOV;

        /// <summary>
        /// Calculate and rotate the cameras for a pixel perfect transition.
        /// </summary>
        [ContextMenu("Recalculate")]
        public void Rotate()
        {
            // Reset rotation
            frontCamera.transform.localEulerAngles = Vector3.zero;

            RecalculateHorizontalFOV();

            // Rotate right
            rightCamera.transform.localEulerAngles = new Vector3(
                0f,
                (horizontalFrontFOV / 2f) + (horizontalRightFOV / 2f),
                0f
            );

            // Rotate left
            leftCamera.transform.localEulerAngles = new Vector3(
                0f,
                -(horizontalFrontFOV / 2f) - (horizontalLeftFOV / 2f),
                0f
            );
        }

        /// <summary>
        /// Recalculate the horizontal field of view for each camera.
        /// </summary>
        protected void RecalculateHorizontalFOV()
        {
            float f = Mathf.Tan(Mathf.Deg2Rad * (frontCamera.fieldOfView / 2.0f));
            horizontalFrontFOV = Mathf.Rad2Deg * Mathf.Atan(f * ((float)frontCamera.pixelWidth / (float)frontCamera.pixelHeight)) * 2.0f;

            f = Mathf.Tan(Mathf.Deg2Rad * (rightCamera.fieldOfView / 2.0f));
            horizontalRightFOV = Mathf.Rad2Deg * Mathf.Atan(f * ((float)rightCamera.pixelWidth / (float)rightCamera.pixelHeight)) * 2.0f;

            f = Mathf.Tan(Mathf.Deg2Rad * (leftCamera.fieldOfView / 2.0f));
            horizontalLeftFOV = Mathf.Rad2Deg * Mathf.Atan(f * ((float)leftCamera.pixelWidth / (float)leftCamera.pixelHeight)) * 2.0f;

            OnRecalculatedFOV?.Invoke();
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (frontCamera == null)
                Debug.LogWarning("[CameraRotator] No front camera assigned!");

            if (rightCamera == null)
                Debug.LogWarning("[CameraRotator] No right camera assigned!");

            if (leftCamera == null)
                Debug.LogWarning("[CameraRotator] No left camera assigned!");
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start() => Rotate();
    }
}
