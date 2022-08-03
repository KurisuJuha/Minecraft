using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlockData : ScriptableObject
{
    public Texture2D blocksTexture;
    public List<Block> blocks = new List<Block>();
}
