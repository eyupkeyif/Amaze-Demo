using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] InputManager inputManager;
    [SerializeField] LayerMask layerMask;
    Vector3 movePosition;
    [SerializeField] ParticleSystem _SplashPrefab;
    [SerializeField] ParticleSystem _SpillPrefab;
    bool isGameOver=false;
    bool canMove=false;
    void Update()
    {
 
        Movement(); 
       
    }

    private void Movement()
    {
        
        movePosition = new Vector3(inputManager.GetInput().x,0,inputManager.GetInput().y);        

        RaycastHit hit;
        
        bool isHit = Physics.Raycast(transform.position,movePosition,out hit,Mathf.Infinity,layerMask);

        if (isHit)
        {

            if (hit.distance>transform.localScale.z)
            {   
                if (movePosition!=Vector3.zero)
                {
                    transform.Translate(movePosition*speed*Time.fixedDeltaTime);
                    canMove=true;
                }
                
            }
            else
            {
                if (canMove && !isGameOver)
                {
                    Vector3 finalPos = hit.transform.position-movePosition;
                    transform.position = new Vector3 (finalPos.x,transform.position.y,finalPos.z);                    
                    
                    HitEffect(hit,finalPos);

                    canMove=false;
                }
                
                    inputManager.ResetInput();
            }

            
        }
    }

    public void HitEffect(RaycastHit hit,Vector3 finalPos)
    {
        ParticleSystem splas = Instantiate(_SplashPrefab, hit.transform.position+Vector3.up, Quaternion.identity);
        Vector3 forward = movePosition;
        forward.y = 0;
        splas.transform.forward = forward;

        if (Vector3.Angle(finalPos, Vector3.up) > 30)
        {
            ParticleSystem spill = Instantiate(_SpillPrefab, hit.transform.position+Vector3.up, Quaternion.identity);
            spill.transform.forward = forward;
        }
    }

    public void IsGameOVer(bool isOVer)
    {
        isGameOver = isOVer;
    }

}
