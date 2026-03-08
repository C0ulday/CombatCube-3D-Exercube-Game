using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sphery.ExerCube
{
    public class CalibrationController : MonoBehaviour
    {
        [SerializeField] protected float stepCompleteTime = 3f;
        [SerializeField] protected float stepResetTime = 5f;
        [SerializeField] protected float mappingTPoseDistance = 1f;
        [SerializeField] protected float startGroundDistance = 0.3f;
        [SerializeField] protected float minHeight = 1f;

        [SerializeField] protected CalibrationStep tPoseStep;
        [SerializeField] protected CalibrationStep groundStep;
        [SerializeField] protected CalibrationStep stretchStep;

        [SerializeField] protected TMP_Text instructions;
        [SerializeField] protected Image handRightIndicator;
        [SerializeField] protected Image handLeftIndicator;
        [SerializeField] protected Image ankleRightIndicator;
        [SerializeField] protected Image ankleLeftIndicator;
        [SerializeField] protected Color validColor = Color.green;
        [SerializeField] protected Color invalidColor = Color.red;

        protected ITrackingManager trackingManager;
        protected IPlayerCalibration playerCalibration;
        protected bool started = false;
        protected Step step = Step.TPose;

        private int previousTrackerCount = 0;
        private float progress = 0f;

        /// <summary>
        /// Step enumerator.
        /// </summary>
        protected enum Step
        {
            TPose,
            Ground,
            Stretch
        }

        /// <summary>
        /// Start the calibration routine.
        /// </summary>
        public void StartCalibrationRoutine()
        {
            if (started)
                return;

            playerCalibration.Reset();
            trackingManager.StartActiveMapping();

            ChangeStep(Step.TPose);
            started = true;
        }

        /// <summary>
        /// Cancel the calibration routine and skip calibration.
        /// </summary>
        public void CancelCalibrationRoutine()
        {
            playerCalibration.SkipCalibration();
            started = false;
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Search player with pose tracker data component
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                trackingManager = player.GetComponentInChildren<ITrackingManager>();
                playerCalibration = player.GetComponentInChildren<IPlayerCalibration>();

                if (trackingManager == null)
                    Debug.LogError("[CalibrationController] No player in scene found with an assigned pose tracker!");

                if (playerCalibration == null)
                    Debug.LogError("[CalibrationController] No player in scene found with an assigned calibration logic!");
            }
            else
            {
                Debug.LogError("[CalibrationController] No player gameobject in scene found!");
            }
        }

        /// <summary>
        /// Change the calibration step.
        /// </summary>
        /// <param name="newStep">The new step to change to.</param>
        protected void ChangeStep(Step newStep)
        {
            switch (newStep)
            {
                case Step.Ground:
                    instructions.text = groundStep.Text;
                    groundStep.SetProgress(0f);
                    StartCoroutine(groundStep.Highlight());
                    StartCoroutine(stretchStep.UnHighlight());
                    StartCoroutine(tPoseStep.UnHighlight());
                    break;

                case Step.Stretch:
                    instructions.text = stretchStep.Text;
                    stretchStep.SetProgress(0f);
                    StartCoroutine(stretchStep.Highlight());
                    StartCoroutine(groundStep.UnHighlight());
                    StartCoroutine(tPoseStep.UnHighlight());
                    break;

                default:
                    previousTrackerCount = trackingManager.TrackerCount;
                    instructions.text = tPoseStep.Text;
                    tPoseStep.SetProgress(0f);
                    StartCoroutine(tPoseStep.Highlight());
                    StartCoroutine(groundStep.UnHighlight());
                    StartCoroutine(stretchStep.UnHighlight());

                    if (trackingManager.TrackerCount >= 4)
                    {
                        handRightIndicator.color = validColor;
                        handLeftIndicator.color = validColor;
                        ankleRightIndicator.color = validColor;
                        ankleLeftIndicator.color = validColor;
                    }
                    else if (trackingManager.TrackerCount >= 2)
                    {
                        handRightIndicator.color = validColor;
                        handLeftIndicator.color = validColor;
                        ankleRightIndicator.color = Color.white;
                        ankleLeftIndicator.color = Color.white;
                    }
                    else
                    {
                        handRightIndicator.color = invalidColor;
                        handLeftIndicator.color = invalidColor;
                        ankleRightIndicator.color = Color.white;
                        ankleLeftIndicator.color = Color.white;
                    }
                    break;
            }

            progress = 0f;
            step = newStep;
        }

        /// <summary>
        /// T-pose detection.
        /// </summary>
        /// <returns><c>true</c> as long as T-pose is detected; otherwise <c>false</c>.</returns>
        private bool DetectingTPose()
        {
            return (trackingManager.TrackerCount > 0) &&
                (trackingManager[Tracker.RightWrist].Position.x - trackingManager[Tracker.LeftWrist].Position.x) >= mappingTPoseDistance;
        }

        /// <summary>
        /// Calibration ground pose detection.
        /// </summary>
        /// <returns><c>true</c> as long as T-pose is detected; otherwise <c>false</c>.</returns>
        private bool DetectingGroundPose()
        {
            float handHeight = (trackingManager[Tracker.RightWrist].Position.y + trackingManager[Tracker.LeftWrist].Position.y) * 0.5f;
            float ground = 0f;

            if (trackingManager.TrackerCount >= 4)
            {
                ground = (trackingManager[Tracker.RightAnkle].Position.y + trackingManager[Tracker.LeftAnkle].Position.y) * 0.5f;
                ground -= trackingManager.AnkleGroundHeight;
            }

            return (handHeight - ground) <= startGroundDistance;
        }

        /// <summary>
        /// Calibration stretch pose detection.
        /// </summary>
        /// <returns><c>true</c> as long as T-pose is detected; otherwise <c>false</c>.</returns>
        private bool DetectingStretchPose()
        {
            float handHeight = (trackingManager[Tracker.RightWrist].Position.y + trackingManager[Tracker.LeftWrist].Position.y) * 0.5f;
            float ground = 0f;

            if (trackingManager.TrackerCount >= 4)
            {
                ground = (trackingManager[Tracker.RightAnkle].Position.y + trackingManager[Tracker.LeftAnkle].Position.y) * 0.5f;
                ground -= trackingManager.AnkleGroundHeight;
            }

            return (handHeight - ground) >= minHeight;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!started)
                return;

            if (previousTrackerCount != trackingManager.TrackerCount)
                ChangeStep(Step.TPose);

            switch (step)
            {
                case Step.Ground:

                    if (DetectingGroundPose())
                        progress += Time.deltaTime / stepCompleteTime;
                    else
                        progress -= Time.deltaTime / stepResetTime;

                    progress = Mathf.Clamp01(progress);
                    groundStep.SetProgress(progress);

                    if (groundStep.Completed)
                        ChangeStep(Step.Stretch);

                    break;

                case Step.Stretch:

                    if (DetectingStretchPose())
                        progress += Time.deltaTime / stepCompleteTime;
                    else
                        progress -= Time.deltaTime / stepResetTime;

                    progress = Mathf.Clamp01(progress);
                    stretchStep.SetProgress(progress);

                    if (stretchStep.Completed)
                    {
                        playerCalibration.Calibrate();
                        started = false;
                    }

                    break;

                default:

                    if (DetectingTPose())
                        progress += Time.deltaTime / stepCompleteTime;
                    else
                        progress -= Time.deltaTime / stepResetTime;

                    progress = Mathf.Clamp01(progress);
                    tPoseStep.SetProgress(progress);

                    if (tPoseStep.Completed)
                        ChangeStep(Step.Ground);

                    break;
            }
        }

        /// <summary>
        /// Calibration step container with helper methods.
        /// </summary>
        [Serializable]
        protected class CalibrationStep
        {
#pragma warning disable CS0649
            [SerializeField] private RectTransform transform;
            [SerializeField] private Image slider;
            [SerializeField] private string text;
#pragma warning restore CS0649

            public string Text => text;
            public bool Completed => slider.fillAmount >= 1f;

            public IEnumerator Highlight() => ScaleRoutine(1.1f);
            public IEnumerator UnHighlight() => ScaleRoutine(1f);
            public void SetProgress(float t) => slider.fillAmount = t;

            private IEnumerator ScaleRoutine(float scale)
            {
                float t = 0f;
                Vector3 initialScale = transform.localScale;
                Vector3 targetScale = Vector3.one * scale;

                if (initialScale == targetScale)
                    yield break;

                do
                {
                    transform.localScale = Vector3.Slerp(initialScale, targetScale, t);
                    t += Time.deltaTime * 2f;
                    yield return null;
                } while (t >= 1f);

                transform.localScale = targetScale;
            }
        }
    }
}
