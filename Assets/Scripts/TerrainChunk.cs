using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();
    int numFaces = 0;

    float seed = TerrainGenerator.seed;
    const float noiseScale = TerrainGenerator.noiseScale;
    const int chunkWidth = TerrainGenerator.chunkWidth;
    const int chunkHeight = TerrainGenerator.chunkHeight;

    public BlockType[,,] blocks = new BlockType[chunkWidth+2, chunkHeight, chunkWidth+2];

    public float offsetX;
    public float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        FillBlocksArray();
        UpdateChunk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FillBlocksArray()
    {
        for (int x = 0; x < chunkWidth + 2; x++)
        {
            for (int z = 0; z < chunkWidth + 2; z++)
            {
                float pn = Mathf.PerlinNoise((x + offsetX + seed) * noiseScale, (z + offsetZ + seed) * noiseScale) * chunkHeight / 4;
                if ((int)pn <= 0) pn = 1;
                for (int y = 0; y < (int)pn; y++)
                {
                    blocks[x, y, z] = BlockType.Grass;
                }
            }
        }
    }

    void ReadBlocksArray()
    {
        for (int x = 1; x < chunkWidth + 1; x++)
        {
            for (int z = 1; z < chunkWidth + 1; z++)
            {
                for (int y = 0; y < chunkHeight; y++)
                {
                    if (blocks[x, y, z] == BlockType.Grass) CreateCube(new Vector3(x, y, z));
                }
            }
        }
    }

    public void UpdateChunk()
    {
        vertices.Clear();
        triangles.Clear();
        uv.Clear();
        numFaces = 0;

        ReadBlocksArray();
        UpdateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void CreateCube(Vector3 pos)
    {
        if ((int)pos.y < chunkHeight-1 && blocks[(int)pos.x,(int)pos.y + 1,(int)pos.z]==BlockType.Air)
            CreateFace(pos, Face.Top);

        if ((int)pos.y > 0 && blocks[(int)pos.x, (int)pos.y - 1, (int)pos.z] == BlockType.Air)
            CreateFace(pos, Face.Bottom);

        if (blocks[(int)pos.x + 1, (int)pos.y, (int)pos.z] == BlockType.Air)
            CreateFace(pos, Face.Right);

        if (blocks[(int)pos.x - 1, (int)pos.y, (int)pos.z] == BlockType.Air)
            CreateFace(pos, Face.Left);

        if (blocks[(int)pos.x, (int)pos.y, (int)pos.z + 1] == BlockType.Air)
            CreateFace(pos, Face.Back);

        if (blocks[(int)pos.x, (int)pos.y, (int)pos.z - 1] == BlockType.Air)
            CreateFace(pos, Face.Front);
    }

    void CreateFace(Vector3 pos, Face f)
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

        uv.AddRange(new Vector2[]
        {
            new Vector2 (0, 0),
            new Vector2 (1, 0),
            new Vector2 (0, 1),
            new Vector2 (1, 1),
        });

        numFaces++;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
    }
}
