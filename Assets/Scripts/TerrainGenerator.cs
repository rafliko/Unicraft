using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int renderDistance;

    public static float seed = 10000;
    public const float noiseScale = 0.02f;
    public const int chunkWidth = 16;
    public const int chunkHeight = 64;

    // Start is called before the first frame update
    void Start()
    {
        seed = UnityEngine.Random.Range(Mathf.Pow(10, 4), Mathf.Pow(10,6));
        Debug.Log(seed);

        for(int x=-renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Instantiate(chunk, new Vector3(x*chunkWidth, 0, z*chunkWidth), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
