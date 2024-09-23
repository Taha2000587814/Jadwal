using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Untagged"))       //If gameObject collides with anything which has a tag
        {
            Destroy(other.gameObject);      //Destroys it
        }
    }
}
