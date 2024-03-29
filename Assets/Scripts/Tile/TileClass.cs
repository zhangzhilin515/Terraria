using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName ="TileClass",menuName ="Terraria/TileClass")]
public class TileClass : ScriptableObject
{
    public TileBase tile;
    public Layers layer;
    public int tileId;
    public bool isIlluminated;
    public float lightLevel;
    public Color lightColor;
}
