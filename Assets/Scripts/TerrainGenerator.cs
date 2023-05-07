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
                CreateChunk(new ChunkPos(x*chunkWidth, z*chunkWidth));
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
                    CreateChunk(cp);

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
                    CreateChunk(cp);

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
                    CreateChunk(cp);

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
                    CreateChunk(cp);

                //unload
                cp = new ChunkPos(px + x * chunkWidth, pz + (renderDistance + 1) * chunkWidth);
                chunks[cp].gameObject.SetActive(false);
            }
        }

        prevpx = px;
        prevpz = pz;
    }

    void CreateChunk(ChunkPos cp)
    {
        TerrainChunk tc = Instantiate(chunkObj, new Vector3(cp.x, 0, cp.z), Quaternion.identity).transform.GetChild(0).GetComponent<TerrainChunk>();
        GenerateTerrain(tc, cp.x, cp.z);
        tc.UpdateChunk();
        chunks.Add(cp, tc);
    }

    void GenerateTerrain(TerrainChunk chunk, int offsetX, int offsetZ)
    {
        for (int x = 0; x < chunkWidth + 2; x++)
        {
            for (int z = 0; z < chunkWidth + 2; z++)
            {
                float pn = Mathf.PerlinNoise((x + offsetX + seed) * noiseScale, (z + offsetZ + seed) * noiseScale) * chunkHeight / 4 + 10;
                for (int y = 0; y <= (int)pn; y++)
                {
                    if (y < 10) chunk.blocks[x, y, z] = BlockType.Stone;
                    else if (y < 14) chunk.blocks[x, y, z] = BlockType.Dirt;
                    else chunk.blocks[x, y, z] = BlockType.Grass;
                }
            }
        }
    }
}
