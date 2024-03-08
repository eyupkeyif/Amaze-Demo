using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer passedTilePrefab;
    bool isPassed=false;
    [SerializeField] Vine vine;
    void Awake()
    {
        
        passedTilePrefab.material.SetFloat("_Fill" ,.36f);
        passedTilePrefab.gameObject.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        passedTilePrefab.gameObject.SetActive(false);

        EventManager.levelEvents.OnLevelPassed+=SuccessLevelHandler;
        
    }

    private void OnTriggerEnter(Collider other) {

        BallBase ballBase = other.gameObject.GetComponent<BallBase>();

        if (ballBase && !isPassed)
        {
            isPassed=true;
            StartCoroutine(FillTile(1));
            Vector3 firstPos = transform.position;
            EventManager.levelEvents.OnTilePassed?.Invoke();
        }
    }

    private IEnumerator FillTile(float time)
    {
        float timer = 0;
        while (timer<=time)
        {
            
            // float fillAmount = Mathf.SmoothStep(passedTilePrefab.material.GetFloat("_Fill" ),1f,timer/time);
            passedTilePrefab.gameObject.SetActive(value: true);
            passedTilePrefab.gameObject.transform.localScale = Vector3.Lerp(passedTilePrefab.gameObject.transform.localScale,new Vector3(0.96f,0.1f,0.96f),timer/time);
            timer +=Time.fixedDeltaTime;
            // passedTilePrefab.material.SetFloat("_Fill" ,fillAmount);
            
            yield return new WaitForFixedUpdate();
        }
        // passedTilePrefab.material.SetFloat("_Fill" ,1f);
        passedTilePrefab.gameObject.transform.localScale = new Vector3(0.96f,0.1f,0.96f);
        
    }

    private void SuccessLevelHandler()
    {
        if (vine)
        {
            vine.gameObject.SetActive(true);
            vine.GrowVines();
        }
        
    }
    public bool IsPassed()
    {
        return isPassed;
    }
}
