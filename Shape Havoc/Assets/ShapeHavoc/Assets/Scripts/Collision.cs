using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public GameObject destroyedParticle;
  
    void OnMouseDown()      //If the object has been clicked
    {
        if ((FindObjectOfType<Player>().ammoCount > 0) && (!FindObjectOfType<Player>().gameIsOver))     //If the player has ammo and the game is not over yet
        {
            FindObjectOfType<AudioManager>().HavocSound();      //Sound effect plays
            Destroy(Instantiate(destroyedParticle, transform.position, Quaternion.identity), 1f);       //Instantiates a particle and destroys it after x seconds
            Destroy(gameObject);        //Destroys the cube which has been clicked
            FindObjectOfType<Player>().ammoCount--;     //Reduces ammo
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))       //If gameobject collides with an obstacle, then game is over
        {
            //Game over functions
            FindObjectOfType<AudioManager>().DeathSound();
            Destroy(Instantiate(destroyedParticle, transform.position, Quaternion.identity), 1f);
            GetComponent<Animation>().Play("CubeDeathAnim");
            FindObjectOfType<Player>().gameIsOver = true;
            FindObjectOfType<GameManager>().EndPanelActivation();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animation>().Play("CameraDeathAnim");
        }
    }
}
