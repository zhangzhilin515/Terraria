using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="TerrainSettings",menuName ="Terraria/TerrainSettings")]
public class TerrainSettings : ScriptableObject
{
    [field:SerializeField]public int seed { get; private set; }
    [field: SerializeField]public Vector2Int chunkSize{ get; private set; }
    [field: SerializeField] public int chunkScale { get; private set; }
    [HideInInspector] public Vector2Int worldSize{ get; private set; }
    [field: SerializeField] public float height{ get; private set; }
    [field: SerializeField] public int heightMulti { get; private set; }
    [field: SerializeField,Range(0,1)] public float heightScale { get; private set; }
    [field: SerializeField, Range(0, 1)] public float caveThreshold { get; private set; }
    [field: SerializeField, Range(0, 1)] public float caveScale { get; private set; }
    [field: SerializeField] public  bool[,] cavePoints { get; private set; }
    [field:SerializeField]public OreClass[] ores { get; private set; }
    [field: SerializeField, Range(0, 1)] public float plantThreshold { get; private set; }
    [field: SerializeField, Range(0, 1)] public float plantFrequency { get; private set; }
    [field: SerializeField, Range(0, 1)] public float treeThreshold { get; private set; }
    [field: SerializeField, Range(0, 1)] public float treeFrequency{ get; private set; }
    public void Init()
    {
        if(seed==0)
        {
            seed = Random.Range(-10000, 10000);
        }
        Random.InitState(seed);
        worldSize = chunkSize * chunkScale;
        cavePoints = new bool[worldSize.x, worldSize.y];
    }
    public void InitCave()
    {
        for (int x = 0; x < worldSize.x; x++)
        {
            int height = GetHeight(x);
            for (int y = 0; y < height; y++)
            {
                float p =(float)y / height;
                float v = Mathf.PerlinNoise((x + seed) * caveScale, (y + seed) * caveScale);
                v /= 0.5f + p;
                cavePoints[x, y] = v >= caveThreshold;
            }
        }
    }
    public int GetHeight(int x)
    {
        return (int)(height + Mathf.PerlinNoise((x + seed)*heightScale, seed*heightScale) * heightMulti);
    }
}
