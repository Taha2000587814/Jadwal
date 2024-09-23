using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public Mesh[] meshes;
    public ParticleSystem tokenParticle;

    [HideInInspector]
    public bool gameIsOver = false;

    private MeshFilter playerMesh;
    private ParticleSystem playerParticle;
    private Animation cameraAnim;


    void Start () {
        //Initizalization
        playerMesh = GetComponent<MeshFilter>();
        playerParticle = GetComponent<ParticleSystem>();
        cameraAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animation>();
	}
	
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))      //If Player collided with an enemy
        {
            playerParticle.Play();      //Plays playerParticle
            if (other.GetComponent<MeshFilter>().mesh.name == playerMesh.mesh.name)       //If the player and the enemy have the same mesh
            {
                FindObjectOfType<ScoreManager>().IncrementScore(350);       //Increments score by 350
                cameraAnim.Play();      //Plays the animation attached to the Main Camera
                Destroy(other.gameObject);      //Destroys Enemy
            }
            else         //They have different meshes
            {
                gameIsOver = true;      //Game is over
                FindObjectOfType<GameManager>().EndPanelActivation();       //Activates endPanel
                FindObjectOfType<PlayerMovement>().enabled = false;     //Stops player
                FindObjectOfType<FollowPath>().enabled = false;     //Stops player
                GetComponent<Collider>().enabled = false;       //Disables collider
                GetComponent<Renderer>().enabled = false;       //Makes player invisible
            }
        }
        else if (other.CompareTag("Obstacle"))         //If Player collided with an obstacle
        {
            playerParticle.Play();
            if (other.GetComponent<MeshFilter>().mesh.name == playerMesh.mesh.name)     //If the obstacle and the player have the same mesh
            {
                FindObjectOfType<ScoreManager>().IncrementScore(100, 0);        //Increments score by 100 (if we delete the second '0' parameter, then a text like "Good Job" will display too)
                cameraAnim.Play();      //Plays the animation attached to the Main Camera
                ChangeMesh();       //Changes player's mesh
            }
            else      //If they have different meshes
            {
                gameIsOver = true;      //Game is over
                FindObjectOfType<AudioManager>().DeathSound();      //Plays DeathSound
                FindObjectOfType<GameManager>().EndPanelActivation();       //Activates endPanel
                FindObjectOfType<PlayerMovement>().enabled = false;     //Stops player
                FindObjectOfType<FollowPath>().enabled = false;     //Stops player
                GetComponent<Collider>().enabled = false;       //Disables collider
                GetComponent<Renderer>().enabled = false;       //Makes player invisible
            }
        }
        else if (other.CompareTag("Token"))     //If player collides with a token
        {
            Destroy(other.gameObject);      //Destroys token
            Destroy(Instantiate(tokenParticle, transform.position, transform.rotation), 1.5f);      //Instantiates tokenParticle, then destroys it after x seconds
            FindObjectOfType<ScoreManager>().IncrementToken();      //Increments tokenCounter
        }
    }

    public void ChangeMesh()
    {
        //Selects a random mesh from the meshes list until it is a different mesh than the player's mesh. Then changes player' s mesh to the selected one
        int index;
        do
        {
            index = Random.Range(0, meshes.Length);
        } while (playerMesh.mesh.name == meshes[index].name + " Instance");
        playerMesh.mesh = meshes[index];
    }
}
