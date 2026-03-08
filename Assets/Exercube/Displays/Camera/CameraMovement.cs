using UnityEngine;
using Koboldgames.Primitives.Variables;

namespace Sphery.ExerCube
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] protected FlexibleBoolean active = new FlexibleBoolean(true);
        [SerializeField] protected FlexibleFloat speed = new FlexibleFloat(4f);
        [SerializeField] protected FlexibleVector2 offset;
        [SerializeField] protected SharedFloat playerSizeFactor;

        private Vector3 initialPos;
        private ITrackingManager trackingManager;

        /// <summary>
        /// Gets or sets the active state of the camera movement logic.
        /// </summary>
        /// <value><c>true</c> if the camera movement is active; otherwise <c>false</c>.</value>
        public bool IsActive
        {
            get { return active.Value; }
            set
            {
                if (active.IsLocal)
                    active.Value = value;
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            initialPos = transform.localPosition;
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                trackingManager = player.GetComponentInChildren<ITrackingManager>();

                if (trackingManager == null)
                {
                    Debug.LogError("[CameraMovement] Player gameobject does not have a pose-tracker component assigned!");
                    enabled = false;
                }
            }
            else
            {
                Debug.LogError("[CameraMovement] No player gameobject found in scene!");
                enabled = false;
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            Vector3 localPosition = initialPos;

            if (active.Value)
            {
                Vector2 camOffset = offset; // Cache getter
                Vector3 diff = (trackingManager[Tracker.LeftWrist].Position - trackingManager[Tracker.RightWrist].Position) / 2f;
                Vector3 halfOffset = camOffset / 2f;

                diff += trackingManager[Tracker.RightWrist].Position;
                localPosition = new Vector3(
                    diff.x * camOffset.x,
                    (diff.y + halfOffset.y) * camOffset.y,
                    0f
                );

                // Failsafe
                if ((localPosition.y > 3f) || (localPosition.y < -0.2f))
                {
                    active.Value = false;
                    Debug.LogWarning("[CameraMovement] Impossible player position! Disable camera movement...");
                }
            }

            // Transform
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                localPosition,
                Time.deltaTime * speed
            );
        }
    }
}
