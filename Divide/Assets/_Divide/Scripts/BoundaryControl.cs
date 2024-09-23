using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public class BoundaryControl : MonoBehaviour
    {

        public static BoundaryControl Instance { get; private set; }

        public static event System.Action<bool> ScalingEvent;

        [SerializeField]
        private PolygonMesh gameField;

        [SerializeField]
        private GameObject spawnDirObject;

        List<Vector2> pointsList = new List<Vector2>();

        bool isScaling;                                 //game is scaling?

        int countClick = 0;                             //test change game field with button change in UI

        int currentLevel;                               //Current level is passed

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
            UIManager.ChangePolygon += UpdateSizeBoundary;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
            UIManager.ChangePolygon -= UpdateSizeBoundary;
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
        }

        void Start()
        {
            currentLevel = 0;
            spawnDirObject.SetActive(false);
        }

        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                DecideDirection.Instance.SetRandomDirection();
                spawnDirObject.SetActive(true);
                for (int i = 0; i < gameField.verticesList.Count; i++)
                {
                    pointsList.Add(gameField.verticesList[i]);
                }
                pointsList.Add(gameField.verticesList[0]);
                SetupLineRender(pointsList);
                GetComponent<EdgeCollider2D>().points = pointsList.ToArray();
            }
        }

        /// <summary>
        /// Update size boundary and polygon with button on UI
        /// </summary>
        void UpdateSizeBoundary()
        {
            DrawLines.Instance.DestroyLinesRender();
            DragDirObject.Instance.transform.parent = DragDirObject.Instance.parent;
            DragDirObject.Instance.transform.localPosition = Vector3.zero;
            DecideDirection.Instance.SetRandomDirection();
            if (countClick > 0)
            {
                countClick = 0;
                PolygonMesh.Instance.UpdateMesh(PolygonMesh.Instance.InitVerticeList().ToArray());
                HoldGameFieldBG.Instance.UpdateMesh(PolygonMesh.Instance.InitVerticeList().ToArray());
                pointsList = new List<Vector2>();
                for (int i = 0; i < gameField.verticesList.Count; i++)
                {
                    pointsList.Add(gameField.verticesList[i]);
                }
                pointsList.Add(gameField.verticesList[0]);
                SetupLineRender(pointsList);
                GetComponent<EdgeCollider2D>().points = pointsList.ToArray();
                return;
            }
            Vector2[] vertices2D = new Vector2[] {
            new Vector2 (5, 5),
            new Vector2 (5, -5),
            new Vector2(-5,-5),
            new Vector2 (-5,2.5f),
            new Vector2 (2.5f,2.5f),
            new Vector2(2.5f,5),
        };
            PolygonMesh.Instance.UpdateMesh(vertices2D);
            HoldGameFieldBG.Instance.UpdateMesh(vertices2D);
            pointsList = new List<Vector2>();
            for (int i = 0; i < gameField.verticesList.Count; i++)
            {
                pointsList.Add(gameField.verticesList[i]);
            }
            pointsList.Add(gameField.verticesList[0]);
            SetupLineRender(pointsList);
            GetComponent<EdgeCollider2D>().points = pointsList.ToArray();
            countClick++;
        }


        /// <summary>
        /// Update boundary and polygon when direction item has completed drawed lines
        /// </summary>
        /// <param name="newVertices">New vertices.</param>
        public void UpdatePolygonBoundary(Vector2[] newVerticesList)
        {
            if (!isScaling)
            {
                DrawLines.Instance.DestroyLinesRender();
                DragDirObject.Instance.transform.parent = DragDirObject.Instance.parent;
                DragDirObject.Instance.transform.localPosition = Vector3.zero;
                DecideDirection.Instance.SetRandomDirection();
            }
            PolygonMesh.Instance.UpdateMesh(newVerticesList);
            pointsList = new List<Vector2>();
            for (int i = 0; i < newVerticesList.Length; i++)
            {
                pointsList.Add(newVerticesList[i]);
            }
            pointsList.Add(newVerticesList[0]);
            SetupLineRender(pointsList);
            if (!isScaling)
            {
                //turn off to test
                CheckBoundSizePolygon();
            }
            GetComponent<EdgeCollider2D>().points = pointsList.ToArray();
        }

        /// <summary>
        /// Setup line render follow boundary
        /// </summary>
        /// <param name="verticesList">Vertices list.</param>
        void SetupLineRender(List<Vector2> verticesList)
        {
            LineRenderer lineRender = GetComponent<LineRenderer>();
            lineRender.positionCount = verticesList.Count;
            for (int i = 0; i < verticesList.Count; i++)
            {
                lineRender.SetPosition(i, new Vector3(verticesList[i].x, verticesList[i].y, 0));
            }
        }


        /// <summary>
        /// Create new rectangle area to contain polygon mesh
        /// </summary>
        /// <param name="recPoint">Rec point.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        void CheckBoundSizePolygon()
        {
            //		Debug.Log("Size bound : "+PolygonMesh.Instance.GetBounds().size.magnitude);
            //		Debug.Log ("Bound center: " + PolygonMesh.Instance.GetBounds ().center);
            if (PolygonMesh.Instance.GetBounds().size.magnitude < PolygonMesh.Instance.sizeMin)
            {
                List<Vector2> endPosList = new List<Vector2>();
                for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                {
                    endPosList.Add(PolygonMesh.Instance.verticesList[i] * GetMinProportion());
                }
                StartCoroutine(CrMoveCameraToFieldCenter(PolygonMesh.Instance.GetBounds().center, endPosList));
            }
        }

        /// <summary>
        /// Calculate about proportion of between rect contain mesh and old rect
        /// </summary>
        float GetMinProportion()
        {
            float wProportion = HoldGameFieldBG.Instance.boundInit.size.x / PolygonMesh.Instance.GetBounds().size.x;
            float hProportion = HoldGameFieldBG.Instance.boundInit.size.y / PolygonMesh.Instance.GetBounds().size.y;
            return (wProportion > hProportion) ? hProportion : wProportion;
        }

        /// <summary>
        /// Process move camera turn back center position of game field whenever it is scale larger 
        /// </summary>
        /// <returns>The move camera to field center.</returns>
        /// <param name="endPos">End position.</param>
        /// <param name="targetPosList">Target position list.</param>
        IEnumerator CrMoveCameraToFieldCenter(Vector2 endPos, List<Vector2> targetPosList)
        {
            float waitingTime = 0.4f;
            float currentTime = 0;
            HoldGameFieldBG.Instance.UpdateMesh(PolygonMesh.Instance.verticesList.ToArray());
            Vector3 pos = Camera.main.transform.position;
            List<Vector2> tempPosList = PolygonMesh.Instance.verticesList;
            currentTime = 0;
            pos = Camera.main.transform.position;

            while (currentTime < waitingTime)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / waitingTime);
                for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                {
                    PolygonMesh.Instance.verticesList[i] = Vector2.Lerp(tempPosList[i], targetPosList[i], CameraController.Instance.moveType.Evaluate(t));
                }
                IsScalingBoundary = true;
                UpdatePolygonBoundary(PolygonMesh.Instance.verticesList.ToArray());
                //Vector3 endPosCam = new Vector3 (PolygonMesh.Instance.GetBounds ().center.x, PolygonMesh.Instance.GetBounds ().center.y, -10);
                //         Camera.main.transform.position = Vector3.Lerp(pos, endPosCam, CameraController.Instance.moveType.Evaluate(t));
                if (!PolygonMesh.Instance.GetBounds().Contains(PlayerController.Instance.transform.localPosition))
                {
                    PlayerController.Instance.transform.localPosition = PolygonMesh.Instance.GetBounds().center;
                    while (!PolygonMesh.Instance.GetBounds().Contains(PlayerController.Instance.transform.localPosition))
                    {
                        float randomX = Random.Range(PolygonMesh.Instance.GetBounds().min.x + 0.5f, PolygonMesh.Instance.GetBounds().max.x - 0.5f);
                        float randomY = Random.Range(PolygonMesh.Instance.GetBounds().min.y + 0.5f, PolygonMesh.Instance.GetBounds().max.y - 0.5f);
                        PlayerController.Instance.transform.localPosition = new Vector3(randomX, randomY, 0);

                    }
                }
                HoldGameFieldBG.Instance.UpdateMesh(PolygonMesh.Instance.verticesList.ToArray());
                Camera.main.transform.position = new Vector3(PolygonMesh.Instance.GetBounds().center.x, PolygonMesh.Instance.GetBounds().center.y, -10);
                yield return new WaitForSeconds(0.01f);
            }
            ++currentLevel;
            HoldGameFieldBG.Instance.SetFieldPassedElement(currentLevel, PolygonMesh.Instance.verticesList);
            //add score
            ScoreManager.Instance.AddScore(1);
            SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
            yield return new WaitForSeconds(0.2f);
            IsScalingBoundary = false;
        }

        public bool IsScalingBoundary
        {
            get { return isScaling; }
            set
            {
                if (value != isScaling)
                {
                    isScaling = value;
                    if (ScalingEvent != null)
                        ScalingEvent(isScaling);
                }
            }
        }
    }
}
