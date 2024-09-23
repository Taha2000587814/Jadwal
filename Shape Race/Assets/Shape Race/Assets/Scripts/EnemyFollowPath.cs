using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class EnemyFollowPath : MonoBehaviour
{

    public EndOfPathInstruction endOfPathInstruction;
    public float movementSpeed = 5f;

    private PathCreator path;

    [HideInInspector]
    public float distanceTravelled;

    private Vector3 newPos;

    void Awake()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponent<PathCreator>();        //Initializes path
        distanceTravelled = FindObjectOfType<Spawner>().distanceTravelled;
        Move();     //Makes the enemy follow the path
    }

    void Update()
    {
        Move();     //Makes the enemy follow the path
    }
    
    public void Move()
    {
        distanceTravelled += movementSpeed * Time.deltaTime;
        newPos = path.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.position = newPos;
        transform.rotation = path.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        transform.Rotate(Vector3.forward, 90f);
    }
}
