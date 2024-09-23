using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolderTransparent : MonoBehaviour
{
    private Transform playerHolderTransform;

    void Start()
    {
        playerHolderTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();       //Initializes playerHolderTransform
    }

    void Update()
    {
        transform.localScale = playerHolderTransform.localScale;        //The scale of this gameObject is equal to the scale of playerHolder gameObject
    }
}
