using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshGenerator : SingletonMonoBehaviour<TerrainMeshGenerator>
{
    public BlockData blockData;
    public GameObject ChunkPrefab;
    public Dictionary<Vector2Int, Chunk> map = new Dictionary<Vector2Int, Chunk>();
    public Dictionary<Vector2Int, GameObject> ChunkObject = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject obj = Instantiate(ChunkPrefab, new Vector3(pos.x * 16, 0, pos.y * 16), Quaternion.identity);
                obj.name = pos.ToString();
                Chunk chunk = new Chunk();
                Mesh mesh = new Mesh();

                map[pos] = chunk;
                ChunkObject[pos] = obj;


                for (int _y = 0; _y < 16; _y++)
                {
                    for (int _x = 0; _x < 16; _x++)
                    {
                        int h = (int)(Mathf.PerlinNoise((_x + pos.x * 16) * 0.01f, (_y + pos.y * 16) * 0.01f) * 30 + 1);
                        for (int i = 0; i < h; i++)
                        {
                            chunk.chunk[_x, i, _y] = 1;
                        }
                    }
                }

                chunk.GenerateMeshData();
                chunk.ToMesh(mesh);

                MeshFilter filter = obj.GetComponent<MeshFilter>();
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                MeshCollider collider = obj.GetComponent<MeshCollider>();

                filter.sharedMesh = mesh;
                collider.sharedMesh = mesh;
            }
        }
    }

    void Update()
    {
        
    }
}
