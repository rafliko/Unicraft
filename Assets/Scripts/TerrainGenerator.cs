using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform playerBody;
    public GameObject chunkObj;
    public int renderDistance;

    public static Dictionary<ChunkPos, TerrainChunk> chunks = new Dictionary<ChunkPos, TerrainChunk>();

    public static float seed = 10000;
    public const float noiseScale = 0.02f;
    public const int chunkWidth = 16;
    public const int chunkHeight = 64;

    ChunkPos cp;

    int prevpx = 0;
    int prevpz = 0;

    // Start is called before the first frame update
    void Start()
    {
        seed = UnityEngine.Random.Range(Mathf.Pow(10, 4), Mathf.Pow(10,6));
        Debug.Log(seed);

        for(int x=-renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                CreateChunk(x*chunkWidth, z*chunkWidth);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int px = Mathf.FloorToInt(playerBody.transform.position.x / chunkWidth) * chunkWidth;
        int pz = Mathf.FloorToInt(playerBody.transform.position.z / chunkWidth) * chunkWidth;

        if(px > prevpx)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                //load
                cp = new ChunkPos(px + renderDistance * chunkWidth, pz + z * chunkWidth);
                if (chunks.ContainsKey(cp))
                    chunks[cp].gameObject.SetActive(true);
                else
                    CreateChunk(cp.x, cp.z);

                //unload
                cp = new ChunkPos(px - (renderDistance + 1) * chunkWidth, pz + z * chunkWidth);
                chunks[cp].gameObject.SetActive(false);
            }
        }

        if (px < prevpx)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                //load
                cp = new ChunkPos(px - renderDistance * chunkWidth, pz + z * chunkWidth);
                if (chunks.ContainsKey(cp))
                    chunks[cp].gameObject.SetActive(true);
                else
                    CreateChunk(cp.x, cp.z);

                //unload
                cp = new ChunkPos(px + (renderDistance + 1) * chunkWidth, pz + z * chunkWidth);
                chunks[cp].gameObject.SetActive(false);
            }
        }

        if (pz > prevpz)
        {
            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                //load
                cp = new ChunkPos(px + x * chunkWidth, pz + renderDistance * chunkWidth);
                if (chunks.ContainsKey(cp))
                    chunks[cp].gameObject.SetActive(true);
                else
                    CreateChunk(cp.x, cp.z);

                //unload
                cp = new ChunkPos(px + x * chunkWidth, pz - (renderDistance + 1) * chunkWidth);
                chunks[cp].gameObject.SetActive(false);
            }
        }

        if (pz < prevpz)
        {
            for (int x = -renderDistance; x <= renderDistance; x++)
            {
                //load
                cp = new ChunkPos(px + x * chunkWidth, pz - renderDistance * chunkWidth);
                if (chunks.ContainsKey(cp))
                    chunks[cp].gameObject.SetActive(true);
                else
                    CreateChunk(cp.x, cp.z);

                //unload
                cp = new ChunkPos(px + x * chunkWidth, pz + (renderDistance + 1) * chunkWidth);
                chunks[cp].gameObject.SetActive(false);
            }
        }

        prevpx = px;
        prevpz = pz;
    }

    void CreateChunk(int x, int z)
    {
        var obj = Instantiate(chunkObj, new Vector3(x, 0, z), Quaternion.identity).transform.GetChild(0);
        obj.GetComponent<TerrainChunk>().offsetX = x;
        obj.GetComponent<TerrainChunk>().offsetZ = z;
        chunks.Add(new ChunkPos(x, z), obj.GetComponent<TerrainChunk>());
    }
}
