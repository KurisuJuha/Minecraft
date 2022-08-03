using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public int[,,] chunk = new int[16, 256, 16];
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public void GenerateMeshData()
    {
        int _x = chunk.GetLength(0);
        int _y = chunk.GetLength(1);
        int _z = chunk.GetLength(2);

        for (int z = 0; z < _z; z++)
        {
            for (int y = 0; y < _y; y++)
            {
                for (int x = 0; x < _x; x++)
                {
                    if (chunk[x,y,z] != 0)
                    {
                        Vector3Int position = new Vector3Int(x, y, z);

                        bool a = !isBlock(new Vector3Int(0, 0, -1), position);
                        bool b = !isBlock(new Vector3Int(1, 0, 0), position);
                        bool c = !isBlock(new Vector3Int(-1, 0, 0), position);
                        bool d = !isBlock(new Vector3Int(0, 1, 0), position);
                        bool e = !isBlock(new Vector3Int(0, -1, 0), position);
                        bool f = !isBlock(new Vector3Int(0, 0, 1), position);

                        SetBox(new Vector3(x, y, z), chunk[x, y, z], a, b, c, d, e, f);
                    }
                }
            }
        }
    }

    private bool isBlock(Vector3Int p, Vector3Int position)
    {
        if (p.x + position.x < 0 
            || p.x + position.x >= chunk.GetLength(0)
            || p.y + position.y < 0
            || p.y + position.y >= chunk.GetLength(1)
            || p.z + position.z < 0 
            || p.z + position.z >= chunk.GetLength(2))
        {
            return false;
        }
        return !TerrainMeshGenerator.Instance.blockData.blocks[chunk[p.x + position.x, p.y + position.y, p.z + position.z]].isTransparentBlock;
    }

    private void SetBox(Vector3 position,int blockid , bool a = true, bool b = true, bool c = true, bool d = true, bool e = true, bool f = true)
    {
        if (a) SetFace(0, 4, 2, 6, position, blockid, 0);
        if (b) SetFace(2, 6, 3, 7, position, blockid, 2);
        if (c) SetFace(1, 5, 0, 4, position, blockid, 3);
        if (d) SetFace(4, 5, 6, 7, position, blockid, 4);

        if (e) SetFace(1, 0, 3, 2, position, blockid, 5);
        if (f) SetFace(3, 7, 1, 5, position, blockid, 1);
    }

    private void SetFace(int a, int b, int c, int d, Vector3 origin, int blockid ,int dir)
    {
        int uvid;
        Block block = TerrainMeshGenerator.Instance.blockData.blocks[blockid];

        switch (dir)
        {
            case 0:
                uvid = block.frontUVid;
                break;
            case 1:
                uvid = block.backUVid;
                break;
            case 2:
                uvid = block.rightUVid;
                break;
            case 3:
                uvid = block.leftUVid;
                break;
            case 4:
                uvid = block.upUVid;
                break;
            case 5:
                uvid = block.downUVid;
                break;
            default:
                uvid = 0;
                break;
        }

        // id‚ð0 ~ 1‚É•ÏŠ·‚·‚é
        float x = (uvid % 16) / 16f;
        float y = (256 - 16 - Mathf.FloorToInt(uvid / 16f)) / 256f;
        float m = 1 / 16f;

        Vector2 _auv = new Vector2(x, y);
        Vector2 _buv = new Vector2(x, y + m);
        Vector2 _cuv = new Vector2(x + m, y);
        Vector2 _duv = new Vector2(x + m, y + m);

        Debug.Log(uvid);
        Debug.Log("a" + _auv);
        Debug.Log("b" + _buv);
        Debug.Log("c" + _cuv);
        Debug.Log("d" + _duv);

        SetFace(Block.vertices[a], Block.vertices[b], Block.vertices[c], Block.vertices[d], origin, _auv, _buv, _cuv, _duv);
    }

    private void SetFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 origin, Vector2 a_uv, Vector2 b_uv, Vector2 c_uv, Vector2 d_uv)
    {
        SetTriangle(a, b, c, origin, a_uv, b_uv, c_uv);
        SetTriangle(b, d, c, origin, b_uv, d_uv, c_uv);
    }

    private void SetTriangle(Vector3 a,Vector3 b, Vector3 c, Vector3 origin, Vector2 a_uv, Vector2 b_uv, Vector2 c_uv)
    {
        vertices.Add(a + origin);
        vertices.Add(b + origin);
        vertices.Add(c + origin);
        triangles.Add(triangles.Count);
        triangles.Add(triangles.Count);
        triangles.Add(triangles.Count);
        uvs.Add(a_uv);
        uvs.Add(b_uv);
        uvs.Add(c_uv);
    }

    public void ToMesh(Mesh mesh)
    {
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
    }
}