using System.Linq;
using UnityEditor;
using UnityEngine;

// IMPORTANT: This script must live inside a folder named "Editor"
// anywhere under Assets (e.g. Assets/Editor/CubeBehaviourEditor.cs).

[CustomEditor(typeof(CubeBehaviour)), CanEditMultipleObjects]
public class CubeBehaviourEditor : Editor
{
    private SerializedProperty sizeProp;

    private void OnEnable()
    {
        sizeProp = serializedObject.FindProperty("size");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        using (var changeCheck = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.PropertyField(sizeProp, new GUIContent("Size"));

            if (changeCheck.changed)
            {
                serializedObject.ApplyModifiedProperties();
                foreach (var t in targets)
                {
                    (t as CubeBehaviour)?.ApplyScale();
                }
            }
        }

        // Warnings based on the current value.
        if (sizeProp.floatValue > 2f)
        {
            EditorGUILayout.HelpBox("The cubes' sizes cannot be bigger than 2!", MessageType.Warning);
        }
        else if (sizeProp.floatValue <= 0f)
        {
            EditorGUILayout.HelpBox("The cubes' size must be greater than 0!", MessageType.Error);
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Select all cubes"))
            {
                var allCubes = FindObjectsOfType<CubeBehaviour>(true);
                Selection.objects = allCubes.Select(c => c.gameObject).ToArray();
            }

            if (GUILayout.Button("Clear selection"))
            {
                Selection.objects = new Object[0];
            }
        }

        EditorGUILayout.Space();

        // Colorized toggle button: green = currently active (click to disable),
        // red = currently inactive (click to enable).
        bool anyActive = targets.Cast<CubeBehaviour>().Any(c => c.gameObject.activeSelf);

        var cachedColor = GUI.backgroundColor;
        GUI.backgroundColor = anyActive ? Color.green : Color.red;

        if (GUILayout.Button("Disable/Enable all cubes", GUILayout.Height(30)))
        {
            foreach (var cube in FindObjectsOfType<CubeBehaviour>(true))
            {
                Undo.RecordObject(cube.gameObject, "Toggle Cube Active");
                cube.gameObject.SetActive(!anyActive);
            }
        }

        GUI.backgroundColor = cachedColor;
    }
}