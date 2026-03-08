using Sphery.ExerCube.Helpers;
using UnityEngine;

namespace Sphery.ExerCube
{
    /*
     *           2.2m
     *         -------- ∡(100°)
     *        /        \
     *  2.2m /    ↕     \ 2.2m
     *      /   2.167m   \
     *
     *            ↔
     *          2.964m
     */
    public class TrackerIndicators : MonoBehaviour
    {
        [SerializeField] protected Transform rightHandIndicator;
        [SerializeField] protected Transform leftHandIndicator;
        [SerializeField] protected Transform rightAnkleIndicator;
        [SerializeField] protected Transform leftAnkleIndicator;
        [SerializeField] protected float triggerDistance = 0.5f;
        [SerializeField] protected float indicatorDistance = 2f;
        [SerializeField, MinMaxRange(0f, 1f)] protected MinMaxRange indicatorSize = new MinMaxRange(0.1f, 1f);

        protected const float CubeWallSize = 2.2f;
        protected const float CubeDepth = 2.166577f;
        protected const float CubeAngleSin = 0.173648178f; // sin(10°)
        protected const float CubeAngleCos = 0.984807753f; // cos(10°)

        protected ITrackingManager trackingManager;
        protected ICameraRotator cameras;

        /// <summary>
        /// Position and scale a specified tracker indicator object.
        /// </summary>
        /// <param name="position">The position of the corresponding tracker object in the cube.</param>
        /// <param name="indicator">The indicator object transform.</param>
        protected void PositionAndScaleIndicator(Vector3 position, Transform indicator)
        {
            float halfDepth = CubeDepth * 0.5f;
            float halfWidth = CubeWallSize * 0.5f;

            float distToFront = halfDepth - position.z;
            float distToQuad = halfWidth - Mathf.Abs(position.x);
            float distQuadToSide = distToFront * CubeAngleSin;
            float distToSide = distToQuad + distQuadToSide;
            float distOnSideWall = distToFront * CubeAngleCos;

            float scale;

            // Display indicator on front wall
            if ((distToFront < distToSide) && (distToFront < triggerDistance))
            {
                scale = Mathf.Lerp(
                    indicatorSize.Max,
                    indicatorSize.Min,
                    distToFront / triggerDistance
                );

                indicator.position = cameras.Front.ViewportToWorldPoint(
                    new Vector3(
                        (halfWidth + position.x) / CubeWallSize,
                        position.y / CubeWallSize,
                        indicatorDistance
                    )
                );
            }
            // Display indicator on side wall
            else
            {
                scale = Mathf.Lerp(
                    indicatorSize.Max,
                    indicatorSize.Min,
                    distToSide / triggerDistance
                );

                // Right
                if (position.x > 0f)
                {
                    indicator.position = cameras.Right.ViewportToWorldPoint(
                        new Vector3(
                            distOnSideWall / CubeWallSize,
                            position.y / CubeWallSize,
                            indicatorDistance
                        )
                    );
                }
                // Left
                else
                {
                    indicator.position = cameras.Left.ViewportToWorldPoint(
                        new Vector3(
                            1f - (distOnSideWall / CubeWallSize),
                            position.y / CubeWallSize,
                            indicatorDistance
                        )
                    );
                }
            }

            indicator.localScale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                trackingManager = player.GetComponentInChildren<ITrackingManager>();

                if (trackingManager == null)
                    Debug.LogError("[TrackerIndicators] No pose tracker component assigned to player gameobject!");
            }
            else
            {
                Debug.LogError("[TrackerIndicators] No player gameobject found in scene!");
            }

            GameObject mainCamera = GameObject.FindWithTag("MainCamera");

            if (mainCamera != null)
            {
                cameras = mainCamera.GetComponent<ICameraRotator>();

                if (cameras == null)
                    Debug.LogError("[TrackerIndicators] No camera rotator component assigned to main camera!");
            }
            else
            {
                Debug.LogError("[TrackerIndicators] No main camera found in scene!");
            }

            // Security checks
            if (rightHandIndicator == null)
                Debug.LogError("[TrackerIndicators] No right hand tracker indicator assigned!");

            if (leftHandIndicator == null)
                Debug.LogError("[TrackerIndicators] No left hand tracker indicator assigned!");

            if (rightAnkleIndicator == null)
                Debug.LogError("[TrackerIndicators] No right ankle tracker indicator assigned!");

            if (leftAnkleIndicator == null)
                Debug.LogError("[TrackerIndicators] No left ankle tracker indicator assigned!");
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        private void LateUpdate()
        {
            Vector3 correction = trackingManager.LocalPositionCorrection;
            Vector3 rightHand = trackingManager[Tracker.RightWrist].Position;
            Vector3 leftHand = trackingManager[Tracker.LeftWrist].Position;
            Vector3 rightAnkle = trackingManager[Tracker.RightAnkle].Position;
            Vector3 leftAnkle = trackingManager[Tracker.LeftAnkle].Position;

            rightHand -= correction;
            leftHand -= correction;
            rightAnkle -= correction;
            leftAnkle -= correction;

            PositionAndScaleIndicator(rightHand, rightHandIndicator);
            PositionAndScaleIndicator(leftHand, leftHandIndicator);

            if (trackingManager.TrackerCount > 2)
            {
                if (!rightAnkleIndicator.gameObject.activeInHierarchy)
                    rightAnkleIndicator.gameObject.SetActive(true);

                if (!leftAnkleIndicator.gameObject.activeInHierarchy)
                    leftAnkleIndicator.gameObject.SetActive(true);

                PositionAndScaleIndicator(rightAnkle, rightAnkleIndicator);
                PositionAndScaleIndicator(leftAnkle, leftAnkleIndicator);
            }
            else
            {
                if (rightAnkleIndicator.gameObject.activeInHierarchy)
                    rightAnkleIndicator.gameObject.SetActive(false);

                if (leftAnkleIndicator.gameObject.activeInHierarchy)
                    leftAnkleIndicator.gameObject.SetActive(false);
            }
        }
    }
}
