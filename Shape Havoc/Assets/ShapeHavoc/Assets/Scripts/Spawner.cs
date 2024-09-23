using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] obstacles;
    public bool randomObstacles = false;
    public int fistTypeAfter = 3, secondTypeAfter = 7, thirdTypeAfter = 12;

    [HideInInspector]
    public int spawnedObstacles = 0;

    private int obstacleType = 0;       //This will help to choose the type of the playerPanel

	void Start () {
        Spawn();        //First spawn
	}

    public void Spawn()
    {
        if (randomObstacles)     //If you want the obstacles to be randomly spawned then set this value to true
        {
            int randomPos = Random.Range(0, obstacles.Length);
            Instantiate(obstacles[randomPos], transform.position, Quaternion.identity);       //Spawns a random obstacle to the spawner's position with the same rotation
            obstacleType = randomPos;
        }
        else
            IncreasingSpawn();      //This way the obstacle types are randomly selected

        FindObjectOfType<Player>().AddPlayerPanel(obstacleType);        //Adds the playerPanel based on the current obstacle type
        spawnedObstacles++;     //Counts the spawn rounds
    }

    public void IncreasingSpawn()
    {
        if (spawnedObstacles < fistTypeAfter)
        {
            Instantiate(obstacles[0], transform.position, Quaternion.identity);     //Instantiates first obstacle type at spawner's position with the same rotation
            obstacleType = 0;
        }
        else if ((spawnedObstacles >= fistTypeAfter) && (spawnedObstacles < secondTypeAfter))
        {
            Instantiate(obstacles[1], transform.position, Quaternion.identity);     //Instantiates second obstacle type at spawner's position with the same rotation
            obstacleType = 1;
        }
        else if ((spawnedObstacles >= secondTypeAfter) && (spawnedObstacles < thirdTypeAfter))
        {
            Instantiate(obstacles[2], transform.position, Quaternion.identity);     //Instantiates third obstacle type at spawner's position with the same rotation
            obstacleType = 2;
        }
        else
        {
            Instantiate(obstacles[3], transform.position, Quaternion.identity);     //Instantiates fourth obstacle type at spawner's position with the same rotation
            obstacleType = 3;
        }
    }
}
