using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelCreator : EditorWindow {

    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;
    protected LevelData[] levels;
    public LevelData newLevel;
    private SerializedProperty column;
    private SerializedProperty row;
    private SerializedProperty levelArray;

    private void OnGUI() {

        levels = GetAllInstances<LevelData>();
        serializedObject = new SerializedObject(newLevel);
        column = serializedObject.FindProperty("column");
        row = serializedObject.FindProperty("row");
        levelArray = serializedObject.FindProperty("levelArray");    
        DrawProperties();

        if (GUILayout.Button("Save"))
        {
            AssetDatabase.CreateAsset(newLevel, "Assets/Resources/Levels/Level" + (levels.Length+1) + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }

        Apply();

    }

    protected void DrawProperties()
    {
        int width = column.intValue;
        int height = row.intValue;

        // Check if levelGrid is an array
        if (levelArray!=null && levelArray.isArray)
        {
            levelArray.arraySize = width*height;
            // Use nested loops to iterate and display grid elements
            for (int y = 0; y < height; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < width; x++)
                {
                    // Access the current element value
                    
                    int value = levelArray.GetArrayElementAtIndex(y * width + x).intValue;

                    // Use EditorGUILayout.IntField to display and modify the value
                    int modifiedValue = EditorGUILayout.IntField($"{x},{y}", value);

                    // Ensure modified value is within valid range (0 or 1)
                    modifiedValue = Mathf.Clamp(modifiedValue, 0, 1);

                    // Update the element value in the array only if necessary
                    if (modifiedValue != value)
                    {
                        levelArray.GetArrayElementAtIndex(y * width + x).intValue = modifiedValue;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Level data is not an array!", MessageType.Warning);
        }
    }

    public static T[] GetAllInstances<T>() where T:LevelData
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i]=AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}
