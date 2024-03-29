using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="TileAtlas",menuName ="Terraria/TileAtlas")]

public class TileAtlas : ScriptableObject
{
    [field: SerializeField] public TileClass grassTile { get; private set; }
    [field: SerializeField] public TileClass dirtTile { get; private set; }
    [field: SerializeField] public TileClass stoneTile { get; private set; }
    [field: SerializeField] public TileClass dirtWall { get; private set; }
    [field: SerializeField] public TileClass stoneWall { get; private set; }
    [field: SerializeField] public TileClass plants { get; private set; }
    [field: SerializeField] public TileClass tree { get; private set; }
}
