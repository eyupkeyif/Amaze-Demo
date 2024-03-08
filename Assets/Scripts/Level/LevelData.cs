using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelData", order = 0)]
public class LevelData : ScriptableObject 
{

    public int[] levelArray;
    public int row;
    public int column;
    
}

