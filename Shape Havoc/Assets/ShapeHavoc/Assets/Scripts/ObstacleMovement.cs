using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour {

    public float movementSpeed;

    private Vector3 nextPos;
    private bool spawned = false, speededUp = false;
    private Animation camAnim;

	void Start () {
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animation>();     //Initializes Animation
    }
	
	void Update () {
        if((FindObjectOfType<Player>().ammoCount < 1) && (!speededUp))      //If the player has no more ammo and the speed of the object is not increased yet
        {
            speededUp = true;
            movementSpeed = 50f;      //Then increases the speed
        }
        if (!FindObjectOfType<Player>().gameIsOver)     //If the game is not over yet
        {
            //Moves object towards the playerPanel
            nextPos = transform.position;
            nextPos.z -= movementSpeed * Time.deltaTime;
            transform.position = nextPos;
        }   
        if ((transform.position.z <= -0.5f) && (!spawned))      //If the obstackle passed the player, then calls Spawner's spawn function but only once
        {
            spawned = true;
            FindObjectOfType<Spawner>().Spawn();
            FindObjectOfType<ScoreManager>().IncrementScore();
            FindObjectOfType<AudioManager>().ScoreSound();
            camAnim.Play();
        }
	}
}
