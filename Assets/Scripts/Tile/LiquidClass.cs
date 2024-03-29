using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="LiquidClass",menuName ="Terraria/LiquidClass")]

public class LiquidClass : TileClass
{
    [field:SerializeField]public float flowSpeed { private set; get; }
    public IEnumerator CalculatePhysics(int x,int y)
    {
        yield return new WaitForSeconds(1f / flowSpeed);
        if(y-1>=0&&TerrainManager.Instance.GetTile(x,y-1,(int)Layers.Ground)==null&&TerrainManager.Instance.GetTile(x,y-1,(int)Layers.Liquid)==null)
        {
            TerrainManager.Instance.PlaceTile(x, y - 1, this);
        }
        else
        {
            if (x - 1 >= 0 && TerrainManager.Instance.GetTile(x-1, y , (int)Layers.Ground) == null && TerrainManager.Instance.GetTile(x-1, y, (int)Layers.Liquid) == null)
            {
                TerrainManager.Instance.PlaceTile(x-1, y , this);
            }
            if (x+1>=TerrainManager.Instance.terrainSettings.worldSize.x && TerrainManager.Instance.GetTile(x+1, y, (int)Layers.Ground) == null && TerrainManager.Instance.GetTile(x+1, y, (int)Layers.Liquid) == null)
            {
                TerrainManager.Instance.PlaceTile(x+1,y, this);
            }
        }
    }
}
