using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))       //If gameObject collides with an obstacle or with an enemy
        {
            Destroy(other.gameObject);      //Then destroys it
        }
    }
}
