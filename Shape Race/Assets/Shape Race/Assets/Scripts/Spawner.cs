using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Spawner : MonoBehaviour {

    public GameObject[] obstacles;
    public GameObject enemy, token;
    public int tokenSpawnFrequency = 8;
    public float timeBetweenObstacleSpawns, minTimeBetweenSpawns, offsetY = 0.85f;

    public EndOfPathInstruction endOfPathInstruction;
    public float timeBetweenEnemies = 1.3f, firstEnemySpawn = 0.5f, firstObstacleSpawn = 1f, aheadOfPlayer = 100f;

    [HideInInspector]
    public float distanceTravelled;

    private PathCreator path;

    void Start()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponent<PathCreator>();        //Initializes path
        Invoke("Spawn", firstObstacleSpawn);        //Spawns first obstacle after x seconds
        Invoke("SpawnEnemy", firstEnemySpawn);      //Spawns first enemy after x seconds
    }

    void Update()
    {
        //Sets the spawner ahead of the player by aheadOfPlayer value
        distanceTravelled = FindObjectOfType<FollowPath>().distanceTravelled + aheadOfPlayer;
        transform.position = path.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = path.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
    }

    public void SpawnEnemy()
    {
        GameObject tempEnemy = Instantiate(enemy, transform.position, Quaternion.identity);       //Spawns an enemy to the spawner's position with the same rotation
        Vector3 enemyPos = transform.position;
        enemyPos.y += offsetY;
        tempEnemy.transform.SetPositionAndRotation(enemyPos, transform.rotation);

        if (!FindObjectOfType<Collision>().gameIsOver)
            Invoke("SpawnEnemy", timeBetweenEnemies);       //Spawns again after x seconds
    }

    public void SpawnToken()
    {
        //Spawns token to a random position
        GameObject tempToken = Instantiate(token, transform.position, transform.rotation);
        tempToken.transform.Rotate(Vector3.forward, 90f);
        Vector3 tokenPos = Vector3.zero;
        tokenPos.x = Random.Range(-7f, 7f);
        tokenPos.y = offsetY;
        tempToken.GetComponent<Transform>().GetChild(0).transform.localPosition = tokenPos;
    }

    public void Spawn()
    {
        GameObject tempObstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position, Quaternion.identity);       //Spawns a random obstacle to the spawner's position with the same rotation
        Vector3 obstPos = transform.position;
        obstPos.y += offsetY;
        tempObstacle.transform.SetPositionAndRotation(obstPos, transform.rotation);
        tempObstacle.transform.Rotate(Vector3.forward, 90f);

        if (!FindObjectOfType<Collision>().gameIsOver)        //Invokes the next spawn only if the game is not over
            Invoke("Spawn", timeBetweenObstacleSpawns);     //Next spawn after 'timeBetweenSpawns' secs

        if (Random.Range(0, tokenSpawnFrequency) == 0)      //If it is time to spawn a token
            Invoke("SpawnToken", timeBetweenObstacleSpawns / 2);
    }
}
