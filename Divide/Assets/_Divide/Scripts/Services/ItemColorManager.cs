using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divide
{
    public class ItemColorManager : MonoBehaviour
    {

        public static ItemColorManager Instance { get; private set; }


        [Header("The color list of item ")]
        public Color[] itemColorList;

        void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnValidate()
        {
            for (int i = 0; i < itemColorList.Length; i++)
            {
                if (itemColorList[i].a == 0)
                    itemColorList[i].a = 255;
            }
            for (int i = 0; i < itemColorList.Length; i++)
            {
                for (int j = i + 1; j < itemColorList.Length; j++)
                {
                    if (itemColorList[i].r == itemColorList[j].r && itemColorList[i].g == itemColorList[j].g && itemColorList[i].b == itemColorList[j].b)
                    {
                        while (true)
                        {
                            itemColorList[j] = new Color(Random.value, Random.value, Random.value, 255);
                            if (itemColorList[i].r == itemColorList[j].r && itemColorList[i].g == itemColorList[j].g && itemColorList[i].b == itemColorList[j].b)
                                continue;
                            else
                                break;
                        }
                    }
                }
            }
        }
    }
}
