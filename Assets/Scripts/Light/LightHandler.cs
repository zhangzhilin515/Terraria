using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : SingleTon<LightHandler>
{
    public TerrainSettings terrainSettings;
    public TerrainManager terrainManager;
    public float[,] lightValues;
    public readonly float sunLight = 15f;
    public Texture2D lightTex;
    public Material lightMap;
    public bool updating;
    public Queue<Vector2Int> updates = new Queue<Vector2Int>();
    public void Init()
    {
        lightValues = new float[terrainSettings.worldSize.x, terrainSettings.worldSize.y];
        lightTex = new Texture2D(terrainSettings.worldSize.x, terrainSettings.worldSize.y);
        transform.localScale = new Vector3(terrainSettings.worldSize.x, terrainSettings.worldSize.y, 1);
        transform.localPosition = new Vector3(terrainSettings.worldSize.x / 2f, terrainSettings.worldSize.y / 2f, 0);
        lightTex.filterMode = FilterMode.Point;
        lightMap.SetTexture("_LightMap", lightTex);
        InitLight();
    }
    private void Update()
    {
        if (!updating && updates.Count > 0)
        {
            updating = true;
            StartCoroutine(lightUpdate(updates.Dequeue()));
        }
    }
    public void InitLight()
    {
        for (int i = 0; i < terrainSettings.worldSize.x; i++)
        {
            for (int j = 0; j < terrainSettings.worldSize.y; j++)
            {
                float lightValue = 0f;
                if (terrainManager.GetLightValue(i, j) != 0) lightValue = terrainManager.GetLightValue(i, j);
                else if (terrainManager.GetTile(i, j, (int)Layers.BackGround) == null && terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue = sunLight;
                else
                {
                    int nx1 = Mathf.Clamp(i - 1, 0, terrainSettings.worldSize.x-1);
                    int nx2 = Mathf.Clamp(i + 1, 0, terrainSettings.worldSize.x - 1);
                    int ny1 = Mathf.Clamp(j - 1, 0, terrainSettings.worldSize.y - 1);
                    int ny2 = Mathf.Clamp(j + 1, 0, terrainSettings.worldSize.y - 1);
                    lightValue = Mathf.Max(lightValues[i, ny1], lightValues[i, ny2], lightValues[nx1, j], lightValues[nx2, j]);
                    if (terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue -= 1f;
                    else
                    {
                        lightValue -= 2.5f;
                    }
                }
                lightValue = Mathf.Clamp(lightValue, 0f, sunLight);
                lightValues[i, j] = lightValue;
            }
        }
        for (int i = terrainSettings.worldSize.x-1; i >=0; i--)
        {
            for (int j =terrainSettings.worldSize.y-1; j>=0; j--)
            {
                float lightValue = 0f;
                if (terrainManager.GetLightValue(i, j) != 0) lightValue = terrainManager.GetLightValue(i, j);
                else if (terrainManager.GetTile(i, j, (int)Layers.BackGround) == null && terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue = sunLight;
                else
                {
                    int nx1 = Mathf.Clamp(i - 1, 0, terrainSettings.worldSize.x - 1);
                    int nx2 = Mathf.Clamp(i + 1, 0, terrainSettings.worldSize.x - 1);
                    int ny1 = Mathf.Clamp(j - 1, 0, terrainSettings.worldSize.y - 1);
                    int ny2 = Mathf.Clamp(j + 1, 0, terrainSettings.worldSize.y - 1);
                    lightValue = Mathf.Max(lightValues[i, ny1], lightValues[i, ny2], lightValues[nx1, j], lightValues[nx2, j]);
                    if (terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue -= 1f;
                    else
                    {
                        lightValue -= 2.5f;
                    }
                }
                lightValue = Mathf.Clamp(lightValue, 0f, sunLight);
                lightValues[i, j] = lightValue;
            }
        }
        for (int i = 0; i < terrainSettings.worldSize.x; i++)
        {
            for (int j = 0; j < terrainSettings.worldSize.y; j++)
            {
                lightTex.SetPixel(i,j,new Color(0, 0, 0, 1f -lightValues[i,j]/sunLight));
            }
        }
        lightTex.Apply();
    }
    public void LightUpdate(int x,int y)
    {
        updates.Enqueue(new Vector2Int(x, y));
    }
    IEnumerator lightUpdate(Vector2Int pos)
    {
        int px1 = Mathf.Clamp(pos.x - (int)sunLight, 0, terrainSettings.worldSize.x - 1);
        int px2 = Mathf.Clamp(pos.x + (int)sunLight, 0, terrainSettings.worldSize.x - 1);
        int py1 = Mathf.Clamp(pos.y - (int)sunLight, 0, terrainSettings.worldSize.y - 1);
        int py2 = Mathf.Clamp(pos.y + (int)sunLight, 0, terrainSettings.worldSize.y - 1);
        for (int i = px1; i < px2; i++)
        {
            for (int j = py1; j < py2; j++)
            {
                lightValues[i, j] = 0f;
            }
        }
        for (int i = px1; i < px2; i++)
        {
            for (int j = py1; j < py2; j++)
            {
                float lightValue = 0f;
                if (terrainManager.GetLightValue(i, j) != 0) lightValue = terrainManager.GetLightValue(i, j);
                else if (terrainManager.GetTile(i, j, (int)Layers.BackGround) == null && terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue = sunLight;
                else
                {
                    int nx1 = Mathf.Clamp(i - 1, 0, terrainSettings.worldSize.x - 1);
                    int nx2 = Mathf.Clamp(i + 1, 0, terrainSettings.worldSize.x - 1);
                    int ny1 = Mathf.Clamp(j - 1, 0, terrainSettings.worldSize.y - 1);
                    int ny2 = Mathf.Clamp(j + 1, 0, terrainSettings.worldSize.y - 1);
                    lightValue = Mathf.Max(lightValues[i, ny1], lightValues[i, ny2], lightValues[nx1, j], lightValues[nx2, j]);
                    if (terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue -= 1f;
                    else
                    {
                        lightValue -= 2.5f;
                    }
                }
                lightValue = Mathf.Clamp(lightValue, 0f, sunLight);
                lightValues[i, j] = lightValue;
            }
        }
        for (int i = px2; i >= px1; i--)
        {
            for (int j = py2; j >= py1; j--)
            {
                float lightValue = 0f;
                if (terrainManager.GetLightValue(i, j) != 0) lightValue = terrainManager.GetLightValue(i, j);
                else if (terrainManager.GetTile(i, j, (int)Layers.BackGround) == null && terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue = sunLight;
                else
                {
                    int nx1 = Mathf.Clamp(i - 1, 0, terrainSettings.worldSize.x - 1);
                    int nx2 = Mathf.Clamp(i + 1, 0, terrainSettings.worldSize.x - 1);
                    int ny1 = Mathf.Clamp(j - 1, 0, terrainSettings.worldSize.y - 1);
                    int ny2 = Mathf.Clamp(j + 1, 0, terrainSettings.worldSize.y - 1);
                    lightValue = Mathf.Max(lightValues[i, ny1], lightValues[i, ny2], lightValues[nx1, j], lightValues[nx2, j]);
                    if (terrainManager.GetTile(i, j, (int)Layers.Ground) == null) lightValue -= 1f;
                    else
                    {
                        lightValue -= 2.5f;
                    }
                }
                lightValue = Mathf.Clamp(lightValue, 0f, sunLight);
                lightValues[i, j] = lightValue;
            }
        }
        for (int i = px1; i < px2; i++)
        {
            for (int j = py1; j < py2; j++)
            {
                lightTex.SetPixel(i, j, new Color(0, 0, 0, 1f - lightValues[i, j] / sunLight));
            }
        }
        lightTex.Apply();
        yield return null;
        updating = false;
    }
}
