using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public class HoldGameFieldBG : MonoBehaviour
    {

        public static HoldGameFieldBG Instance { get; private set; }

        [SerializeField]
        private Material oldBGMaterial;

        public Dictionary<int, List<Vector2>> fieldsPassedList = new Dictionary<int, List<Vector2>>();

        [HideInInspector]
        public Bounds boundInit = new Bounds();

        MeshFilter filter;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
            filter = GetComponent<MeshFilter>();
        }

        public void UpdateMesh(Vector2[] vertices2D)
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

            filter.mesh.Clear();
            filter.mesh = msh;
            //		oldBGMaterial.color = new Color (80, 80, 80, 200);
        }

        public void ClearMesh()
        {
            filter.mesh.Clear();
        }

        public Bounds GetBounds()
        {
            return filter.mesh.bounds;
        }


        #region GET/SET ELEMENT OF FIELD PASSED DICTIONARY BY INDEX
        public KeyValuePair<int, List<Vector2>> GetPassedMeshByIndex(int index)
        {
            int i = 0;
            if (fieldsPassedList.Count < 0)
                return new KeyValuePair<int, List<Vector2>>();
            foreach (KeyValuePair<int, List<Vector2>> itemField in fieldsPassedList)
            {
                if (i == index)
                    return itemField;
                i++;
            }
            return new KeyValuePair<int, List<Vector2>>();
        }

        public int GetKeyFieldPassedListByIndex(int index)
        {
            return GetPassedMeshByIndex(index).Key;
        }

        public List<Vector2> GetValueFieldPassedListByIndex(int index)
        {
            return GetPassedMeshByIndex(index).Value;
        }

        /// <summary>
        /// Setup current level and vertices list set for it
        /// </summary>
        /// <param name="key">levelKey. Current level</param>
        /// <param name="value">verticesList. Vertices list of mesh</param>
        public void SetFieldPassedElement(int levelKey, List<Vector2> verticesList)
        {
            if (fieldsPassedList.ContainsKey(levelKey))
            {
                fieldsPassedList[levelKey] = verticesList;
            }
            else {
                fieldsPassedList.Add(levelKey, verticesList);
            }
        }
        #endregion
    }
}
