using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    #region Public
    public float speed, moveLimit;   //variable to make the enemy move
    #endregion

    #region Private
    private Vector2 direction;       //direction of movement
    private Rigidbody2D myBody;      //for rigidbody2D
    #endregion

    // Use this for initialization
    void Start ()
    {
        myBody = GetComponent<Rigidbody2D>(); //get the rigidbody2D component

        int r = Random.Range(0, 2); //select random number

        if (r == 0)                   //decide direction depending on numnber
            direction = Vector2.left;
        else
            direction = Vector2.right;

        transform.localScale = new Vector2(direction.x, 1); //decide localScale depending on numnber
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.gameOver)
            return;

        Movement();
    }

    /// <summary>
    /// Method which make enemy move
    /// </summary>
    void Movement()
    {
        if (transform.position.x >= moveLimit) //changes the direction and localScale when limit is exceed
        {
            direction = new Vector2(-1, 0);
            transform.localScale = new Vector2(direction.x, 1);
        }
        else if (transform.position.x <= -moveLimit)
        {
            direction = new Vector2(1, 0);
            transform.localScale = new Vector2(direction.x, 1);
        }
        //add the velocity in given direction
        myBody.velocity = new Vector2(speed * direction.x, myBody.velocity.y);

    }
}
