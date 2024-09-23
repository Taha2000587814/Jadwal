using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkinChange : MonoBehaviour {

    public Mesh[] enemyMeshes;

    private Color[] colors = new Color[] { Color.magenta, Color.cyan, Color.green, Color.blue, Color.white, Color.gray, Color.black };      //You can add new colors here
    private MeshFilter enemyMesh;
    private ParticleSystem enemyParticle;

	void Start () {
        //Inizialization
        GetComponent<ParticleSystemRenderer>().material.color = GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Length)];       //Selects a random color fo the enemy
        enemyMesh = GetComponent<MeshFilter>();
        enemyParticle = GetComponent<ParticleSystem>();

        if (Random.Range(0, 2) == 0)        //Changes mesh 50% of the time
            ChangeMesh();
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))       //If the enemy collides with an obstacle
        {
            if (other.GetComponent<MeshFilter>().mesh.name == enemyMesh.mesh.name)      //If the obstacle has the same mesh as the gabeObject
            {
                ChangeMesh();       //Then gameObject changes its mesh
                enemyParticle.Play();       //Plays enemyParticle
            }
        }
    }

    public void ChangeMesh()
    {
        //Selects a random mesh from the meshes list until it is a different mesh than the enemy's mesh. Then changes enemy' s mesh to the selected one
        int index;
        do
        {
            index = Random.Range(0, enemyMeshes.Length);
        } while (enemyMesh.mesh.name == enemyMeshes[index].name + " Instance");
        enemyMesh.mesh = enemyMeshes[index];
    }
}
