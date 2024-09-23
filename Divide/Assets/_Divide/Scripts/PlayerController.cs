using UnityEngine;
using System.Collections;

namespace Divide
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public static event System.Action PlayerDied;

        public GameObject changeDirObject;

        public float moveSpeed;

        Vector2 moveDirection;                          //direction move of player

        bool canMove;                                   //player whether can move

        //	public bool isCollisionWithGameField;			//check whether player is locate on game field?
        //	public LayerMask whatIsGameField;			

        Rigidbody2D body;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
            body = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
            BoundaryControl.ScalingEvent += OnStopPlayer;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
            BoundaryControl.ScalingEvent -= OnStopPlayer;
        }

        // Listens to changes in game state
        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                // Do whatever necessary when a new game starts
                moveDirection = new Vector2(Random.value, Random.value);
                body.velocity = moveDirection.normalized * moveSpeed * Time.fixedDeltaTime * 10;
            }
        }

        //Listens to scaling event from boundary control
        void OnStopPlayer(bool state)
        {
            if (state == true)
            {
                body.velocity = Vector3.zero;
            }
            else {
                moveDirection = new Vector2(Random.value, Random.value);
                body.velocity = moveDirection.normalized * moveSpeed * Time.fixedDeltaTime * 10;
            }
        }

        //void Update()
        //{
        //    //		isCollisionWithGameField = Physics2D.OverlapCircle (transform.position, 0.2f, whatIsGameField);
        //}

        // Calls this when the player dies and game over
        IEnumerator CrDie()
        {
            body.velocity = Vector3.zero;
            // Fire event
            if (PlayerDied != null)
                PlayerDied();
            CameraController.Instance.ShakeCamera();
            yield return new WaitForSeconds(0.5f);
            DrawLines.Instance.DestroyLinesRender();
            DragDirObject.Instance.transform.parent = DragDirObject.Instance.parent;
            DragDirObject.Instance.transform.localPosition = Vector3.zero;
            DecideDirection.Instance.SetRandomDirection();

        }


        GameObject SpawnChangeDirObj()
        {
            GameObject changeDirObj = Instantiate(changeDirObject);
            changeDirObj.transform.position = transform.position;
            changeDirObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            return changeDirObj;
        }

        void OnTriggerEnter2D(Collider2D target)
        {
            if (target.gameObject.tag == "ChangeDirection")
            {
                moveDirection = new Vector2(Random.value, Random.value);
                body.velocity = moveDirection.normalized * moveSpeed * Time.fixedDeltaTime * 10;
                Destroy(target.gameObject, 0.3f);
            }
            if (target.gameObject.tag == "LineRender")
            {
                StartCoroutine(CrDie());
            }
        }

    }
}
