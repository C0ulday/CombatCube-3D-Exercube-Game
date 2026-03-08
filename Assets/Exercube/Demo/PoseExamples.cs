using UnityEngine;
using UnityEngine.UI;

namespace Sphery.ExerCube
{
    public class PoseExamples : MonoBehaviour
    {
        [SerializeField] protected Pose testPose;
        [SerializeField] protected float oscillationDelay = 0.3f;
        [SerializeField] protected Image evalOutput;

        protected ITrackingManager poseTracker;

        private bool currentState = false;
        private bool previousState = false;
        private (int, int, int, int) previousOscillationData;
        private float oscillationTimeout = 0f;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("[PoseExamples] No player object found in scene!");
                return;
            }

            // Give me components
            poseTracker = player.GetComponentInChildren<ITrackingManager>();

            // Security checks
            if (poseTracker == null)
                Debug.LogError("[PoseExamples] No 'pose tracker' component found on player object!");
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            // Determine the type of pose
            bool isHeld = testPose.IsHeldPose;

            // Unity overrides equality operators to check floating point errors for us...
            bool isOscillating = isHeld && (testPose.Oscillate != Vector2.zero ||
                                            testPose.OscillateAnkles != Vector2.zero);

            // We differentiate between standard poses and held poses:
            // - A standard pose is just a positional tracking (ensuring trackers are in specified areas).
            // - A held pose just indicates that the positional tracking must be held over a specifc
            //   amount of time. Additional to that, it is able to be oscillating.
            //
            // An oscillating held pose basically means that hand or ankle (or either) trackers must
            // oscillate to each other over time (e.g. 'Tripple' pose).

            // Check if the trackers match the position of the given pose
            // It does not matter if the pose is held or not; every pose has positional area data
            if (poseTracker.IsPoseMatching(testPose))
            {
                // If the pose is held, you would just check it every frame of a specified time...

                // If it is oscillating we make some more checks:
                if (!isOscillating)
                {
                    currentState = true;
                }
                else
                {
                    // Getting oscillation data returns a tuple that is structures as follows:
                    (int handsHorizontal,
                     int handsVertical,
                     int anklesHorizontal,
                     int anklesVertical) = poseTracker.GetOscillationData(testPose);

                    // Every returned value indicates a specified oscillation state:
                    // 1  = Right tracker's axis coordinate is higher than the one on the left
                    // 0  = Left tracker's axis coordinate is higher than the one on the left
                    // -1 = Too little difference or invalid data
                    //
                    // Horizontal:  axis of moving the tracker left and right in the ExerCube
                    // Vertical:    axis of moving the tracker up and down in the ExerCube
                    //
                    // For tracking oscillating poses you would basically store the returned data
                    // and check for changes over time. Consider following example:
                    bool validHandsHorizontal = handsHorizontal == -1 || previousOscillationData.Item1 != handsHorizontal;
                    bool validHandsVertical = handsVertical == -1 || previousOscillationData.Item2 != handsVertical;
                    bool validAnklesHorizontal = anklesHorizontal == -1 || previousOscillationData.Item3 != anklesHorizontal;
                    bool validAnklesVertical = anklesVertical == -1 || previousOscillationData.Item4 != anklesVertical;
                    // We can just ignore invalid/non-relevant data (-1)

                    // Check validity; if trackers actually do oscillate
                    if (validHandsHorizontal && validHandsVertical && validAnklesHorizontal && validAnklesVertical)
                    {
                        currentState = true;
                        oscillationTimeout = 0f;
                        // Also reset the timeout here...
                    }
                    else
                    {
                        // No change in oscillation:
                        // We do not directly assign this as new pose state as it is very likely
                        // that between consecutive frames there is now oscillating change. We like
                        // to separately control this by a custom delay. If we have no change after
                        // this delay we set the state.
                        if (oscillationTimeout > oscillationDelay)
                        {
                            // Still no change...
                            currentState = false;
                            oscillationTimeout = 0f;
                        }
                        else
                        {
                            // Measure timeout
                            oscillationTimeout += Time.deltaTime;
                        }
                    }

                    // Save previous oscillation state for next frame
                    previousOscillationData = (handsHorizontal, handsVertical, anklesHorizontal, anklesVertical);
                }
            }
            else
            {
                // Pose is not matching the required areas
                currentState = false;
            }

            // Display if pose is correctly reproduced in the ExerCube or not
            // Only update UI image if an actual change happened; invoking Unity's redraw event only
            // if necessary (just a little performance tweak)
            if (currentState != previousState)
            {
                evalOutput.color = currentState ? Color.green : Color.red;
                previousState = currentState;
            }
        }
    }
}
