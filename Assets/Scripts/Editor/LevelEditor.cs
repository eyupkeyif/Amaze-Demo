using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelEditor: EditorWindow 
{
    protected SerializedObject serializedObject;
    private SerializedProperty column;
    private SerializedProperty row;
    private SerializedProperty levelArray;
    protected LevelData[] levels;
    protected string selectedPropertyPath;
    protected string selectedProperty;

    [MenuItem("Amaze!!! Demo/Level Editor")]
    private static void ShowWindow() {
        var window = GetWindow<LevelEditor>();
        window.titleContent = new GUIContent("Level Editor");
        window.Show();
    }

    private void OnGUI() {

        levels = GetAllInstances<LevelData>();
        serializedObject = new SerializedObject(levels[0]);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150),GUILayout.ExpandHeight(true));
        DrawSliderBar(levels);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box" , GUILayout.ExpandHeight(true));

        if (selectedProperty!=null)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i].name==selectedProperty)
                {
                    serializedObject = new SerializedObject(levels[i]);
                    column = serializedObject.FindProperty("column");
                    row = serializedObject.FindProperty("row");
                    levelArray = serializedObject.FindProperty("levelArray");
                    DrawProperties();
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();

    }

    protected void DrawSliderBar(LevelData[] allLevels)
    {
        foreach (LevelData level in allLevels)
        {
            if (GUILayout.Button( level.name))
            {
                selectedPropertyPath =level.name.ToString();
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = selectedPropertyPath;
        }

        if (GUILayout.Button("Create Level"))
        {
            LevelData newLevel = ScriptableObject.CreateInstance<LevelData>();
            LevelCreator levelCreator = GetWindow<LevelCreator>("New Level");
            levelCreator.newLevel = newLevel;
        }
    }

    protected void DrawProperties()
    {
        EditorGUILayout.PropertyField(column);
        EditorGUILayout.PropertyField(row);
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
                    EditorGUILayout.LabelField($"{x},{y}",GUILayout.Width(20), GUILayout.Height(20));
                    int modifiedValue = EditorGUILayout.IntField(value, GUILayout.Width(20), GUILayout.Height(20));


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
        if (serializedObject!=null)
        {
            serializedObject.ApplyModifiedProperties();
        }  
        
    }
    
}
