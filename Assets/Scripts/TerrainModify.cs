using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModify : MonoBehaviour
{
    RaycastHit hit;

    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, groundMask))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);

                hit.point = hit.point + transform.forward * 0.01f;

                AlterChunk(hit.point, BlockType.Air);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, groundMask) && hit.distance > 1)
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);

                hit.point = hit.point - transform.forward * 0.01f;
                AlterChunk(hit.point, BlockType.Grass);
            }
        }
    }

    void AlterChunk(Vector3 pos, BlockType bt)
    {
        int cw = TerrainGenerator.chunkWidth;

        int ox = Mathf.FloorToInt(pos.x / cw) * cw;
        int oz = Mathf.FloorToInt(pos.z / cw) * cw;

        int x = Mathf.FloorToInt(pos.x) - ox;
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z) - oz;

        if (x == 0) { ox -= cw; x = cw; }
        if (z == 0) { oz -= cw; z = cw; }

        var chunk = TerrainGenerator.chunks[new ChunkPos(ox, oz)];
        Debug.Log(x + "," + y + "," + z);
        chunk.blocks[x, y, z] = bt;
        chunk.UpdateChunk();

        //if edge value update next chunk
        if (z == TerrainGenerator.chunkWidth)
        {
            TerrainGenerator.chunks[new ChunkPos(ox, oz + cw)].blocks[x, y, 0] = bt;
            TerrainGenerator.chunks[new ChunkPos(ox, oz + cw)].UpdateChunk();
        }
        else if (z == 1)
        {
            TerrainGenerator.chunks[new ChunkPos(ox, oz - cw)].blocks[x, y, cw + 1] = bt;
            TerrainGenerator.chunks[new ChunkPos(ox, oz - cw)].UpdateChunk();
        }

        if (x == TerrainGenerator.chunkWidth)
        {
            TerrainGenerator.chunks[new ChunkPos(ox + cw, oz)].blocks[0, y, z] = bt;
            TerrainGenerator.chunks[new ChunkPos(ox + cw, oz)].UpdateChunk();
        }
        else if (x == 1)
        {
            TerrainGenerator.chunks[new ChunkPos(ox - cw, oz)].blocks[cw + 1, y, z] = bt;
            TerrainGenerator.chunks[new ChunkPos(ox - cw, oz)].UpdateChunk();
        }
    }
}
