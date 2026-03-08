using System;
using UnityEngine;
using TMPro;
using Koboldgames.Primitives.Events;

namespace Sphery.ExerCube
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PoseTrackerDebugPanel : MonoBehaviour
    {
        [Header("Event Listeners")]
        [SerializeField] protected SharedEvent onDebugActivate;
        [SerializeField] protected SharedEvent onDebugDeactivate;
        [SerializeField] protected SharedPoseEvent onPoseChange;

        [Header("Text References")]
        [SerializeField] protected TMP_Text titleText;
        [SerializeField] protected TMP_Text trackingDataText;
        [SerializeField] protected TMP_Text poseDataText;

        protected const string DataTextFormat = "<mspace=0.7em>" +
                                                "x____ y____ z____\n" +
                                                "{0,5:F2} {1,5:F2} {2,5:F2}\n" +
                                                "{3,5:F2} {4,5:F2} {5,5:F2}\n" +
                                                "{6,5:F2} {7,5:F2} {8,5:F2}\n" +
                                                "{9,5:F2} {10,5:F2} {11,5:F2}" +
                                                "</mspace>";

        protected bool active = false;
        protected ITrackingManager poseTracker;

        /// <summary>
        /// On activate debug panel listener.
        /// </summary>
        private void OnActivate() => active = true;

        /// <summary>
        /// On deactivate debug panel listener.
        /// </summary>
        private void OnDeactivate() => active = false;

        /// <summary>
        /// On pose change in debug panel listener.
        /// </summary>
        /// <param name="pose">The new pose.</param>
        private void OnPoseChange(Pose pose)
        {
            titleText.text = pose.PoseTitle;

            Vector3 posRH = pose.HandRight.center;
            Vector3 posLH = pose.HandLeft.center;
            Vector3 posRA = pose.AnkleRight.center;
            Vector3 posLA = pose.AnkleLeft.center;

            poseDataText.text = String.Format(
                DataTextFormat,
                posRH.x, posRH.y, posRH.z,
                posLH.x, posLH.y, posLH.z,
                posRA.x, posRA.y, posRA.z,
                posLA.x, posLA.y, posLA.z
            );
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
                poseTracker = playerObj.GetComponentInChildren<ITrackingManager>();

                if (poseTracker == null)
                    Debug.LogError("[PlayerCalibration] No player in scene found with an assigned pose tracker!");
            }
            else
            {
                Debug.LogError("[PlayerCalibration] No player in scene found!");
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            if (onDebugActivate != null)
                onDebugActivate.AddListener(OnActivate);

            if (onDebugDeactivate != null)
                onDebugDeactivate.AddListener(OnDeactivate);

            if (onPoseChange != null)
                onPoseChange.AddListener(OnPoseChange);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            if (onDebugActivate != null)
                onDebugActivate.RemoveListener(OnActivate);

            if (onDebugDeactivate != null)
                onDebugDeactivate.RemoveListener(OnDeactivate);

            if (onPoseChange != null)
                onPoseChange.RemoveListener(OnPoseChange);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!active)
                return;

            Vector3 posRH = poseTracker[Tracker.RightWrist].Position;
            Vector3 posLH = poseTracker[Tracker.LeftWrist].Position;
            Vector3 posRA = poseTracker[Tracker.RightAnkle].Position;
            Vector3 posLA = poseTracker[Tracker.LeftAnkle].Position;

            trackingDataText.text = String.Format(
                DataTextFormat,
                posRH.x, posRH.y, posRH.z,
                posLH.x, posLH.y, posLH.z,
                posRA.x, posRA.y, posRA.z,
                posLA.x, posLA.y, posLA.z
            );
        }
    }
}
