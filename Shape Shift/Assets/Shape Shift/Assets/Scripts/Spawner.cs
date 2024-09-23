using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obstacles;

    public float obstacleSpeed;

    private bool canSpawn = true, spawned = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))         //If an obstacle collides with the spawner, then it cannot spawn
            canSpawn = false;        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))         //If an obstacle collides leaves the trigger of the spawner, then it can spawn
        {
            canSpawn = true;
            Invoke("TryToSpawn", 0.8f);     //Tries to spawn again after x seconds
        }
    }

    public void Spawn()
    {
        if (!FindObjectOfType<GameManager>().gameIsOver)        //If the game is not over yet
        {
            spawned = true;

            GameObject tempObstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position, Quaternion.identity);     //Spawns a random obstacle
            tempObstacle.GetComponent<Rigidbody>().AddForce(Vector3.forward * -obstacleSpeed);      //Makes the obstacle move towards the player

            Invoke("CanSpawnAgain", 0.8f);
        }
    }

    public void CanSpawnAgain()
    {
        spawned = false;
    }

    public void TryToSpawn()
    {
        if (canSpawn && !spawned)
            Spawn();
    }
}
