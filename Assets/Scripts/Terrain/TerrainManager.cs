using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainManager : SingleTon<TerrainManager>
{
    public TerrainSettings terrainSettings;
    public TileClass[,,] tileDatas;
    public Tilemap[] tileMaps;
    public TileAtlas tileAtlas;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        terrainSettings.Init();
        terrainSettings.InitCave();
        tileDatas = new TileClass[terrainSettings.worldSize.x, terrainSettings.worldSize.y, 4];
        Generate();
    }
    public void Generate()
    {
        //Ground
        for (int x = 0; x < terrainSettings.worldSize.x; x++)
        {
            int height = terrainSettings.GetHeight(x);
            for (int y = 0; y < height; y++)
            {
                TileClass tileToPlace;
                if(y>height-Random.Range(3,5))
                {
                    tileToPlace = tileAtlas.grassTile;
                }
                else if(y>height-30)
                {
                    tileToPlace = tileAtlas.dirtTile;
                }
                else
                {
                    tileToPlace = tileAtlas.stoneTile;
                }
                foreach (var ore in terrainSettings.ores)
                {
                    if(Mathf.PerlinNoise((x+ore.offset)*ore.oreFrequency,(y+ore.offset)*ore.oreFrequency)<ore.oreRadius)
                    {
                        tileToPlace = ore;
                        break;
                    }
                }
                if(!terrainSettings.cavePoints[x,y])
                {
                    PlaceTile(x, y, tileToPlace);
                }
                if(y>height-40)
                {
                    PlaceTile(x, y, tileAtlas.dirtWall);
                }
                else
                {
                    PlaceTile(x, y, tileAtlas.stoneWall);
                }
            }
        }
        //Plant
        for (int x = 0; x < terrainSettings.worldSize.x; x++)
        {
            int height = terrainSettings.GetHeight(x);
            for(int y=0;y<height;y++)
            {
                if (y == height - 1 &&tileDatas[x,y,(int)Layers.Ground]==tileAtlas.grassTile)
                {
                    if (Mathf.PerlinNoise((x + terrainSettings.seed) * terrainSettings.plantFrequency, (y + terrainSettings.seed) * terrainSettings.plantFrequency) > terrainSettings.plantThreshold)
                    {
                        PlaceTile(x, y, tileAtlas.plants);
                    }
                    else if (Mathf.PerlinNoise((x + terrainSettings.seed) * terrainSettings.treeFrequency, (y + terrainSettings.seed) * terrainSettings.treeFrequency) > terrainSettings.treeThreshold)
                    {
                        if(tileDatas[x-1,terrainSettings.GetHeight(x-1),(int)Layers.Addons]!=tileAtlas.tree)
                        {
                            SpawnTree(x, y+1);
                        }
                    }
                }
            }
        }
        //light
        LightHandler.Instance.Init();
    }
    public TileClass GetTile(int x, int y, int layer)
    {
        if (x < 0 || x >= terrainSettings.worldSize.x) return null;
        if (y < 0 || y >= terrainSettings.worldSize.y) return null;
        return tileDatas[x, y, layer];
    }
    public void PlaceTile(int x,int y,TileClass tileClass)
    {
        if (x < 0 || x >= terrainSettings.worldSize.x || y < 0 || y >= terrainSettings.worldSize.y) return;
        int layer = (int)tileClass.layer;
        tileMaps[layer].SetTile(new Vector3Int(x, y, 0), tileClass.tile);
        tileDatas[x, y, layer] = tileClass;
        if(tileClass is LiquidClass)
        {
            StartCoroutine(((LiquidClass)tileClass).CalculatePhysics(x, y));
        }
        if(tileClass.isIlluminated)
        {
            LightHandler.Instance.LightUpdate(x, y);
        }
    }
    public void SpawnTree(int x,int y)
    {
        if (x < 0 || x >= terrainSettings.worldSize.x || y < 0 || y >= terrainSettings.worldSize.y) return;
        int h = Random.Range(8, 15);
        int maxBranch = Random.Range(3, 10);
        int bCounts = 0;
        for (int ny = y; ny < y+h; ny++)
        {
            PlaceTile(x, ny, tileAtlas.tree);
            if(ny==y)
            {
                if(Random.Range(0,100)<30)
                {
                    if(x>0&&tileDatas[x-1,ny-1,(int)Layers.Ground]!=null&& tileDatas[x - 1, ny , (int)Layers.Ground] == null)
                    {
                        PlaceTile(x - 1, ny, tileAtlas.tree);
                    }
                }
                if (Random.Range(0, 100) < 30)
                {
                    if (x <terrainSettings.worldSize.x-1 && tileDatas[x + 1, ny - 1, (int)Layers.Ground] != null && tileDatas[x + 1, ny, (int)Layers.Ground] == null)
                    {
                        PlaceTile(x + 1, ny, tileAtlas.tree);
                    }
                }
            }
            else if(ny>=y+2&&ny<=y+h-3)
            {
                if(bCounts<maxBranch&&Random.Range(0,100)<40)
                {
                    if (x > 0 && tileDatas[x - 1, ny, (int)Layers.Ground] == null&&tileDatas[x-1,ny-1,(int)Layers.Addons]!=tileAtlas.tree)
                    {
                        PlaceTile(x - 1, ny, tileAtlas.tree);
                        bCounts++;
                    }
                }
                if (bCounts < maxBranch && Random.Range(0, 100) < 40)
                {
                    if (x <terrainSettings.worldSize.x-1 && tileDatas[x + 1, ny, (int)Layers.Ground] == null && tileDatas[x + 1, ny-1, (int)Layers.Addons] != tileAtlas.tree)
                    {
                        PlaceTile(x +1, ny, tileAtlas.tree);
                        bCounts++;
                    }
                }
            }
        }
    }
    public void Erase(int x,int y,int layer)
    {
        if (x < 0 || x >= terrainSettings.worldSize.x || y < 0 || y >= terrainSettings.worldSize.y) return;
        tileMaps[layer].SetTile(new Vector3Int(x, y, 0), null);
        if(tileDatas[x, y, layer].isIlluminated)
        {
            tileDatas[x, y, layer] = null;
            LightHandler.Instance.LightUpdate(x, y);
        }
        tileDatas[x, y, layer] = null;
    }
    public float GetLightValue(int x,int y)
    {
        float lightValue = 0;
        for (int i = 0; i < tileDatas.GetLength(2); i++)
        {
            if (tileDatas[x, y, i] == null) continue;
            if (tileDatas[x, y, i].lightLevel > lightValue) lightValue = tileDatas[x, y, i].lightLevel;
        }
        return lightValue;
    }
}
