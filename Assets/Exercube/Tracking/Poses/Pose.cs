using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif

namespace Sphery.ExerCube
{
    [CreateAssetMenu(menuName = "ExerCube/Pose", fileName = "Pose.asset")]
    public class Pose : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("Display")]
        [SerializeField] protected Sprite poseSprite = null;
        [SerializeField] protected Sprite cueSprite = null;
        [SerializeField] protected string poseTitle = null;

        [Header("Animation")]
        [SerializeField] protected string poseAnimatorTrigger = null;

        [Header("Tracking")]
        [SerializeField] protected bool isCompoundPose = false;
        [SerializeField] protected bool isHeldPose = false;
        [SerializeField] protected bool ignorePlayerSizeFactor = false;

        [SerializeField] protected Bounds handRight = new Bounds(Vector3.right + Vector3.up, Vector3.one);
        [SerializeField] protected Bounds handLeft = new Bounds(Vector3.left + Vector3.up, Vector3.one);
        [SerializeField] protected Bounds ankleRight = new Bounds(Vector3.right, Vector3.one);
        [SerializeField] protected Bounds ankleLeft = new Bounds(Vector3.left, Vector3.one);

        [SerializeField] protected float handDistance = 0f;
        [SerializeField] protected float ankleDistance = 0f;
        [SerializeField] protected float handAnkleDistance = 0f;

        [SerializeField] protected Vector2 oscillate = Vector2.zero;
        [SerializeField] protected Vector2 oscillateAnkles = Vector2.zero;
        [SerializeField] protected Pose secondaryPose = null;

#if UNITY_EDITOR
        [System.NonSerialized] private readonly Color handleBlue = new Color(33f / 255f, 55f / 255f, 153f / 255f);
        [System.NonSerialized] private readonly Color handleRed = new Color(1f, 32f / 255f, 39f / 255f);

        [System.NonSerialized] private BoxBoundsHandle boundsHandRightHandle = new BoxBoundsHandle();
        [System.NonSerialized] private BoxBoundsHandle boundsHandLeftHandle = new BoxBoundsHandle();
        [System.NonSerialized] private BoxBoundsHandle boundsAnkleRightHandle = new BoxBoundsHandle();
        [System.NonSerialized] private BoxBoundsHandle boundsAnkleLeftHandle = new BoxBoundsHandle();
#endif

        /// <summary>
        /// Gets the sprite icon for this pose.
        /// </summary>
        /// <value>The sprite icon for this pose.</value>
        public Sprite PoseSprite => poseSprite;

        /// <summary>
        /// Gets the sprite icon for the cognitive spatial cue of this pose.
        /// </summary>
        /// <value>The cue icon for this pose.</value>
        public Sprite CueSprite => cueSprite;

        /// <summary>
        /// Gets a string representation of this pose.
        /// </summary>
        /// <value>The string representation of this pose.</value>
        public string PoseTitle => poseTitle;

        /// <summary>
        /// Gets the animation trigger string used for this pose.
        /// </summary>
        /// <value>The animation trigger string used for this pose.</value>
        public string PoseAnimatorTrigger => poseAnimatorTrigger;

        /// <summary>
        /// Gets a value indicating if this is a compound pose.
        /// </summary>
        /// <value><c>true</c> if this pose is compound; otherwise <c>false</c>.</value>
        public bool IsCompoundPose => isCompoundPose;

        /// <summary>
        /// Gets a value indicating if this is a held pose.
        /// </summary>
        /// <value><c>true</c> if this pose is held; otherwise <c>false</c>.</value>
        public bool IsHeldPose => isHeldPose;

        /// <summary>
        /// Gets the value indicating if the player size should be ignored.
        /// </summary>
        /// <value><c>true</c> if player size is ignored; otherwise <c>false</c>.</value>
        public bool IgnorePlayerSizeFactor => ignorePlayerSizeFactor;

        /// <summary>
        /// Gets the hand right bounding box (AABB) for tracking.
        /// </summary>
        /// <value>The hand right bounding box for tracking.</value>
        public Bounds HandRight => handRight;

        /// <summary>
        /// Gets the hand left bounding box (AABB) for tracking.
        /// </summary>
        /// <value>The hand left bounding box for tracking.</value>
        public Bounds HandLeft => handLeft;

        /// <summary>
        /// Gets the ankle right bounding box (AABB) for tracking.
        /// </summary>
        /// <value>The ankle right bounding box for tracking.</value>
        public Bounds AnkleRight => ankleRight;

        /// <summary>
        /// Gets the ankle left bounding box (AABB) for tracking.
        /// </summary>
        /// <value>The ankle left bounding box for tracking.</value>
        public Bounds AnkleLeft => ankleLeft;

        /// <summary>
        /// Gets the minimal distance between the arms in order for this pose to evaluate correctly.
        /// </summary>
        /// <value>The minimal distance between the arms.</value>
        public float HandDistance => handDistance;

        /// <summary>
        /// Gets the minimal distance between the ankles in order for this pose to evaluate correctly.
        /// </summary>
        /// <value>The minimal distance between the ankles.</value>
        public float AnkleDistance => ankleDistance;

        /// <summary>
        /// Gets the minimal distance between the arms and the ankles in order for this pose to evaluate correctly.
        /// </summary>
        /// <value>The minimal distance between the arms and the ankles.</value>
        public float HandAnkleDistance => handAnkleDistance;

        /// <summary>
        /// Gets the minimum oscillate offset for the trackers.
        /// Used for held poses.
        /// </summary>
        /// <value>The minimum oscillate offset.</value>
        public Vector2 Oscillate => oscillate;

        /// <summary>
        /// Gets the minimum oscillate offset for the ankle trackers.
        /// Used for held poses.
        /// </summary>
        /// <value>The minimum oscillate offset.</value>
        public Vector2 OscillateAnkles => oscillateAnkles;

        /// <summary>
        /// Gets the secondary pose associated with this pose.
        /// Used for compound poses.
        /// </summary>
        /// <value>The secondary pose.</value>
        public Pose Secondary => secondaryPose;

#if UNITY_EDITOR

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            boundsHandRightHandle.axes = PrimitiveBoundsHandle.Axes.All;
            boundsHandRightHandle.center = handRight.center;
            boundsHandRightHandle.size = handRight.size;
            boundsHandRightHandle.wireframeColor = handleBlue;
            boundsHandRightHandle.handleColor = handleRed;

            boundsHandLeftHandle.axes = PrimitiveBoundsHandle.Axes.All;
            boundsHandLeftHandle.center = handLeft.center;
            boundsHandLeftHandle.size = handLeft.size;
            boundsHandLeftHandle.wireframeColor = handleBlue;
            boundsHandLeftHandle.handleColor = handleRed;

            boundsAnkleRightHandle.axes = PrimitiveBoundsHandle.Axes.All;
            boundsAnkleRightHandle.center = ankleRight.center;
            boundsAnkleRightHandle.size = ankleRight.size;
            boundsAnkleRightHandle.wireframeColor = handleBlue;
            boundsAnkleRightHandle.handleColor = handleRed;

            boundsAnkleLeftHandle.axes = PrimitiveBoundsHandle.Axes.All;
            boundsAnkleLeftHandle.center = ankleLeft.center;
            boundsAnkleLeftHandle.size = ankleLeft.size;
            boundsAnkleLeftHandle.wireframeColor = handleBlue;
            boundsAnkleLeftHandle.handleColor = handleRed;

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
#if UNITY_2019_1_OR_NEWER
        private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;
#else
        private void OnDisable() => SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif

        /// <summary>
        /// Enables the Editor to handle an event in the scene view.
        /// </summary>
        /// <param name="sceneView">Scene view object.</param>
        private void OnSceneGUI(SceneView sceneView)
        {
            if (this == null)
                return;

            if (Selection.activeObject == this)
            {
                GUIStyle styles = new GUIStyle(EditorStyles.label);
                styles.normal.textColor = handleRed;

                Handles.Label(handRight.center, $"Right Hand [{poseTitle}]", styles);
                boundsHandRightHandle.DrawHandle();
                handRight.center = boundsHandRightHandle.center;
                handRight.size = boundsHandRightHandle.size;

                Handles.Label(handLeft.center, $"Left Hand [{poseTitle}]", styles);
                boundsHandLeftHandle.DrawHandle();
                handLeft.center = boundsHandLeftHandle.center;
                handLeft.size = boundsHandLeftHandle.size;

                Handles.Label(ankleRight.center, $"Right Ankle [{poseTitle}]", styles);
                boundsAnkleRightHandle.DrawHandle();
                ankleRight.center = boundsAnkleRightHandle.center;
                ankleRight.size = boundsAnkleRightHandle.size;

                Handles.Label(ankleLeft.center, $"Left Ankle [{poseTitle}]", styles);
                boundsAnkleLeftHandle.DrawHandle();
                ankleLeft.center = boundsAnkleLeftHandle.center;
                ankleLeft.size = boundsAnkleLeftHandle.size;
            }
        }

#endif

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            boundsHandRightHandle.center = handRight.center;
            boundsHandRightHandle.size = handRight.size;
            boundsHandLeftHandle.center = handLeft.center;
            boundsHandLeftHandle.size = handLeft.size;
            boundsAnkleRightHandle.center = ankleRight.center;
            boundsAnkleRightHandle.size = ankleRight.size;
            boundsAnkleLeftHandle.center = ankleLeft.center;
            boundsAnkleLeftHandle.size = ankleLeft.size;
#endif
        }

    }
}
