using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {


    public float movementSpeed = 5f;

    private MeshFilter enemyMesh;
    private Vector3 newPos;
    private float posX;
    private bool canMove = true, moveRight = false, moveLeft = false;

    void Start()
    {
        enemyMesh = transform.GetChild(0).GetComponent<MeshFilter>();
        transform.localPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (moveLeft)
            MoveLeft();
        if (moveRight)
            MoveRight();
    }

    void Update()
    {
        if (transform.childCount < 1)
            Destroy(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Obstacle")) && (transform.childCount >= 1))       //If enemy collides with an obstacle
        {
            if (other.GetComponent<MeshFilter>().mesh.name != enemyMesh.mesh.name)      //If ahead of the enemy there is a obstacle with a different mesh
            {
                if (canMove)
                {
                    if ((moveRight == false) && (moveLeft == false))
                    {
                        if (transform.localPosition.x == 0f)
                        {
                            if (Random.Range(0, 2) == 0)
                                moveLeft = true;
                            else
                                moveRight = true;
                        }
                        else if (transform.localPosition.x > 0f)
                            moveLeft = true;
                        else
                            moveRight = true;
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        canMove = false;
        if (other.CompareTag("Obstacle"))       //If obstacle leaves the trigger
            Invoke("Stop", 0.05f);      //Stops enemy on the X axis
        Invoke("CanMoveAgain", 0.6f);
    }

    public void CanMoveAgain()
    {
        canMove = true;
    }

    public void Stop()
    {
        moveLeft = moveRight = false;
    }

    public void MoveLeft()
    {
        newPos = transform.localPosition;
        newPos.x -= 0.1f;
        transform.localPosition = newPos;
    }

    public void MoveRight()
    {
        newPos = transform.localPosition;
        newPos.x += 0.1f;
        transform.localPosition = newPos;
    }
}
