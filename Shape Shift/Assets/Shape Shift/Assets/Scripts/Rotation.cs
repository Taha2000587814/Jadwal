using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 150f * Time.deltaTime);        //Rotates the gameObject on the Y axis    
    }
}
