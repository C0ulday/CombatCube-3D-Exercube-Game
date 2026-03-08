namespace Koboldgames.NonNull
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    using Object = UnityEngine.Object;

    internal abstract class BaseDrawer : PropertyDrawer
    {
        /// <summary>
        /// Find a Unity object that can be used to fill a specified field.
        /// </summary>
        /// <param name="property">The SerializedProperty to search an object instance for.</param>
        /// <param name="actionName">Outputs a string that describes the action to fill this field.</param>
        /// <returns>The object that has been found, or <c>null</c> if the search failed.</returns>
        public static Object FindObjectToFill(SerializedProperty property, out string actionName)
        {
            actionName = null;

            bool unsupported = (property.propertyType != SerializedPropertyType.ObjectReference) ||
                               (property.propertyPath.Contains(".Array"));

            if(unsupported)
                return null;

            Object targetObject = property.serializedObject.targetObject;
            Type objectType = targetObject.GetType();
            FieldInfo field = GetField(property.propertyPath, objectType);

            if(field == null)
                return null;

            Type fieldType = field.FieldType;

            // Is a component
            if(fieldType.IsSubclassOf(typeof(Component)))
            {
                Component component = targetObject as Component;

                // Search this component
                if(component != null)
                {
                    Component[] components = component.GetComponents(fieldType);

                    if(components.Length == 1)
                    {
                        actionName = "This";
                        return components[0];
                    }
                }

                actionName = "Fill";
                return FindSceneObjectToFill(fieldType);
            }

            // Is a scriptable object
            if(fieldType.IsSubclassOf(typeof(ScriptableObject)))
            {
                actionName = "Fill";
                return FindAssetObjectToFill(fieldType);
            }

            return null;
        }

        /// <summary>
        /// Find a Unity object that can be used to fill a field from the currently active scene.
        /// </summary>
        /// <param name="fieldType">The type of the component field.</param>
        /// <returns>The object that has been found, or <c>null</c> if the search failed.</returns>
        private static Object FindSceneObjectToFill(Type fieldType)
        {
            Object objectInScene = null;
            GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

            for(int i = 0; i < rootObjects.Length; i++)
            {
                Component[] candidates = rootObjects[i].GetComponentsInChildren(fieldType, true);

                if(candidates.Length == 1)
                {
                    if(objectInScene != null)
                        objectInScene = candidates[0];
                    else
                        return null;
                }
                else if(candidates.Length > 1)
                {
                    return null;
                }
            }

            return objectInScene;
        }

        /// <summary>
        /// Find a Unity object that can be used to fill a field from the assets folder.
        /// </summary>
        /// <param name="fieldType">The type of the component field.</param>
        /// <returns>The object that has been found, or <c>null</c> if the search failed.</returns>
        private static Object FindAssetObjectToFill(Type fieldType)
        {
            string[] objectsInAssets = AssetDatabase.FindAssets($"t:{fieldType.Name}");

            if(objectsInAssets.Length != 1)
                return null;

            return AssetDatabase.LoadAssetAtPath(
                AssetDatabase.GUIDToAssetPath(objectsInAssets[0]),
                fieldType
            );
        }

        /// <summary>
        /// Reflect a field from a property path on a specified type.
        /// </summary>
        /// <param name="propertyPath">The path to the property.</param>
        /// <param name="fromType">The type from which should be reflected.</param>
        /// <returns>The reflected field info.</returns>
        private static FieldInfo GetField(string propertyPath, Type fromType)
        {
            FieldInfo field = fromType.GetField(
                propertyPath,
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance
            );

            if(field != null)
                return field;

            Type baseType = fromType.BaseType;

            // Recursive field search
            if((baseType != null) && (baseType != typeof(object)))
                return GetField(propertyPath, baseType);

            return null;
        }

        /// <summary>
        /// Draw a non-null property field.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        /// <returns><c>true</c> if the property has children; otherwise <c>false</c>.</returns>
        protected bool NonNullPropertyField(Rect position, SerializedProperty property, GUIContent label)
        {
            bool hasChildren;
            string fillButtonText;
            Color tmpColor = GUI.backgroundColor;
            Object fillCandidate = FindObjectToFill(property, out fillButtonText);

            if(fillCandidate != null)
            {
                Rect propertyRect = new Rect(position);
                Rect buttonRect = new Rect(position);

                propertyRect.xMax -= 45f;
                buttonRect.xMin = propertyRect.xMax + 4f;

                GUI.backgroundColor = Color.red;
                hasChildren = EditorGUI.PropertyField(propertyRect, property, label);
                GUI.backgroundColor = tmpColor;

                if(GUI.Button(buttonRect, fillButtonText))
                    property.objectReferenceValue = fillCandidate;
            }
            else
            {
                GUI.backgroundColor = Color.red;
                hasChildren = EditorGUI.PropertyField(position, property, label);
                GUI.backgroundColor = tmpColor;
            }

            return hasChildren;
        }
    }
}
