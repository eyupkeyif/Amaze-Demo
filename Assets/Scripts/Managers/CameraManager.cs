using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera main;
    public void SetPosition(Vector3 position, int column,int row)
    {
        main.orthographicSize = row;
        main.gameObject.transform.position = new Vector3(position.x,main.gameObject.transform.position.y,position.z);
    }
}
