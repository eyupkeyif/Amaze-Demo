using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Vector2 dragInput,startInput,finalInput,direction;
    private void Update() 
    {
        if (direction==Vector2.zero)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startInput = (Vector2)Input.mousePosition;
            }
            else if(Input.GetMouseButton(0))
            {
                dragInput = (Vector2)Input.mousePosition;

                finalInput = dragInput-startInput;

                if (finalInput!=Vector2.zero)
                {
                    CheckInput(finalInput);
                }
            }
        }
        
        

    }

    private Vector2 CheckInput(Vector2 dir)
    {
        if (Mathf.Abs(dir.x)>Mathf.Abs(dir.y))
        {
            if (dir.x>0)
            {
                direction = new Vector2(1,0);
            }
            else
            {
                direction = new Vector2(-1,0);
            }
        }
        else
        {
            if (dir.y>0)
            {
                direction = new Vector2(0,1);
            }
            else
            {
                direction = new Vector2(0,-1);
            }
        }

        return direction;
    }

    public Vector2 GetInput()
    {
        return direction;
    }
    public void ResetInput()
    {
        direction = Vector2.zero;
    }
}
