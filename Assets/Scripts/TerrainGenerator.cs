using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject chunk;
    public int renderDistance;

    public static Dictionary<ChunkPos, TerrainChunk> chunks = new Dictionary<ChunkPos, TerrainChunk>();

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
                var obj = Instantiate(chunk, new Vector3(x*chunkWidth, 0, z*chunkWidth), Quaternion.identity).transform.GetChild(0);
                obj.GetComponent<TerrainChunk>().offsetX = x*chunkWidth;
                obj.GetComponent<TerrainChunk>().offsetZ = z * chunkWidth;
                chunks.Add(new ChunkPos(x*chunkWidth,z*chunkWidth), obj.GetComponent<TerrainChunk>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
