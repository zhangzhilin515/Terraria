using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="OreClass",menuName ="Terraria/OreClass")]
public class OreClass : TileClass
{
    [field:SerializeField,Range(0,1)]public float oreRadius { get; private set; }
    [field: SerializeField,Range(0,1)] public float oreFrequency { get; private set; }
    [field: SerializeField] public float offset { get; private set; }
    [field: SerializeField] public int minY { get; private set; }
    [field: SerializeField] public int maxY { get; private set; }
}
