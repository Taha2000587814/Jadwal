using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScale : MonoBehaviour
{
    public float speed = 0.5f, computerSpeed;

    private Vector3 startScale, endScale, tempScale;

    private Touch initTouch = new Touch();
    private bool touching = false;

    void Start()
    {
        tempScale = startScale = transform.localScale;
        endScale = new Vector3(startScale.y, startScale.x, startScale.z);

        Debug.Log(startScale);
        Debug.Log(endScale);

    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            float initPos = initTouch.position.y;

            if (Input.GetTouch(0).phase == TouchPhase.Began)        //if finger touches the screen
            {
                if (touching == false)
                {
                    touching = true;
                    initTouch = Input.GetTouch(0);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)       //if finger moves while touching the screen
            {
                float deltaY = initPos - Input.GetTouch(0).position.y;


                tempScale.x -= deltaY * -speed * Time.deltaTime;
                tempScale.y += deltaY * -speed * Time.deltaTime;

                tempScale.x = Mathf.Clamp(tempScale.x, endScale.x, startScale.x);
                tempScale.y = Mathf.Clamp(tempScale.y, startScale.y, endScale.y);

                transform.localScale = tempScale;

                initTouch = Input.GetTouch(0);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)       //if finger releases the screen
            {
                initTouch = new Touch();
                touching = false;
            }
        }

        //if you play on computer---------------------------------
        if (Input.GetKey(KeyCode.W))
        {
            tempScale.x -= computerSpeed * Time.deltaTime;
            tempScale.y += computerSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            tempScale.x += computerSpeed * Time.deltaTime;
            tempScale.y -= computerSpeed * Time.deltaTime;
        }

        tempScale.x = Mathf.Clamp(tempScale.x, endScale.x, startScale.x);
        tempScale.y = Mathf.Clamp(tempScale.y, startScale.y, endScale.y);

        transform.localScale = tempScale;
        //--------------------------------------------------------
    }
}