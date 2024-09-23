using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public GameObject tokenParticle, playerHolderTransparent;
    public Transform playerHolderTransform;

    private ParticleSystem playerParticle;
    private bool canCollide = true;

    void Start()
    {
        playerParticle = GetComponent<ParticleSystem>();        //Initializes playerParticle
    }

    public void OnTriggerEnter(Collider other)
    {
        if (canCollide)
        {
            if (other.CompareTag("Obstacle"))       //If Player collides with an Obstacle
            {
                FindObjectOfType<GameManager>().EndPanelActivation();       //Activates EndPanel
                canCollide = false;     //Player cannot collide with anything
            }
            else if (other.CompareTag("Token"))     //If Player collides with a Token
            {
                FindObjectOfType<ScoreManager>().IncrementToken();      //Increments tokenCounter
                Destroy(other.gameObject);      //Destroys Token
                Destroy(Instantiate(tokenParticle, other.transform.position, Quaternion.identity), 1.2f);       //Spawns a tokenParticle and destroys it after x seconds
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Score") && canCollide)        //If player exits Score trigger
        {
            FindObjectOfType<ScoreManager>().IncrementScore();      //Increments score
            playerParticle.Play();      //Plays playerParticle

            Destroy(Instantiate(playerHolderTransparent, playerHolderTransform.position, Quaternion.identity), 1f);     //Spawns playerHolderTransparent and destroys it after x seconds
        }
    }

    public void CanCollideAgain()
    {
        canCollide = true;      //Player can collide
    }
}
