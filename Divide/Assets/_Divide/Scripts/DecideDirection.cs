using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public enum CoordinateDirection
    {
        North_South,
        East_West,
        North_East,
        East_South,
        South_West,
        West_North
    }

    public class DecideDirection : MonoBehaviour
    {

        public static DecideDirection Instance { get; private set; }

        [Header("Transform references")]
        public Transform lineOneCentrePos;
        public Transform lineTwoCentrePos;

        [Header("Choose Direction for Line 1 and Line 2")]
        [Tooltip("First word is direction of line 1.\nSecond word after underline char is direction of line 2")]
        public CoordinateDirection chooseDirection;

        float rotateValue = 90f;            //The value about alpha angle of change item direction

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
        }

        /// <summary>
        /// Set random direction item
        /// </summary>
        public void SetRandomDirection()
        {
            int randomIndex = Random.Range(0, System.Enum.GetValues(typeof(CoordinateDirection)).Length);
            //		int randomIndex=1; //test
            chooseDirection = (CoordinateDirection)randomIndex;
            BuildDirection();
            GetComponent<DrawLines>().SetDirectionForHeadLines(chooseDirection);
        }

        /// <summary>
        /// Set new direction for item whenever player clicked on it
        /// </summary>
        /// <param name="coordinateDir">Coordinate dir.</param>
        public void SetDirectionByInputValue(CoordinateDirection coordinateDir)
        {
            chooseDirection = coordinateDir;
            BuildDirection();
            GetComponent<DrawLines>().SetDirectionForHeadLines(chooseDirection);
        }

        /// <summary>
        /// Decide direction of two lines
        /// </summary>
        void BuildDirection()
        {
            switch (chooseDirection)
            {
                case CoordinateDirection.North_South:
                    lineOneCentrePos.transform.localEulerAngles = new Vector3(0, 0, -rotateValue);
                    lineTwoCentrePos.transform.localEulerAngles = new Vector3(0, 0, -rotateValue);
                    break;
                case CoordinateDirection.East_West:
                    lineOneCentrePos.transform.localEulerAngles = new Vector3(0, 0, -(rotateValue * 2));
                    lineTwoCentrePos.transform.localEulerAngles = new Vector3(0, 0, -(rotateValue * 2));
                    break;
                case CoordinateDirection.North_East:
                    lineOneCentrePos.transform.localEulerAngles = new Vector3(0, 0, -rotateValue);
                    lineTwoCentrePos.transform.localEulerAngles = Vector3.zero;
                    break;
                case CoordinateDirection.East_South:
                    lineOneCentrePos.transform.localEulerAngles = new Vector3(0, 0, -(rotateValue * 2));
                    lineTwoCentrePos.transform.localEulerAngles = new Vector3(0, 0, -rotateValue);
                    break;
                case CoordinateDirection.South_West:
                    lineOneCentrePos.transform.localEulerAngles = new Vector3(0, 0, rotateValue);
                    lineTwoCentrePos.transform.localEulerAngles = new Vector3(0, 0, rotateValue * 2);
                    break;
                case CoordinateDirection.West_North:
                    lineOneCentrePos.transform.localEulerAngles = Vector3.zero;
                    lineTwoCentrePos.transform.localEulerAngles = new Vector3(0, 0, rotateValue);
                    break;
            }
        }

    }
}
