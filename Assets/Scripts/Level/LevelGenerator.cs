using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Tile tilePrefab;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject treePrefab;
    private GameObject levelParent;
    private BallBase ball;
    private LevelData levelData;
    [SerializeField] CameraManager cameraManager;
    private List<Tile> tiles = new List<Tile>();
    [SerializeField] AssetManager assetManager;
    int levelNumber;
    int totalLevel;
    private void Start() 
    {
        totalLevel = assetManager.LoadAllLevels().Count;

        if (PlayerPrefs.HasKey("Level"))
        {
            levelNumber = PlayerPrefs.GetInt("Level");
        }
        else
        {
            levelNumber = 1;
            PlayerPrefs.SetInt("Level",levelNumber);
        }

        GenerateLevel(levelNumber);
        SetupBall();    
        SetupCamera();

        EventManager.levelEvents.OnTilePassed+=SuccessLevelHandler;

    }
    private void SetupBall()
    {
       ball = GameObject.FindObjectOfType<BallBase>(); 
       ball.transform.position = new Vector3(1,ball.transform.position.y,1);
    }
    private void SetupCamera()
    {
        Vector3 camPos = new Vector3((float)(levelData.row-1)/2,0,(float)(levelData.column-1)/2);
        cameraManager.SetPosition(camPos,levelData.column,levelData.row);
    }
    public void GenerateLevel(int levelIndex)
    {
        tiles.Clear();

        levelData = assetManager.LoadLevel(levelIndex);
        levelParent = new GameObject("Level " + levelIndex);
        levelParent.transform.position = Vector3.zero;
        levelParent.transform.localScale = Vector3.one;

        if (levelData)
        {
            for (int i = 0; i < levelData.row; i++)
            {
                for (int k = 0; k < levelData.column; k++)
                {
                    int val = levelData.levelArray[i*levelData.column + k];

                    switch (val)
                    {
                        case 0:
                        GameObject _wall = Instantiate(wall,new Vector3(i,0,k),Quaternion.identity);
                        _wall.transform.SetParent(levelParent.transform);
                        break;
                        case 1:
                        Tile tile = Instantiate(tilePrefab,new Vector3(i,0,k),Quaternion.identity);
                        tile.gameObject.transform.SetParent(levelParent.transform);
                        if (!tiles.Contains(tile))
                        {
                            tiles.Add(tile);
                        }
                        break;
                        default:
                        break;
                    }
                }
            }
        } 
    }
 
    private IEnumerator SuccessCoroutine(float time)
    {

        yield return new WaitForSeconds(2);

        float timer=0;

        ball.gameObject.SetActive(false);

        while (timer<time)
        {
            levelParent.transform.position = Vector3.Lerp(levelParent.transform.position,Vector3.left*levelData.row*2,timer/time);
            timer+=Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        Destroy(levelParent);

        GenerateLevel(levelNumber);
        
        SetupCamera();

        EventManager.levelEvents.OnNewLevelStart?.Invoke();

    }

    public void SuccessLevelHandler()
    {
        foreach (var item in tiles)
        {
            if (!item.IsPassed())
            {
                return;
            }
        }



        if (levelNumber<10)
        {
            levelNumber++;
        }
        else
        {
            levelNumber = UnityEngine.Random.Range(1,totalLevel);
        }
        
        PlayerPrefs.SetInt("Level",levelNumber);
        EventManager.levelEvents.OnLevelPassed?.Invoke();
        StartCoroutine(SuccessCoroutine(1));
    }

}
