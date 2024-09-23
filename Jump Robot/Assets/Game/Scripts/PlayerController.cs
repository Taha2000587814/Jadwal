using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    #region Public
    /// <summary>
    /// Reference to the variable used for player movement
    /// </summary>
    public float speed, moveLimit, minJump, maxJump;
    public LayerMask inAirLayer;
    #endregion

    #region Private
    private LayerMask currentLayer;               //keep track of player layer
    private float jumpForce;                      //jump force which is provided to player
    [SerializeField]
    private float groundRadius = 0.25f;           //radius of collider which detect ground
    [SerializeField]
    private Transform ground;                     //ref to the position from where we will create a raycast
    [SerializeField]
    private LayerMask whatIsGround;               //ref to the layer on which ground object are on
    private bool isGrounded;                      //ref to see if player is on ground
    private Rigidbody2D myBody;
    private Vector2 direction = new Vector2(1, 0);//movement direction
    private bool canJump = true;
    private AudioSource sound;
    #endregion


    [HideInInspector]
    public ManageVariables vars;
    /// <summary>
    /// Get the data from the resource
    /// </summary>
    void OnEnable()
    {
        vars = Resources.Load("ManageVariablesContainer") as ManageVariables;
    }


    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();   //ref to the rigidbody2D component on the object
        sound = GetComponent<AudioSource>();    //ref to the sound component on the object
        currentLayer = gameObject.layer;        //ref to the layer
        CameraFollow.instance.PlayerSettings(); //setting camera
    }

    void Update()
    {
        if (GameManager.instance.gameOver)//check if game is over
        {
            myBody.gravityScale = 0;
            return;
        }

        ChangeLayer(); //methode which change layer of player
        if (myBody.velocity.y <= 0) //check if the y velocity is negative
        {
            GroundCheck(); //if yes gravity check is on
        }

        Movement(); //methode performing movement
    }

    /// <summary>
    /// Method for changing the layer
    /// </summary>
    void ChangeLayer()
    {
        //if the ray detect platform and velocity is below zero the player layer is changed
        if (CreateRay() == true && myBody.velocity.y < 0)
        {
            gameObject.layer = currentLayer;
        }
        //same as above just layer is different
        else if (CreateRay() == false && myBody.velocity.y > 0)
        {
            gameObject.layer = 8;
        }
    }

    /// <summary>
    /// bool which create the ray
    /// </summary>
    bool CreateRay()
    {
        //create the raycast and provide few config like position , size layer etc.
        RaycastHit2D hit = Physics2D.Raycast(ground.position, Vector2.down, 0.5f, whatIsGround);
        Debug.DrawRay(ground.position, Vector2.down * 0.1f, Color.red); //to show on the game scene

        //here we check if the ray of the robot is detecting ground
        if (hit.collider != null)
        {
            //check which wall its has collided
            if (hit.collider.tag == "Platform")
            {
                return true; //if hitting platform we return true
            }
        }

        return false;// else retune false
    }

    /// <summary>
    /// Method responsible for movement
    /// </summary>
    void Movement()
    {
        if (transform.position.x >= moveLimit) //when reached to the limit changes the direction and localscale
        {
            direction = new Vector2(-1, 0);
            transform.localScale = new Vector2(direction.x, 1);
        }
        else if (transform.position.x <= -moveLimit)
        {
            direction = new Vector2(1, 0);
            transform.localScale = new Vector2(direction.x,1);
        }
        //velocity is given to the provided direction with the required speed
        myBody.velocity = new Vector2(speed * direction.x, myBody.velocity.y);

        //when player click on screen and few other conditions are true
        if (Input.GetMouseButtonDown(0) && canJump && GameManager.instance.startGame)
        {
            sound.PlayOneShot(vars.jumpSound);//play jump sound
            jumpForce = maxJump;//set the jump force
        }
        //when player click on screen and few other conditions are true
        if (Input.GetMouseButton(0) && canJump && GameManager.instance.startGame)
        {
            if (jumpForce > minJump)//check if jump force is greater than min jump force
            {
                jumpForce -= (Time.deltaTime * 1.8f);//if yes then its reduces with time
            }
            else
            {
                jumpForce = minJump;//when jump force become less than min force then they are set to be equal
                canJump = false;//and canjump is made false
            }          
            isGrounded = false;//and grounded is made false
            //we first make the y velocity zero
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
            //then add the force
            //myBody.velocity = new Vector2(myBody.velocity.x, jumpForce);
            myBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetMouseButtonUp(0) && canJump && GameManager.instance.startGame)
        {
            canJump = false;
        }

    }

    //this methode keep track of ground objects
    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(ground.transform.position, groundRadius, whatIsGround);
        //we check if player is grounded and can double jump is false we make jump true
        if (isGrounded)
        {
            canJump = true;
            jumpForce = minJump;
        }
    }
    //this methode is just to show the ground detector radius in editor
    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(ground.transform.position, new Vector3(0, 0, 1), groundRadius);
#endif
    }

    /// <summary>
    /// Method which detects the enemy when they collide
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //game over , play sound 
            sound.PlayOneShot(vars.gameOverSound);//play the gameover sound
            GameManager.instance.gameOver = true;//make game over true
        }
    }

}
