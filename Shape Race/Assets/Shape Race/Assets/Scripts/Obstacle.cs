using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public Mesh[] meshes;

    private MeshFilter obstMesh;
    private int randomIndex;

	void Start () {
        obstMesh = GetComponent<MeshFilter>();      //Initializes mesh
        randomIndex = Random.Range(0, meshes.Length);       //Selects a random mesh from the array

        obstMesh.mesh = meshes[randomIndex];        //Changes mesh

        if (obstMesh.mesh.name == "Cube Instance")      //If this is cube then it will be rotated on the X axis by 90 degrees
            transform.Rotate(Vector3.right, 90f);
    }
}
