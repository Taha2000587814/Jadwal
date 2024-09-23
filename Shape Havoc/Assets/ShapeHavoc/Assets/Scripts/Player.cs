using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject[] playerPanels;

    [HideInInspector]
    public bool gameIsOver = false;
    [HideInInspector]
    public int ammoCount = 0;


    public void AddPlayerPanel(int index)       //Spawner script calls this function
    {
        if (FindObjectOfType<Spawner>().spawnedObstacles != 0)      //If this is not the first spawn
            Destroy(GameObject.FindGameObjectWithTag("PlayerPanel").gameObject, 0.45f);     //Then destroys the previously spawned playerPanel
        Instantiate(playerPanels[index], transform.position, Quaternion.identity);      //Spawns a new one
    }
}
