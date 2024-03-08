using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private const string LEVEL_PATH = "Levels";
    public LevelData LoadLevel(int levelIndex)
    {
        return Resources.Load<LevelData>(LEVEL_PATH + "/Level" + levelIndex);
    }
    public List<LevelData> LoadAllLevels()
    {
        return Resources.LoadAll<LevelData>(LEVEL_PATH).ToList();
    }
}
