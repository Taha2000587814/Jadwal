using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Divide
{
    public class DragDirObject : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public static DragDirObject Instance { get; private set; }

        [Header("References Components")]
        public GameObject allowChangeDirObj;
        public SpriteRenderer parentSprite;
        public Transform parent;

        [Header("Click times direction item to hide allow click object")]
        public int clickTimesToHideItem;
        [Header("Drag times on game field to show allow click object")]
        public int dragTimesToShowItem;

        bool isDragging;                            //player is whether dragging change direction item?
        bool isAllowClickToChangeDir;               //Allow player to click to change item's direction
        bool isClickedChangeDirItem;                //is clicked on change direction item

        int countDrag;                              //count drag times
        int countClickAfterDrag;                    //count click on item change direction after player had dragged

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
            countDrag = 0;
            countClickAfterDrag = 0;
        }

        void Start()
        {
            TurnOnChangeDirection();
        }


        /// <summary>
        /// Turn on allow item change direction
        /// </summary>
        public void TurnOnChangeDirection()
        {
            isAllowClickToChangeDir = true;
            allowChangeDirObj.SetActive(true);
            parentSprite.enabled = false;
        }

        /// <summary>
        /// Turn off allow item change direction
        /// </summary>
        public void TurnOffChangeDirection()
        {
            isAllowClickToChangeDir = false;
            allowChangeDirObj.SetActive(false);
            parentSprite.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isAllowClickToChangeDir && !BoundaryControl.Instance.IsScalingBoundary)
            {
                if (!isDragging)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.tick);
                    RaycastHit2D hit;
                    Vector2 worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    hit = Physics2D.Raycast(worldTouch, Vector2.zero, 1);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.tag == "DirectionItem")
                        {
                            RotateDirectionItemHandler();
                            isClickedChangeDirItem = true;
                        }
                    }
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.startDrag);
            if (!isDragging && DrawLines.Instance.canEnterToDirItem && !BoundaryControl.Instance.IsScalingBoundary)
            {
                DrawLines.Instance.DestroyLinesRender();
                RaycastHit2D hit;
                Vector2 worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(worldTouch, Vector2.zero, 1);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "DirectionItem")
                    {
                        isDragging = true;
                        transform.parent = null;
                    }
                }
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging && !BoundaryControl.Instance.IsScalingBoundary)
            {
                Vector2 worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(worldTouch.x, worldTouch.y + 0.5f, 0);

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.endDrag);
            if (isDragging && !BoundaryControl.Instance.IsScalingBoundary)
            {
                isDragging = false;
                if (isClickedChangeDirItem)
                {
                    isClickedChangeDirItem = false;
                    countClickAfterDrag++;
                }
                RaycastHit hit;
                Ray worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //		hit = Physics. (worldTouch, Vector2.zero, 1);
                if (Physics.Raycast(worldRay, out hit, 100))
                {
                    if (hit.collider.gameObject.tag == "GameField")
                    {
                        DrawLines.Instance.ShootRays();
                        if (countClickAfterDrag >= clickTimesToHideItem)
                        {
                            countClickAfterDrag = 0;
                            TurnOffChangeDirection();
                        }
                        if (!isAllowClickToChangeDir)
                        {
                            countDrag++;
                            if (countDrag > dragTimesToShowItem)
                            {
                                countDrag = 0;
                                TurnOnChangeDirection();
                            }
                        }
                        return;
                    }
                }
                if (transform.parent == null)
                {
                    DrawLines.Instance.DestroyLinesRender();
                    transform.parent = parent;
                    transform.localPosition = Vector3.zero;
                }
            }

        }

        /// <summary>
        /// Process change direction item when player click on it
        /// </summary>
        public void RotateDirectionItemHandler()
        {
            if (DrawLines.Instance.canEnterToDirItem)
            {
                switch (DecideDirection.Instance.chooseDirection)
                {
                    case CoordinateDirection.North_East:
                        DecideDirection.Instance.SetDirectionByInputValue(CoordinateDirection.East_South);
                        break;
                    case CoordinateDirection.East_South:
                        DecideDirection.Instance.SetDirectionByInputValue(CoordinateDirection.South_West);
                        break;
                    case CoordinateDirection.South_West:
                        DecideDirection.Instance.SetDirectionByInputValue(CoordinateDirection.West_North);
                        break;
                    case CoordinateDirection.West_North:
                        DecideDirection.Instance.SetDirectionByInputValue(CoordinateDirection.North_East);
                        break;
                    case CoordinateDirection.North_South:
                        DecideDirection.Instance.SetDirectionByInputValue(CoordinateDirection.East_West);
                        break;
                    case CoordinateDirection.East_West:
                        DecideDirection.Instance.SetDirectionByInputValue(CoordinateDirection.North_South);
                        break;
                }
            }
        }
    }
}
