namespace Koboldgames.NonNull
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEditor;
    using UnityEditor.Compilation;
    using UnityEditor.SceneManagement;
    using UnityEditor.SettingsManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Koboldgames.NonNull.SettingsManagement;

    using Assembly = System.Reflection.Assembly;
    using Object = UnityEngine.Object;

    [InitializeOnLoad]
    internal static class Processor
    {
        private static Dictionary<Type, IList<NonNullData>> objectCache;

        [UserSetting("General Settings", "Run Processor On Enter Playmode")]
        private static NonNullSetting<bool> runOnPlay;

        [UserSetting("General Settings", "Include Prefabs And Assets")]
        private static NonNullSetting<bool> includeAssets;

        /// <summary>
        /// Initializes the static <see cref="Processor"/> class.
        /// </summary>
        static Processor()
        {
            objectCache = new Dictionary<Type, IList<NonNullData>>();
            runOnPlay = new NonNullSetting<bool>("general.runOnPlay", true);
            includeAssets = new NonNullSetting<bool>("general.includeAssets", false);

            if(runOnPlay.value)
                FindNonNull();
        }

        /// <summary>
        /// Starts an asynchronous task to check all fields that are decorated with a
        /// <see cref="NonNullAttribute"/> or <see cref="NonEmptyAttribute"/>.
        /// </summary>
        /// <returns>A task object representing this task.</returns>
        [MenuItem("Tools/NonNull/Check Currently Loaded")]
        public static Task FindNonNull()
        {
            var assemblies = CompilationPipeline.GetAssemblies();

            Task gatheringTask = Task.Run(() => GatherNonNullData(assemblies));
            Task processTask = gatheringTask.ContinueWith(
                (task) => ProcessSerializedFields(includeAssets.value),
                TaskScheduler.FromCurrentSynchronizationContext()
            );

            return Task.WhenAll(gatheringTask, processTask);
        }

        /// <summary>
        /// Starts an asynchronous task to check all fields that are decorated with a
        /// <see cref="NonNullAttribute"/> or <see cref="NonEmptyAttribute"/> in all the scenes that
        /// are listed in the build settings.
        /// </summary>
        /// <returns>A task object representing this task.</returns>
        [MenuItem("Tools/NonNull/Check All Scenes")]
        public static Task FindNonNullAllScenes()
        {
            var assemblies = CompilationPipeline.GetAssemblies();

            Task gatheringTask = Task.Run(() => GatherNonNullData(assemblies));
            Task processTask = gatheringTask.ContinueWith(
                (task) => ProcessSerializedFieldsAllScenes(),
                TaskScheduler.FromCurrentSynchronizationContext()
            );

            return Task.WhenAll(gatheringTask, processTask);
        }

        #region Processing

        /// <summary>
        /// Gathering type and field data (through reflection) of decorated classes/fields. This
        /// method does not depend on the Unity main thread and can run in a different context.
        /// Helps to prefilter what actually has to be checked.
        /// The gathered data will be stored in the static <see cref="objectCache"/> field.
        /// </summary>
        /// <param name="assemblies">An array of Unity assemblies to search in.</param>
        private static void GatherNonNullData(UnityEditor.Compilation.Assembly[] assemblies)
        {
            IList<NonNullData> list;
            var filteredAssemblies = assemblies.Where((assembly) => !assembly.name.StartsWith("Unity"));

            List<Assembly> allAssemblies = new List<Assembly>(filteredAssemblies.Count());
            List<Type> allTypes = new List<Type>();
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo field;

            // Gather the corresponding 'System.Reflection.Assembly' assemblies
            foreach(var assembly in filteredAssemblies)
                allAssemblies.Add(Assembly.Load(assembly.name));

            // Gather all types in the assemblies that inherit from 'UnityEngine.Component'
            foreach(var assembly in allAssemblies)
            {
                allTypes.AddRange(
                    assembly
                        .GetTypes()
                        .Where((t) => {
                            return (
                                typeof(Component).IsAssignableFrom(t) ||
                                typeof(ScriptableObject).IsAssignableFrom(t)
                            ) && !t.IsAbstract;
                        })
                );
            }

            objectCache.Clear();

            // Search for attributes on all the types
            foreach(var type in allTypes)
            {
                bool classAttr = type.GetCustomAttribute(typeof(NonNullAttribute), false) != null;

                // Gather all fields of this type
                fields.Clear();
                GetFieldsRecursive(type, fields);

                // Iterate fields and construct cacheable data
                for(int i = 0; i < fields.Count; i++)
                {
                    field = fields[i];

                    bool nonNull = classAttr || field.GetCustomAttribute(typeof(NonNullAttribute), false) != null;
                    bool allowNull = classAttr && field.GetCustomAttribute(typeof(AllowNullAttribute), false) != null;
                    bool nonEmpty = field.GetCustomAttribute(typeof(NonEmptyAttribute), false) != null;

                    // Field needs to be processed by the logic
                    if(nonNull || allowNull || nonEmpty)
                    {
                        NonNullData data = new NonNullData {
                            Field = field,
                            NonNull = nonNull && !allowNull,
                            NonEmpty = nonEmpty
                        };

                        if(objectCache.TryGetValue(type, out list))
                            list.Add(data);
                        else
                            objectCache.Add(type, new List<NonNullData> { data });
                    }
                }
            }
        }

        /// <summary>
        /// Process and check the decorated fields.
        /// Depends on data that has been stored in the <see cref="objectCache"/>.
        /// </summary>
        private static void ProcessSerializedFields(bool includeAssets = false)
        {
            NonNullData data;
            Object[] objects;
            string error;

            // Iterate cache
            foreach(var cacheData in objectCache)
            {
                if(cacheData.Key.IsGenericType)
                {
                    if(!cacheData.Key.IsValueType)
                    {
                        Debug.LogWarning(
                            "[NonNull] Fields of generic types are not supported.\n" +
                            $"Type '{cacheData.Key.Name}' contains one or more fields that can " +
                            $"not be serialized. You may mark this type as an abstract class."
                        );
                    }

                    continue;
                }

                if(includeAssets)
                {
                    objects = Resources.FindObjectsOfTypeAll(cacheData.Key);
                }
                else
                {
#if UNITY_2020_1_OR_NEWER
                    objects = Object.FindObjectsOfType(cacheData.Key, true);
#else
                    objects = Object.FindObjectsOfType(cacheData.Key);
#endif
                }

                // Recursive through all found objects
                for(int i = 0; i < objects.Length; i++)
                {
                    // Check all the decorated fields
                    for(int j = 0; j < cacheData.Value.Count; j++)
                    {
                        data = cacheData.Value[j];

                        // Detected null value
                        if(FieldIsNull(data.Field, objects[i]))
                        {
                            if(data.NonNull)
                            {
                                LogError(
                                    "Serialized value is null",
                                    objects[i],
                                    cacheData.Key,
                                    data.Field
                                );

                                if(Application.isPlaying)
                                    EditorApplication.isPaused = true;
                            }
                        }
                        // Detected empty value
                        else if(FieldIsEmpty(data.Field, objects[i], out error))
                        {
                            if(data.NonEmpty)
                            {
                                LogError(error, objects[i], cacheData.Key, data.Field);

                                if(Application.isPlaying)
                                    EditorApplication.isPaused = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process and check the decorated fields in all the scenes listed in the build settings.
        /// Loads and unloads every scene to be processed one by one.
        /// Depends on data that has been stored in the <see cref="objectCache"/>.
        /// </summary>
        private static void ProcessSerializedFieldsAllScenes()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            if(scenes.Length == 0)
            {
                Debug.LogWarning("[NonNull] No scenes in build settings, so no scenes checked.");
                return;
            }

            string[] loadedScenes = new string[EditorSceneManager.sceneCount];
            int activeScene = 0;

            // Save all currently loaded scenes
            for(int i = 0; i < loadedScenes.Length; i++)
            {
                Scene scene = EditorSceneManager.GetSceneAt(i);
                loadedScenes[i] = scene.path;

                if(EditorSceneManager.GetActiveScene() == scene)
                    activeScene = i;
            }

            // Load all scenes one by one that are staged in the build settings
            for(int i = 0; i < scenes.Length; i++)
            {
                bool canceled = EditorUtility.DisplayCancelableProgressBar(
                    "Checking all scenes...",
                    scenes[i].path,
                    i / (float)scenes.Length
                );

                if(!canceled)
                {
                    // Process loaded scene
                    EditorSceneManager.OpenScene(scenes[i].path, OpenSceneMode.Single);
                    ProcessSerializedFields();
                }
                else
                {
                    // Cancel now...
                    i = scenes.Length;
                }
            }

            EditorUtility.ClearProgressBar();

            // Restore the previously loaded scenes
            for(int i = 0; i < loadedScenes.Length; i++)
            {
                Scene scene = EditorSceneManager.OpenScene(
                    loadedScenes[i],
                    i == 0 ?
                        OpenSceneMode.Single :
                        OpenSceneMode.Additive
                );

                if(activeScene == i)
                    EditorSceneManager.SetActiveScene(scene);
            }
        }

        #endregion

        #region Reflection Helpers

        private static void GetFieldsRecursive(Type type, List<FieldInfo> collection)
        {
            collection.AddRange(
                type.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly
                )
            );

            if(type.BaseType != null)
                GetFieldsRecursive(type.BaseType, collection);
        }

        /// <summary>
        /// Checks if a field is <c>null</c> (reflection).
        /// </summary>
        /// <param name="fieldInfo">The specified field.</param>
        /// <param name="instance">The instance to be checked.</param>
        /// <returns><c>true</c> if the specified field is <c>null</c>; otherwise <c>false</c>.</returns>
        private static bool FieldIsNull(FieldInfo fieldInfo, object instance)
        {
            object fieldValue = fieldInfo.GetValue(instance);

            if(fieldValue is Object)
                return (fieldValue as Object) == null;

            return object.ReferenceEquals(fieldValue, null);
        }

        /// <summary>
        /// Checks if a field is empty (reflection).
        /// </summary>
        /// <param name="fieldInfo">The specified field.</param>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="error">The thrown error or <c>null</c> if none occured.</param>
        /// <returns><c>true</c> if the specified field is empty; otherwise <c>false</c>.</returns>
        private static bool FieldIsEmpty(FieldInfo fieldInfo, object instance, out string error)
        {
            object fieldValue = fieldInfo.GetValue(instance);

            error = null;

            if((fieldValue is String) && String.IsNullOrEmpty((string)fieldValue))
                error = "Empty string";
            else if((fieldValue is AnimationCurve) && (((AnimationCurve)fieldValue).length == 0))
                error = "Empty animation curve";
            else if((fieldValue is LayerMask) && ((LayerMask)fieldValue == 0))
                error = "Unspecified layer mask";
            else if((fieldValue is Array) && (((Array)fieldValue).Length == 0))
                error = "Empty array";
            else if((fieldValue is IList) && (((IList)fieldValue).Count == 0))
                error = "Empty list";
            else if((fieldValue is Color) && ((Color)fieldValue == new Color(0f, 0f, 0f, 0f)))
                error = "No color";
            else if((fieldValue is Enum) && ((int)fieldValue == 0))
                error = "Empty enum value";
            else if((fieldValue is int) && ((int)fieldValue == 0))
                error = "Zero integer value";
            else if((fieldValue is float) && Mathf.Approximately((float)fieldValue, 0f))
                error = "Zero float value";
            else if((fieldValue is double) && ((double)fieldValue == 0d))
                error = "Zero double value";
            else if(VectorIsEmpty(fieldValue))
                error = "Empty vector value";

            return error != null;
        }

        /// <summary>
        /// Check for an empty vector field.
        /// </summary>
        /// <param name="value">The reflected vector value.</param>
        /// <returns><c>true</c> if the specified vector is empty; otherwise <c>false</c>.</returns>
        private static bool VectorIsEmpty(object value)
        {
            return ((value is Vector2) && ((Vector2)value == Vector2.zero)) ||
                   ((value is Vector2Int) && ((Vector2Int)value == Vector2Int.zero)) ||
                   ((value is Vector3) && ((Vector3)value == Vector3.zero)) ||
                   ((value is Vector3Int) && ((Vector3Int)value == Vector3Int.zero)) ||
                   ((value is Vector4) && ((Vector4)value == Vector4.zero));
        }

        #endregion

        #region Error Logging

        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="error">The error message to log.</param>
        /// <param name="obj">The referenced object associated with the error.</param>
        private static void LogError(string error, Object obj) =>
            Debug.LogError($"[NonNull] {error} on {obj.name}.", obj);

        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="error">The error message to log.</param>
        /// <param name="obj">The referenced object associated with the error.</param>
        /// <param name="type">The type of instance the error was found in.</param>
        /// <param name="field">The field that triggered the error.</param>
        private static void LogError(string error, Object obj, Type type, FieldInfo field) =>
            Debug.LogError($"[NonNull] {error} for {field.Name} in '{type.Name}' on {obj.name}.", obj);

        #endregion
    }
}
