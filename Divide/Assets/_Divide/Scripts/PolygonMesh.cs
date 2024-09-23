using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public class PolygonMesh : MonoBehaviour
    {

        public static PolygonMesh Instance { get; private set; }

        [SerializeField]
        private Material bgMesh;

        [HideInInspector]
        public List<Vector2> verticesList = new List<Vector2>();

        [Header("Min size of mesh")]
        public float sizeMin;

        [Header("Vertices init for mesh")]
        public Transform[] verticesInitList;

        MeshCollider meshCollider;

        MeshFilter filter;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
            verticesList = new List<Vector2>();
            meshCollider = GetComponent<MeshCollider>();
            filter = GetComponent<MeshFilter>();
        }

        // Use this for initialization
        void Start()
        {
            //create Vector2 vertices
            verticesList = InitVerticeList();
            //use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(verticesList.ToArray());
            int[] indices = tr.Triangulate();

            //Create the vector3 vertices
            Vector3[] vertices = new Vector3[verticesList.Count];
            for (int i = 0; i < verticesList.Count; i++)
            {
                vertices[i] = new Vector3(verticesList[i].x, verticesList[i].y, 0);
            }

            //Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            //Setup gameobject with mesh
            filter.mesh.Clear();
            filter.mesh = msh;
            meshCollider.sharedMesh = msh;
            HoldGameFieldBG.Instance.SetFieldPassedElement(0, verticesList);
            HoldGameFieldBG.Instance.UpdateMesh(verticesList.ToArray());
            HoldGameFieldBG.Instance.boundInit = GetBounds();
        }

        public List<Vector2> InitVerticeList()
        {
            List<Vector2> tempList = new List<Vector2>();
            for (int i = 0; i < verticesInitList.Length; i++)
            {
                tempList.Add(verticesInitList[i].position);
            }
            return tempList;
        }

        public void UpdateMesh(Vector2[] vertices2D)
        {
            verticesList = new List<Vector2>();
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            //Create the mesh
            Mesh msh = new Mesh();
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices2D.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
                verticesList.Add(vertices[i]);
            }
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            filter.mesh.Clear();
            filter.mesh = msh;
            meshCollider.sharedMesh = msh;
        }

        public void ClearMesh()
        {
            filter.mesh.Clear();
        }

        public Bounds GetBounds()
        {
            return filter.mesh.bounds;
        }

        public void ScalePolygon(Vector2 amount)
        {
            Bounds meshBounds = filter.mesh.bounds;
            meshBounds.Expand(amount);
            filter.mesh.RecalculateBounds();
        }
    }
}
