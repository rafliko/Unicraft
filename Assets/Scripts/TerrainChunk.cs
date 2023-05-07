using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();
    int numFaces = 0;

    const int chunkWidth = TerrainGenerator.chunkWidth;
    const int chunkHeight = TerrainGenerator.chunkHeight;

    public BlockType[,,] blocks = new BlockType[chunkWidth+2, chunkHeight, chunkWidth+2];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChunk()
    {
        vertices.Clear();
        triangles.Clear();
        uv.Clear();
        numFaces = 0;

        CreateCubes();
        UpdateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void CreateCubes()
    {
        for (int x = 1; x < chunkWidth + 1; x++)
        {
            for (int z = 1; z < chunkWidth + 1; z++)
            {
                for (int y = 0; y < chunkHeight; y++)
                {
                    if (blocks[x, y, z] != BlockType.Air) CreateCube(new Vector3(x, y, z), blocks[x, y, z]);
                }
            }
        }
    }

    void CreateCube(Vector3 pos, BlockType block)
    {
        Vector2[] uvpos = GetBlockUV(block);

        if ((int)pos.y < chunkHeight-1 && blocks[(int)pos.x,(int)pos.y + 1,(int)pos.z]==BlockType.Air)
            CreateFace(pos, Face.Top, uvpos);

        if ((int)pos.y > 0 && blocks[(int)pos.x, (int)pos.y - 1, (int)pos.z] == BlockType.Air)
            CreateFace(pos, Face.Bottom, uvpos);

        if (blocks[(int)pos.x + 1, (int)pos.y, (int)pos.z] == BlockType.Air)
            CreateFace(pos, Face.Right, uvpos);

        if (blocks[(int)pos.x - 1, (int)pos.y, (int)pos.z] == BlockType.Air)
            CreateFace(pos, Face.Left, uvpos);

        if (blocks[(int)pos.x, (int)pos.y, (int)pos.z + 1] == BlockType.Air)
            CreateFace(pos, Face.Back, uvpos);

        if (blocks[(int)pos.x, (int)pos.y, (int)pos.z - 1] == BlockType.Air)
            CreateFace(pos, Face.Front, uvpos);
    }

    Vector2[] GetBlockUV(BlockType block)
    {
        Vector2[] ret = new Vector2[4];

        switch(block)
        {
            case BlockType.Grass:
                ret = new Vector2[]
                {
                    new Vector2 (0f, 1f), //top-left
                    new Vector2 (0.4f, 1f), //top-right
                    new Vector2 (0f, 0.6f), //bottom-left
                    new Vector2 (0.4f, 0.6f), //bottom-right
                };
                break;

            case BlockType.Dirt:
                ret = new Vector2[]
                {
                    new Vector2 (0.5f, 1f),
                    new Vector2 (1f, 1f),
                    new Vector2 (0.5f, 0.5f),
                    new Vector2 (1f, 0.5f),
                };
                break;

            case BlockType.Stone:
                ret = new Vector2[]
                {
                    new Vector2 (0f, 0.5f),
                    new Vector2 (0.5f, 0.5f),
                    new Vector2 (0f, 0f),
                    new Vector2 (0.5f, 0f),
                };
                break;
        }

        return ret;
    }

    void CreateFace(Vector3 pos, Face f, Vector2[] uvpos)
    {
        switch(f)
        {
            case Face.Top:
                vertices.Add(pos + new Vector3(0, 1, 0));
                vertices.Add(pos + new Vector3(0, 1, 1));
                vertices.Add(pos + new Vector3(1, 1, 0));
                vertices.Add(pos + new Vector3(1, 1, 1));
                break;

            case Face.Bottom:
                vertices.Add(pos + new Vector3(1, 0, 1));
                vertices.Add(pos + new Vector3(0, 0, 1));
                vertices.Add(pos + new Vector3(1, 0, 0));
                vertices.Add(pos + new Vector3(0, 0, 0));
                break;

            case Face.Left:
                vertices.Add(pos + new Vector3(0, 1, 1));
                vertices.Add(pos + new Vector3(0, 1, 0));
                vertices.Add(pos + new Vector3(0, 0, 1));
                vertices.Add(pos + new Vector3(0, 0, 0));
                break; 
            
            case Face.Right:
                vertices.Add(pos + new Vector3(1, 0, 0));
                vertices.Add(pos + new Vector3(1, 1, 0));
                vertices.Add(pos + new Vector3(1, 0, 1));
                vertices.Add(pos + new Vector3(1, 1, 1));
                break;

            case Face.Front:
                vertices.Add(pos + new Vector3(0, 0, 0));
                vertices.Add(pos + new Vector3(0, 1, 0));
                vertices.Add(pos + new Vector3(1, 0, 0));
                vertices.Add(pos + new Vector3(1, 1, 0));
                break;

            case Face.Back:
                vertices.Add(pos + new Vector3(1, 1, 1));
                vertices.Add(pos + new Vector3(0, 1, 1));
                vertices.Add(pos + new Vector3(1, 0, 1));
                vertices.Add(pos + new Vector3(0, 0, 1)); 
                break;

            default:
                return;
        }

        triangles.AddRange(new int[]
        {
            numFaces*4+0, numFaces*4+1, numFaces*4+2,
            numFaces*4+3, numFaces*4+2, numFaces*4+1,
        });

        uv.AddRange(uvpos);

        numFaces++;
    }

    void UpdateMesh()
    {
        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
    }
}
