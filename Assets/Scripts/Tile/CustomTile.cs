using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="CustomTile",menuName ="Terraria/CustomTile")]
public class CustomTile : RuleTile<CustomTile.Neighbor>
{
    public TileBase[] specifiedTiles;
    public int blockId;
    public class Neighbor:RuleTile.TilingRule.Neighbor
    {
        public const int Any = 3;
        public const int Specified = 4;
        public const int NotSpecified = 5;
        public const int Air = 6;
    }
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if(neighbor==3)
        {
            return CheckAny(other);
        }
        else if(neighbor==4)
        {
            return CheckSpecified(other);
        }
        else if(neighbor==5)
        {
            return CheckNotSpecified(other);
        }
        else if(neighbor==6)
        {
            return CheckAir(other);
        }
        return base.RuleMatch(neighbor, other);
    }
    private bool CheckAny(TileBase other)
    {
        if (specifiedTiles.Contains(other)||other==this)
        {
            return true;
        }
        return false;
    }
    private bool CheckSpecified(TileBase other)
    {
        if(specifiedTiles.Contains(other))
        {
            return true;
        }
        return false;
    }
    private bool CheckNotSpecified(TileBase other)
    {
        if (specifiedTiles.Contains(other)||other==this)
        {
            return false;
        }
        return true;
    }
    private bool CheckAir(TileBase other)
    {
        if(other==null)
        {
            return true;
        }
        return false;
    }
}
