using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public enum SideType
    {
        Top, Bot, Left, Right
    }

    public class DrawLines : MonoBehaviour
    {

        public static DrawLines Instance { get; private set; }


        [Header("Object references")]
        public GameObject lineOne;
        public GameObject lineTwo;

        [Header("Transform references")]
        public Transform headLineOne;
        public Transform headLineTwo;
        public Transform playerTransform;

        [Header("Move Aniamtion Curve")]
        public AnimationCurve moveType;

        [Header("Time process for draw lines")]
        public float timeWaitingShoot = 1.2f;
        public float delayTimeForMove = 0.01f;

        Vector3 tempPosHeadLineOne;                     //Temporary position of the head line one object
        Vector3 tempPosHeadLineTwo;                     //Temporary position of the head line two object

        Vector2 headLineOneDir;                         //The head line one direction when shooting line to link boundary
        Vector2 headLineTwoDir;                         //The head line two direction when shooting line to link boundary

        Vector2 pointCollisionOne;                      //The first point collision between this object with boundary
        Vector2 pointCollisionTwo;                      //The second point collision between this object with boundary

        Vector2 nullVector2 = new Vector3(1000, 1000);      //The null vector to check whether any vector is null

        bool canDraw = true;                                //player can draw or shoot lines ?

        [HideInInspector]
        public Rect rectNeedCheck = new Rect();         //rect technique to check whether player is locate on it?
        bool isRectContainPlayer;                       //Above rect property has got contain player?

        [HideInInspector]
        public bool canEnterToDirItem;

        private CoordinateDirection directionChosen;    //direction of item which player was chose

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
            canEnterToDirItem = true;
        }

        void OnEnable()
        {
            PlayerController.PlayerDied += ProcessPlayerDie;
        }

        void OnDisable()
        {
            PlayerController.PlayerDied -= ProcessPlayerDie;
        }

        void ProcessPlayerDie()
        {
            canDraw = false;
            canEnterToDirItem = false;
        }

        /// <summary>
        /// Set direction init for two lines 
        /// </summary>
        /// <param name="directionWasChoose">Direction was choose.</param>
        public void SetDirectionForHeadLines(CoordinateDirection directionWasChoose)
        {
            directionChosen = directionWasChoose;
            switch (directionWasChoose)
            {
                case CoordinateDirection.North_South:
                    headLineOneDir = Vector2.up;
                    headLineTwoDir = Vector2.down;
                    break;
                case CoordinateDirection.East_West:
                    headLineOneDir = Vector2.right;
                    headLineTwoDir = Vector2.left;
                    break;
                case CoordinateDirection.North_East:
                    headLineOneDir = Vector2.up;
                    headLineTwoDir = Vector2.right;
                    break;
                case CoordinateDirection.East_South:
                    headLineOneDir = Vector2.right;
                    headLineTwoDir = Vector2.down;
                    break;
                case CoordinateDirection.South_West:
                    headLineOneDir = Vector2.down;
                    headLineTwoDir = Vector2.left;
                    break;
                case CoordinateDirection.West_North:
                    headLineOneDir = Vector2.left;
                    headLineTwoDir = Vector2.up;
                    break;
            }
        }

        /// <summary>
        /// Get point collision between transform and boundary follow head line one diretion
        /// </summary>
        /// <returns>The collision point one.</returns>
        Vector2 GetCollisionPointOne()
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, headLineOneDir, Mathf.Infinity, 1 << LayerMask.NameToLayer("Boundary"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Boundary")
                {
                    return hit.point;
                }
            }
            return Vector2.zero;
        }

        /// <summary>
        ///  Get point collision between transform and boundary follow head line two diretion
        /// </summary>
        /// <returns>The collision point two.</returns>
        Vector2 GetCollisionPointTwo()
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, headLineTwoDir, Mathf.Infinity, 1 << LayerMask.NameToLayer("Boundary"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Boundary")
                {
                    return hit.point;
                }
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Get collision point follow by input direction
        /// </summary>
        /// <returns>The collision point by direction.</returns>
        /// <param name="direction">Direction.</param>
        Vector2 GetCollisionPointByDirection(Vector2 direction)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Boundary"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Boundary")
                {
                    return hit.point;
                }
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Shoots the rays from item direction which play was dragged into game field
        /// </summary>
        public void ShootRays()
        {
            pointCollisionOne = GetCollisionPointOne();
            pointCollisionTwo = GetCollisionPointTwo();
            StartCoroutine(CrShootLines());
        }

        /// <summary>
        /// Process shoot lines from item's direction position
        /// </summary>
        /// <returns>The shoot lines.</returns>
        IEnumerator CrShootLines()
        {
            canEnterToDirItem = false;
            float currentTime = 0;
            tempPosHeadLineOne = headLineOne.position;
            tempPosHeadLineTwo = headLineTwo.position;
            if (pointCollisionOne == Vector2.zero || pointCollisionTwo == Vector2.zero)
                yield return null;
            Vector3 posOne = tempPosHeadLineOne;
            Vector3 posTwo = tempPosHeadLineTwo;
            while (currentTime <= timeWaitingShoot)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / timeWaitingShoot);
                tempPosHeadLineOne = Vector3.Lerp(posOne, new Vector3(pointCollisionOne.x, pointCollisionOne.y, 0), moveType.Evaluate(t));
                tempPosHeadLineTwo = Vector3.Lerp(posTwo, new Vector3(pointCollisionTwo.x, pointCollisionTwo.y, 0), moveType.Evaluate(t));
                //setup line one
                if (canDraw)
                {
                    SetupLineRender(lineOne, tempPosHeadLineOne);
                    SetupCollider(lineOne, tempPosHeadLineOne);
                    //setup line two
                    SetupLineRender(lineTwo, tempPosHeadLineTwo);
                    SetupCollider(lineTwo, tempPosHeadLineTwo);
                }
                yield return new WaitForSeconds(delayTimeForMove);
            }
            CalculateForRect();
            CalculateVerticesNeedDelete();
            canEnterToDirItem = true;
            yield return null;
        }

        /// <summary>
        /// Calculate vectices which need delete in game field
        /// </summary>
        void CalculateVerticesNeedDelete()
        {
            if (canDraw)
            {
                //draw completed
                for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                {
                    //Calculate follow two way via check whether current rect is contain player
                    if (isRectContainPlayer)
                    {
                        //Contained player => Hold all vertices in which this rect
                        if (!rectNeedCheck.Contains(PolygonMesh.Instance.verticesList[i]))
                        {
                            PolygonMesh.Instance.verticesList[i] = nullVector2;
                        }
                    }
                    else {
                        //Don't contain player => Delete all vertices in which this rect
                        if (rectNeedCheck.Contains(PolygonMesh.Instance.verticesList[i]))
                        {
                            PolygonMesh.Instance.verticesList[i] = nullVector2;
                        }
                    }
                }
                MakeNewVerticesPolygon();
                isRectContainPlayer = false;
            }
        }

        /// <summary>
        /// Add follow normal way. It mean is vertices list need calculate hasn't got contain vertice 0
        /// </summary>
        /// <param name="posAddFirst">Position add first.</param>
        /// <param name="centerPostion">Center postion.</param>
        /// <param name="posAddSecond">Position add second.</param>
        void AddFollowNormalWay(Vector2 posAddFirst, Vector2 centerPostion, Vector2 posAddSecond)
        {
            bool isAddFirst = false;
            List<Vector2> newVerticesList = new List<Vector2>();
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                {
                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                }
                else {
                    if (!isAddFirst)
                    {
                        newVerticesList.Add(posAddFirst);
                        newVerticesList.Add(centerPostion);
                        newVerticesList.Add(posAddSecond);
                        isAddFirst = true;
                    }
                    continue;
                }
            }
            if (!isAddFirst)
            {
                newVerticesList.Add(posAddFirst);
                newVerticesList.Add(centerPostion);
                newVerticesList.Add(posAddSecond);
            }
            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
        }

        /// <summary>
        /// Add follow special way. It mean is vertices list need calculate has got contain vertice 0
        /// </summary>
        void AddFollowSpecialWay(Vector2 posAddFirst, Vector2 centerPostion, Vector2 posAddSecond)
        {
            List<Vector2> newVerticesList = new List<Vector2>();
            bool isAlreadyAdd = false;
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                {
                    newVerticesList.Add(posAddFirst);
                    for (int j = i; j < PolygonMesh.Instance.verticesList.Count; j++)
                    {
                        if (PolygonMesh.Instance.verticesList[j] != nullVector2)
                        {
                            newVerticesList.Add(PolygonMesh.Instance.verticesList[j]);
                        }
                        else {
                            if (!isAlreadyAdd)
                            {
                                newVerticesList.Add(posAddSecond);
                                newVerticesList.Add(centerPostion);
                                isAlreadyAdd = true;
                            }
                        }
                    }
                    break;
                }
            }
            if (!isAlreadyAdd)
            {
                newVerticesList.Add(posAddSecond);
                newVerticesList.Add(centerPostion);
            }
            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
        }

        /// <summary>
        /// Make new vertices list for polygon and collider boundary
        /// </summary>
        void MakeNewVerticesPolygon()
        {
            List<Vector2> newVerticesList = new List<Vector2>();
            //		Debug.Log ("Point One: " + pointCollisionOne);
            //		Debug.Log ("Point Two: " + pointCollisionTwo);
            //		Debug.Log ("Trans: " + transform.position);
            switch (directionChosen)
            {
                case CoordinateDirection.North_East:
                    //Done
                    if (isRectContainPlayer)
                    {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //Contain player but vertice 0 still != null
                            //					Debug.Log ("Contain: Zero != null");
                            AddFollowNormalWay(pointCollisionTwo, transform.position, pointCollisionOne);
                        }
                        else {
                            //					Debug.Log ("Contain: Zero null");
                            //Contain Player and vertice 0 == null
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                {
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                                }
                            }
                            newVerticesList.Add(pointCollisionTwo);
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionOne);
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    else {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //Not contain player but vertice 0 still != null
                            //					Debug.Log ("Dont Contain: Zero != null");
                            AddFollowNormalWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                        else {
                            //					Debug.Log ("Dont Contain: Zero null");
                            //Not contain Player and vertice 0 == null
                            AddFollowSpecialWay(pointCollisionTwo, transform.position, pointCollisionOne);
                        }
                    }
                    break;
                case CoordinateDirection.East_South:
                    //Doing

                    if (isRectContainPlayer)
                    {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Contain: Zero != null");
                            //Contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionTwo, transform.position, pointCollisionOne);
                        }
                        else {
                            //					Debug.Log ("Contain: Zero null");
                            //Contain Player and vertice 0 == null
                            AddFollowSpecialWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                    }
                    else {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Dont Contain: Zero != null");
                            //Not contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                        else {
                            //					Debug.Log ("Dont Contain: Zero null");
                            newVerticesList.Add(pointCollisionOne);
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionTwo);
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    break;
                case CoordinateDirection.South_West:
                    if (isRectContainPlayer)
                    {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Contain: Zero != null");
                            //Contain player but vertice 0 still != null
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionOne);
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            newVerticesList.Add(pointCollisionTwo);
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                        else {
                            //					Debug.Log ("Contain: Zero null");
                            //Contain Player and vertice 0 == null
                            AddFollowSpecialWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                    }
                    else {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Dont Contain: Zero != null");
                            //Not contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                        else {
                            //					Debug.Log ("Dont Contain: Zero null");
                            newVerticesList.Add(pointCollisionOne);
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionTwo);
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    break;
                case CoordinateDirection.West_North:
                    if (isRectContainPlayer)
                    {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Contain: Zero != null");
                            //Contain player but vertice 0 still != null
                            newVerticesList.Add(pointCollisionTwo);
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionOne);
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                        else {
                            //					Debug.Log ("Contain: Zero null");
                            //Contain Player and vertice 0 == null
                            newVerticesList.Add(pointCollisionTwo);
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionOne);
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    else {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Dont Contain: Zero != null");
                            //Not contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                        else {
                            //					Debug.Log ("Dont Contain: Zero null");
                            //Not contain Player and vertice 0 == null. 
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            newVerticesList.Add(pointCollisionOne);
                            newVerticesList.Add(transform.position);
                            newVerticesList.Add(pointCollisionTwo);
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    break;
                case CoordinateDirection.North_South:
                    if (isRectContainPlayer)
                    {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Contain: Zero != null");
                            //Contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                        else {
                            //					Debug.Log ("Contain: Zero == null");
                            //Contain Player and vertice 0 == null
                            newVerticesList.Add(pointCollisionOne);
                            newVerticesList.Add(pointCollisionTwo);
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    else {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Dont Contain: Zero != null");
                            //Not contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionTwo, transform.position, pointCollisionOne);
                        }
                        else {
                            //					Debug.Log ("Dont Contain: Zero null");
                            //Not contain Player and vertice 0 == null. 
                            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            newVerticesList.Add(pointCollisionTwo);
                            newVerticesList.Add(pointCollisionOne);
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    break;
                case CoordinateDirection.East_West:
                    if (isRectContainPlayer)
                    {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Contain: Zero != null");
                            //Contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionTwo, transform.position, pointCollisionOne);
                        }
                        else {
                            //					Debug.Log ("Contain: Zero null");
                            //Contain Player and vertice 0 == null
                            AddFollowSpecialWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                    }
                    else {
                        if (PolygonMesh.Instance.verticesList[0] != nullVector2)
                        {
                            //					Debug.Log ("Dont Contain: Zero != null");
                            //Not contain player but vertice 0 still != null
                            AddFollowNormalWay(pointCollisionOne, transform.position, pointCollisionTwo);
                        }
                        else {
                            //					Debug.Log ("Dont Contain: Zero null");
                            //Not contain Player and vertice 0 == null. 
                            int indexFinded = 0;
                            for (int i = PolygonMesh.Instance.verticesList.Count - 1; i >= 0; i--)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                {
                                    indexFinded = i;
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                                    break;
                                }
                            }
                            newVerticesList.Add(pointCollisionOne);
                            newVerticesList.Add(pointCollisionTwo);
                            for (int i = 0; i < indexFinded; i++)
                            {
                                if (PolygonMesh.Instance.verticesList[i] != nullVector2)
                                    newVerticesList.Add(PolygonMesh.Instance.verticesList[i]);
                            }
                            BoundaryControl.Instance.UpdatePolygonBoundary(newVerticesList.ToArray());
                        }
                    }
                    break;
            }
        }


        void UpdatePointsCollision()
        {
            //Update for point one 
            Vector2 newPoint = GetNewPoint(headLineOne.position);
            if (newPoint != nullVector2)
            {
                //			Debug.Log ("One: " + pointCollisionOne);
                //			Debug.Log ("<color=yellow>HeadOne Same: </color>" + newPoint);
                pointCollisionOne = newPoint;
                //			Debug.Log ("One after: " + pointCollisionOne);
            }
            //Update for point two
            newPoint = GetNewPoint(headLineTwo.position);
            if (newPoint != nullVector2)
            {
                //			Debug.Log ("Two: " + pointCollisionTwo);
                //			Debug.Log ("<color=green>HeadTwo Same: </color>" + newPoint);
                pointCollisionTwo = newPoint;
                //			Debug.Log ("Two after: " + pointCollisionTwo);
            }
            DestroyTheSamePoint();
        }

        /// <summary>
        /// Calculate the rect which cover corner game field will delete
        /// </summary>
        void CalculateForRect()
        {
            float widthRectCheck, heightRectCheck;
            Vector2 a = new Vector2(0.3f, 0.3f);
            Vector2 worldTouch = new Vector2();
            Vector2 posOneForRectCheck, posTwoForRectCheck;
            Vector2 topWorldTouch, botWorldTouch, rightWorldTouch, leftWorldTouch;
            switch (directionChosen)
            {
                case CoordinateDirection.North_East:
                    //Get top-right position of screen
                    worldTouch = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                    posOneForRectCheck = new Vector2(pointCollisionOne.x, worldTouch.y);
                    posTwoForRectCheck = new Vector2(worldTouch.x, pointCollisionTwo.y);
                    widthRectCheck = Mathf.Abs(Vector2.Distance(posTwoForRectCheck, transform.position));
                    heightRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, transform.position));
                    rectNeedCheck = CreateRectangle(transform.position, widthRectCheck, heightRectCheck);
                    if (rectNeedCheck.Contains(playerTransform.position))
                        isRectContainPlayer = true;
                    else
                        UpdatePointsCollision();
                    break;
                case CoordinateDirection.East_South:
                    //GEt bot-right position of screen
                    worldTouch = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
                    posOneForRectCheck = new Vector2(worldTouch.x, pointCollisionOne.y);
                    posTwoForRectCheck = new Vector2(pointCollisionTwo.x, worldTouch.y);
                    widthRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, transform.position));
                    heightRectCheck = Mathf.Abs(Vector2.Distance(posTwoForRectCheck, transform.position));
                    rectNeedCheck = CreateRectangle(posTwoForRectCheck - a, widthRectCheck, heightRectCheck);
                    if (rectNeedCheck.Contains(playerTransform.position))
                        isRectContainPlayer = true;
                    else
                        UpdatePointsCollision();
                    break;
                case CoordinateDirection.South_West:
                    //Get bot-left position of screen
                    worldTouch = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
                    posOneForRectCheck = new Vector2(pointCollisionOne.x, worldTouch.y);
                    posTwoForRectCheck = new Vector2(worldTouch.x, pointCollisionTwo.y);
                    widthRectCheck = Mathf.Abs(Vector2.Distance(posTwoForRectCheck, transform.position));
                    heightRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, transform.position));
                    rectNeedCheck = CreateRectangle(worldTouch - a, widthRectCheck, heightRectCheck);
                    if (rectNeedCheck.Contains(playerTransform.position))
                        isRectContainPlayer = true;
                    else
                        UpdatePointsCollision();
                    break;
                case CoordinateDirection.West_North:
                    //Get top-left position of screen
                    worldTouch = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
                    posOneForRectCheck = new Vector2(worldTouch.x, pointCollisionOne.y);
                    posTwoForRectCheck = new Vector2(pointCollisionTwo.x, worldTouch.y);
                    widthRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, transform.position));
                    heightRectCheck = Mathf.Abs(Vector2.Distance(posTwoForRectCheck, transform.position));
                    rectNeedCheck = CreateRectangle(posOneForRectCheck - a, widthRectCheck, heightRectCheck);
                    if (rectNeedCheck.Contains(playerTransform.position))
                        isRectContainPlayer = true;
                    else
                        UpdatePointsCollision();
                    break;
                case CoordinateDirection.North_South:
                    //Calculate left rect and check current transfrom of player
                    //Get top-left and bot-left position of Screen
                    topWorldTouch = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
                    botWorldTouch = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
                    posOneForRectCheck = new Vector2(pointCollisionOne.x, topWorldTouch.y);
                    posTwoForRectCheck = new Vector2(pointCollisionTwo.x, botWorldTouch.y);
                    widthRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, topWorldTouch));
                    heightRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, posTwoForRectCheck));
                    rectNeedCheck = CreateRectangle(botWorldTouch - a, widthRectCheck, heightRectCheck);
                    if (rectNeedCheck.Contains(playerTransform.position))
                        isRectContainPlayer = true;
                    else {
                        //Update for point One
                        Vector2 newPoint = GetNewPointOfVerticalStraight(SideType.Top);
                        if (newPoint != nullVector2)
                        {
                            //					Debug.Log ("One: " + pointCollisionOne);
                            //					Debug.Log ("<color=yellow>HeadOne Same: </color>" + newPoint);
                            pointCollisionOne = newPoint;
                            //					Debug.Log ("One after: " + pointCollisionOne);
                        }
                        //Update for point Two
                        newPoint = GetNewPointOfVerticalStraight(SideType.Bot);
                        if (newPoint != nullVector2)
                        {
                            //					Debug.Log ("Two: " + pointCollisionTwo);
                            //					Debug.Log ("<color=green>HeadTwo Same: </color>" + newPoint);
                            pointCollisionTwo = newPoint;
                            //					Debug.Log ("Two after: " + pointCollisionTwo);
                        }
                        DestroyStraightSamePoint(true);
                    }
                    //Need  check  delete
                    break;
                case CoordinateDirection.East_West:
                    //Calculate bot rect and check current transfrom of player
                    //Get bot-left and bot-right position os Screen
                    rightWorldTouch = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
                    leftWorldTouch = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
                    posOneForRectCheck = new Vector2(rightWorldTouch.x, pointCollisionOne.y);
                    posTwoForRectCheck = new Vector2(leftWorldTouch.x, pointCollisionTwo.y);
                    widthRectCheck = Mathf.Abs(Vector2.Distance(posOneForRectCheck, posTwoForRectCheck));
                    heightRectCheck = Mathf.Abs(Vector2.Distance(leftWorldTouch, posTwoForRectCheck));
                    rectNeedCheck = CreateRectangle(leftWorldTouch - a, widthRectCheck, heightRectCheck);
                    if (rectNeedCheck.Contains(playerTransform.position))
                        isRectContainPlayer = true;
                    else {
                        //Update for point One
                        Vector2 newPoint = GetNewPointOfHozirontalStraight(SideType.Right);
                        if (newPoint != nullVector2)
                        {
                            //					Debug.Log ("One: " + pointCollisionOne);
                            //					Debug.Log ("<color=yellow>HeadOne Same: </color>" + newPoint);
                            pointCollisionOne = newPoint;
                            //					Debug.Log ("One after: " + pointCollisionOne);
                        }
                        //Update for point Two
                        newPoint = GetNewPointOfHozirontalStraight(SideType.Left);
                        if (newPoint != nullVector2)
                        {
                            //					Debug.Log ("Two: " + pointCollisionTwo);
                            //					Debug.Log ("<color=green>HeadTwo Same: </color>" + newPoint);
                            pointCollisionTwo = newPoint;
                            //					Debug.Log ("Two after: " + pointCollisionTwo);
                        }
                        DestroyStraightSamePoint(false);
                    }
                    break;
            }
        }

        /// <summary>
        /// Create new rectangle area to check whether old vertices of polygon locate in it
        /// </summary>
        /// <param name="recPoint">Rec point.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        Rect CreateRectangle(Vector2 recPoint, float width, float height)
        {
            Rect newRect = new Rect();
            newRect.x = recPoint.x;
            newRect.y = recPoint.y;
            newRect.width = width + 0.3f;
            newRect.height = height + 0.3f;
            return newRect;
        }

        /// <summary>
        /// Setup collider while item's direction object is shooting lines
        /// </summary>
        /// <param name="objNeedSetup">Object need setup.</param>
        /// <param name="endPos">End position.</param>
        void SetupCollider(GameObject objNeedSetup, Vector2 endPos)
        {
            List<Vector2> pointsList = new List<Vector2>();
            objNeedSetup.GetComponent<EdgeCollider2D>().enabled = true;
            pointsList.Add(objNeedSetup.transform.InverseTransformPoint(transform.position));
            pointsList.Add(objNeedSetup.transform.InverseTransformPoint(new Vector3(endPos.x, endPos.y, 0)));
            objNeedSetup.GetComponent<EdgeCollider2D>().points = pointsList.ToArray();
        }

        /// <summary>
        /// Setup linerender while item's direction object is shooting lines
        /// </summary>
        /// <param name="objContainLine">Object contain line.</param>
        /// <param name="endPos">End position.</param>
        /// <param name="endColor">End color.</param>
        void SetupLineRender(GameObject objContainLine, Vector2 endPos)
        {
            LineRenderer lineRender = objContainLine.GetComponent<LineRenderer>();
            lineRender.positionCount = 2;
            lineRender.startWidth = 0.1f;
            lineRender.endWidth = 0.1f;
            lineRender.SetPosition(0, transform.position);
            lineRender.SetPosition(1, new Vector3(endPos.x, endPos.y, 0f));
            //lineRender.startColor = Color.white;
        }

        /// <summary>
        /// Destroy all lines which player shooted
        /// </summary>
        public void DestroyLinesRender()
        {
            lineOne.GetComponent<LineRenderer>().positionCount = 0;
            lineTwo.GetComponent<LineRenderer>().positionCount = 0;
            tempPosHeadLineOne = headLineOne.position;
            tempPosHeadLineTwo = headLineTwo.position;
            pointCollisionOne = Vector2.zero;
            pointCollisionTwo = Vector2.zero;
            DestroyLinesCollider();
        }

        /// <summary>
        /// Destroy all collider of two lines is shooted
        /// </summary>
        void DestroyLinesCollider()
        {
            lineOne.GetComponent<EdgeCollider2D>().enabled = false;
            lineTwo.GetComponent<EdgeCollider2D>().enabled = false;
        }

        /// <summary>
        /// Get new point for pointCollisionOne whenever the rectContainPlayer is false
        /// </summary>
        /// <returns>The new point.</returns>
        /// <param name="point">Point.</param>
        Vector2 GetNewPoint(Vector2 point)
        {
            List<Vector2> verticesSameCount = new List<Vector2>();
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (Mathf.Approximately(point.x, PolygonMesh.Instance.verticesList[i].x) || Mathf.Approximately(point.y, PolygonMesh.Instance.verticesList[i].y))
                {
                    verticesSameCount.Add(PolygonMesh.Instance.verticesList[i]);
                }
            }
            if (verticesSameCount.Count > 0)
            {
                Vector2 maxVertice = verticesSameCount[0];
                for (int i = 1; i < verticesSameCount.Count; i++)
                {
                    if (Mathf.Abs(Vector2.Distance(point, maxVertice)) < Mathf.Abs(Vector2.Distance(point, verticesSameCount[i])))
                    {
                        maxVertice = verticesSameCount[i];
                    }
                }
                return maxVertice;
            }
            return nullVector2;
        }

        void DestroyTheSamePoint()
        {
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (Mathf.Approximately(headLineOne.position.x, PolygonMesh.Instance.verticesList[i].x) || Mathf.Approximately(headLineOne.position.y, PolygonMesh.Instance.verticesList[i].y))
                {
                    //				Debug.Log ("<color=blue> One rong: </color>" + PolygonMesh.Instance.verticesList [i]);
                    PolygonMesh.Instance.verticesList[i] = nullVector2;
                }
            }
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (Mathf.Approximately(headLineTwo.position.x, PolygonMesh.Instance.verticesList[i].x) || Mathf.Approximately(headLineTwo.position.y, PolygonMesh.Instance.verticesList[i].y))
                {
                    //				Debug.Log ("<color=green> Two rong: </color> " + PolygonMesh.Instance.verticesList [i]);
                    PolygonMesh.Instance.verticesList[i] = nullVector2;
                }
            }
        }


        /// <summary>
        /// Get new point for pointCollisionOne or Two in case straigh lines direction
        /// </summary>
        /// <param name="straightLineType">Straight line type.</param>
        /// <param name="isAbove">If set to <c>true</c> is above.</param>
        Vector2 GetNewPointOfVerticalStraight(SideType sideType)
        {
            List<Vector2> verticesSameCount = new List<Vector2>();
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (sideType == SideType.Top)
                {
                    //If side=top then check vertices which above transform
                    if (PolygonMesh.Instance.verticesList[i].y > transform.position.y)
                    {
                        if (Mathf.Approximately(transform.position.x, PolygonMesh.Instance.verticesList[i].x))
                        {
                            verticesSameCount.Add(PolygonMesh.Instance.verticesList[i]);
                        }
                    }
                }
                else if (sideType == SideType.Bot)
                {
                    //Otherwise, side==bot -> check vertices at bottom of transform position 
                    if (PolygonMesh.Instance.verticesList[i].y < transform.position.y)
                    {
                        if (Mathf.Approximately(transform.position.x, PolygonMesh.Instance.verticesList[i].x))
                        {
                            verticesSameCount.Add(PolygonMesh.Instance.verticesList[i]);
                        }
                    }
                }
            }
            if (verticesSameCount.Count > 0)
            {
                Vector2 maxVertice = verticesSameCount[0];
                for (int i = 1; i < verticesSameCount.Count; i++)
                {
                    if (Mathf.Abs(Vector2.Distance(maxVertice, transform.position)) < Mathf.Abs(Vector2.Distance(verticesSameCount[i], transform.position)))
                    {
                        maxVertice = verticesSameCount[i];
                    }
                }
                return maxVertice;
            }
            return nullVector2;
        }

        Vector2 GetNewPointOfHozirontalStraight(SideType sideType)
        {
            List<Vector2> verticesSameCount = new List<Vector2>();
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (sideType == SideType.Left)
                {
                    //If side == left then check vertices which left transform
                    if (PolygonMesh.Instance.verticesList[i].x < transform.position.x)
                    {
                        //Next, check vertices have got x pos simalar with transform's x axis
                        if (Mathf.Approximately(transform.position.x, PolygonMesh.Instance.verticesList[i].y))
                        {
                            verticesSameCount.Add(PolygonMesh.Instance.verticesList[i]);
                        }
                    }
                }
                else if (sideType == SideType.Right)
                {
                    //Otherwise, side==right -> check vertices at right of transform position 
                    if (PolygonMesh.Instance.verticesList[i].x > transform.position.x)
                    {
                        if (Mathf.Approximately(transform.position.x, PolygonMesh.Instance.verticesList[i].y))
                        {
                            verticesSameCount.Add(PolygonMesh.Instance.verticesList[i]);
                        }
                    }
                }
            }
            if (verticesSameCount.Count > 0)
            {
                Vector2 maxVertice = verticesSameCount[0];
                for (int i = 1; i < verticesSameCount.Count; i++)
                {
                    if (Mathf.Abs(Vector2.Distance(maxVertice, transform.position)) < Mathf.Abs(Vector2.Distance(verticesSameCount[i], transform.position)))
                    {
                        maxVertice = verticesSameCount[i];
                    }
                }
                return maxVertice;
            }
            return nullVector2;
        }

        void DestroyStraightSamePoint(bool verticalOrHorizontal)
        {
            for (int i = 0; i < PolygonMesh.Instance.verticesList.Count; i++)
            {
                if (verticalOrHorizontal)
                {
                    //if param is vertical direction
                    if (Mathf.Approximately(transform.position.x, PolygonMesh.Instance.verticesList[i].x))
                        PolygonMesh.Instance.verticesList[i] = nullVector2;
                }
                else {
                    //If param is horizontal direction
                    if (Mathf.Approximately(transform.position.y, PolygonMesh.Instance.verticesList[i].y))
                        PolygonMesh.Instance.verticesList[i] = nullVector2;
                }
            }
        }


    }
}
