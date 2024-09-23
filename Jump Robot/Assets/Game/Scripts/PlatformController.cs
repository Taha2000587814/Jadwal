﻿using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    public Color green, red;         //color code for platforms
    public GameObject[] enemies;     //0 for normal , 1 for fast and 2 for slow
    public Transform Spawner;        //ref spawn position for enemies
    public SpriteRenderer[] sprites; //ref to all the sprites of platform

    private Transform player;        //ref to player
    private GameObject lastEnemy;    //ref to the enemy which was activated
    private string detectedObject;   //ref to the player name when detected
    private AudioSource sound;

    [HideInInspector]
    public ManageVariables vars;

    void OnEnable()
    {
        vars = Resources.Load("ManageVariablesContainer") as ManageVariables;
    }

    // Use this for initialization
    void Start ()
    {
        sound = GetComponent<AudioSource>(); //get the sound component
        player = FindObjectOfType<PlayerController>().transform; //get the player transform
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check if the distance between player and itself is ledd than -3
        if (transform.position.y - player.position.y <= -3f)
        {   //if yes then check if there was any lastenemy game object assigned
            if(lastEnemy != null)
            {   //if yes the deactivate it and remove th referance
                lastEnemy.SetActive(false);
                lastEnemy = null;
            }
            detectedObject = ""; //set the string to nothing
            gameObject.SetActive(false); //deactivate the gameobject
        }
	}
    /// <summary>
    /// Method which perform few settings when the gameobject is activated in the scene
    /// </summary>
    public void Settings()
    {   //change color or all sprite to red
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = red;
        }

        int r = Random.Range(0, 6);         //select the random number
        float pos = Random.Range(-2f, 2f);  //select the random position within limits

        if (r >= 0 && r < 3) //for normal
        {
            enemies[0].transform.position = new Vector3(pos, Spawner.position.y);
            enemies[0].SetActive(true);
            lastEnemy = enemies[0];
        }
        else if (r == 3) //for fast
        {
            enemies[1].transform.position = new Vector3(pos, Spawner.position.y);
            enemies[1].SetActive(true);
            lastEnemy = enemies[1];
        }
        else if (r == 4) // for slow
        {
            enemies[2].transform.position = new Vector3(pos, Spawner.position.y);
            enemies[2].SetActive(true);
            lastEnemy = enemies[2];
        }
        else if (r == 5)
        {
            return;
        }
    }

    /// <summary>
    /// Method which detects the player
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {   //check the tag and compare the object name with the detecteObjet
        if (other.CompareTag("Player") && other.gameObject.name != detectedObject)
        {   //if above conditions are satisfied then assign the object name to detectedObject
            detectedObject = other.gameObject.name;
            //increase score , change color
            if (GameManager.instance.startGame == true)
            {   //check if game has started and the increase the score by 1
                GameManager.instance.currentScore++;
                NewHiScoreEffect();
            }

            for (int i = 0; i < sprites.Length; i++)
            {   //then make the sprites to green
                sprites[i].color = green;
            }
        }
    }

    /// <summary>
    /// Method which show hi score effect when the last best score is defeated
    /// </summary>
    void NewHiScoreEffect()
    {
        if (GameManager.instance.currentScore > GameManager.instance.bestScore &&
            GameManager.instance.scoreEffect == 0)
        {
            GameManager.instance.scoreEffect = 1;
            //spawn the scoreEffect
            GameObject scoreEffect = ObjectPooling.instance.GetScoreEffect();
            scoreEffect.transform.position = transform.position;
            scoreEffect.SetActive(true);
            sound.PlayOneShot(vars.hiScore);
            scoreEffect.GetComponent<DeactivateObject>().Deactivate();
        }
    }

}
