using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Divide
{
    public enum GameState
    {
        Prepare,
        Playing,
        Paused,
        PreGameOver,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static event System.Action<GameState, GameState> GameStateChanged;

        private static bool isRestart;

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;

                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
            }
        }

        public static int GameCount
        {
            get { return _gameCount; }
            private set { _gameCount = value; }
        }

        private static int _gameCount = 0;

        [Header("Set the target frame rate for this game")]
        [Tooltip("Use 60 for games requiring smooth quick motion, set -1 to use platform default frame rate")]
        public int targetFrameRate = 60;

        private GameState _gameState = GameState.Prepare;

        // List of public variables referencing other objects
        [Header("Object References")]
        public PlayerController playerController;
        public GameObject boundary;
        public GameObject gameField;


        private float originalSizePlayerPar;

        void OnEnable()
        {
            PlayerController.PlayerDied += PlayerController_PlayerDied;
            BoundaryControl.ScalingEvent += ActivePlayerController;
        }

        void OnDisable()
        {
            PlayerController.PlayerDied -= PlayerController_PlayerDied;
            BoundaryControl.ScalingEvent -= ActivePlayerController;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            // Initial setup
            Application.targetFrameRate = targetFrameRate;
            ScoreManager.Instance.Reset();

            ParticleSystem parPlayer = playerController.gameObject.GetComponent<ParticleSystem>();
            var main = parPlayer.main;
            originalSizePlayerPar = main.startSize.constant;

            PrepareGame();
        }


        // Listens to the event when player dies and call GameOver
        void PlayerController_PlayerDied()
        {
            GameOver();
        }

        // Make initial setup and preparations before the game can be played
        public void PrepareGame()
        {
            GameState = GameState.Prepare;

            // Automatically start the game if this is a restart.
            if (isRestart)
            {
                isRestart = false;
                StartGame();
            }
        }

        // A new game official starts
        public void StartGame()
        {
            GameState = GameState.Playing;
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
            }
        }

        // Called when the player died
        public void GameOver()
        {
            if (SoundManager.Instance.background != null)
            {
                SoundManager.Instance.StopMusic();
            }
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
            StartCoroutine(CR_HideObject());
            GameState = GameState.GameOver;
            GameCount++;

            // Add other game over actions here if necessary
        }

        IEnumerator CR_HideObject()
        {
            yield return new WaitForSeconds(0.2f);
            boundary.SetActive(false);
            gameField.SetActive(false);
        }
        // Start a new game
        public void RestartGame(float delay = 0)
        {
            isRestart = true;
            StartCoroutine(CRRestartGame(delay));
        }

        IEnumerator CRRestartGame(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void HidePlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(false);
        }

        public void ShowPlayer()
        {
            if (playerController != null)
                playerController.gameObject.SetActive(true);
        }

        void ActivePlayerController(bool isDisable)
        {
            SpriteRenderer spritePlayer = playerController.gameObject.GetComponent<SpriteRenderer>();
            if (spritePlayer != null)
                spritePlayer.enabled = !isDisable;
            ParticleSystem parPlayer = playerController.gameObject.GetComponent<ParticleSystem>();
            var main = parPlayer.main;
            if (parPlayer != null)
            {
                if (isDisable)
                    main.startSize = 0;
                else
                    main.startSize = originalSizePlayerPar;
            }
        }
    }
}
