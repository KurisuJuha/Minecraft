using UnityEngine;
using System;

[Serializable]
public class Block
{
    public string Name;
    public bool isTransparentBlock;
    [Header("UV id")]
    public int frontUVid;
    public int backUVid;
    public int rightUVid;
    public int leftUVid;
    public int upUVid;
    public int downUVid;

    public static Vector3[] vertices = new Vector3[8]
    {
        new Vector3(0,0,0),
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(1,0,1),
        new Vector3(0,1,0),
        new Vector3(0,1,1),
        new Vector3(1,1,0),
        new Vector3(1,1,1),
    };
}