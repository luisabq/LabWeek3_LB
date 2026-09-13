using System.Linq;
using UnityEditor;
using UnityEngine;

// IMPORTANT: This script must live inside a folder named "Editor"
// anywhere under Assets (e.g. Assets/Editor/SphereBehaviourEditor.cs).

[CustomEditor(typeof(SphereBehaviour)), CanEditMultipleObjects]
public class SphereBehaviourEditor : Editor
{
    private SerializedProperty radiusProp;

    private void OnEnable()
    {
        radiusProp = serializedObject.FindProperty("radius");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        using (var changeCheck = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.PropertyField(radiusProp, new GUIContent("Radius"));

            if (changeCheck.changed)
            {
                serializedObject.ApplyModifiedProperties();
                foreach (var t in targets)
                {
                    (t as SphereBehaviour)?.ApplyScale();
                }
            }
        }

        // Warnings based on the current value.
        if (radiusProp.floatValue < 1f)
        {
            EditorGUILayout.HelpBox("The spheres' radius cannot be smaller than 1!", MessageType.Warning);
        }
        else if (radiusProp.floatValue <= 0f)
        {
            EditorGUILayout.HelpBox("The spheres' radius must be greater than 0!", MessageType.Error);
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Select all spheres"))
            {
                var allSpheres = FindObjectsOfType<SphereBehaviour>(true);
                Selection.objects = allSpheres.Select(s => s.gameObject).ToArray();
            }

            if (GUILayout.Button("Clear selection"))
            {
                Selection.objects = new Object[0];
            }
        }

        EditorGUILayout.Space();

        bool anyActive = targets.Cast<SphereBehaviour>().Any(s => s.gameObject.activeSelf);

        var cachedColor = GUI.backgroundColor;
        GUI.backgroundColor = anyActive ? Color.green : Color.red;

        if (GUILayout.Button("Disable/Enable all spheres", GUILayout.Height(30)))
        {
            foreach (var sphere in FindObjectsOfType<SphereBehaviour>(true))
            {
                Undo.RecordObject(sphere.gameObject, "Toggle Sphere Active");
                sphere.gameObject.SetActive(!anyActive);
            }
        }

        GUI.backgroundColor = cachedColor;
    }
}