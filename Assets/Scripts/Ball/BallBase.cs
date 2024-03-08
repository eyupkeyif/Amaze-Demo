using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallBase : MonoBehaviour
{
    private BallController ballController;
    

    private void Start() 
    {
        ballController = gameObject.GetComponent<BallController>();
        EventManager.levelEvents.OnLevelPassed+=LevelSuccessHandler;
        EventManager.levelEvents.OnNewLevelStart+=Reset;
    }
    
    public void LevelSuccessHandler()
    {
        ballController.IsGameOVer(true);
    }
    public void Reset()
    {
        gameObject.SetActive(true);
        ballController.IsGameOVer(false);
        gameObject.transform.position = new Vector3(1,gameObject.transform.position.y,1);
    }

    
}
