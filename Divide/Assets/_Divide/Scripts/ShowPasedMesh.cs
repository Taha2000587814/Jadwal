using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public class ShowPasedMesh : MonoBehaviour
    {

        public static ShowPasedMesh Instance { get; private set; }

        public GameObject meshElement;

        GameObject tempObject;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void ProcessShowMeshsList()
        {

            Debug.Log("Count list: " + HoldGameFieldBG.Instance.fieldsPassedList.Count);
            for (int i = 0; i < HoldGameFieldBG.Instance.fieldsPassedList.Count; i++)
            {
                GameObject newMeshElement = Instantiate(meshElement);
                DrawMesh(newMeshElement.GetComponent<MeshFilter>(), HoldGameFieldBG.Instance.GetValueFieldPassedListByIndex(i).ToArray());
                newMeshElement.transform.parent = transform;
                newMeshElement.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                newMeshElement.transform.localRotation = Quaternion.identity;
                //			if (tempObject==null) {
                newMeshElement.transform.localPosition = Vector3.zero;
                //				tempObject = newMeshElement;
                //				return;
                //			}
                //			newMeshElement.transform.localPosition = tempObject.transform.localPosition + Vector3.one;
                //			tempObject = newMeshElement;
            }
        }
        void DrawMesh(MeshFilter meshElement, Vector2[] vertices2D)
        {
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();
            //Create the mesh
            Mesh msh = new Mesh();
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices2D.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();
            meshElement.mesh.Clear();
            meshElement.mesh = msh;
        }
    }
}
