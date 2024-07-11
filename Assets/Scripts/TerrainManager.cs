using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;
    private TerrainData originialTerrainData;
    private int heightmapWidth;
    private int heightmapHeight;

    public Texture2D deformationMask;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();

        originialTerrainData = terrain.terrainData;

        heightmapWidth = originialTerrainData.heightmapResolution;
        heightmapHeight = originialTerrainData.heightmapResolution;

        terrainData = Instantiate(originialTerrainData);
        terrain.terrainData = terrainData;

        InitializeDeformationMask();
    }

    //deform terrain when ball hits it
    public void DeformTerrain(Vector3 position, int areaSize, float power)
    {
        Vector2 terrainPos = WorldToTerrainSpace(position);
        int x = Mathf.FloorToInt(terrainPos.x * heightmapWidth);
        int y = Mathf.FloorToInt(terrainPos.y * heightmapHeight);

        int xStart = Mathf.Clamp(x - areaSize / 2, 0, heightmapWidth - 1);
        int yStart = Mathf.Clamp(y - areaSize / 2, 0, heightmapHeight - 1);
        int xEnd = Mathf.Clamp(x + areaSize / 2, 0, heightmapWidth);
        int yEnd = Mathf.Clamp(y + areaSize / 2, 0, heightmapHeight);

        int width = xEnd - xStart;
        int height = yEnd - yStart;

        float[,] heights = terrainData.GetHeights(xStart, yStart, width, height);
        TreeInstance[] treeInstances = terrainData.treeInstances;

        WriteImpact(position, areaSize);

        Vector3 center = new Vector3(x, heights[y-yStart,x-xStart], y);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 point = new Vector3(xStart + j, heights[i, j], yStart + i);
                
                float distance = Vector3.Distance(point, center);
               
                float falloff = Mathf.Clamp01(1 - distance / (areaSize / 2));
                heights[i, j] -= power * falloff * falloff;
                if (heights[i, j] < 0f) heights[i, j] = 0f;               
            }
        }

        terrainData.SetHeights(xStart, yStart, heights);
    }

    //paint brown spots on deformation mask
    void WriteImpact(Vector3 impactPoint, int areaSize)
    {
        Vector2 center = WorldToTerrainTextureSpace(impactPoint);
        Color[] colors = deformationMask.GetPixels();

        int x = Mathf.FloorToInt(center.x);
        int y = Mathf.FloorToInt(center.y);

        int width = areaSize;
        int height = areaSize;
        float distance = areaSize/2;

        for (int i = -width; i < width; i++)
        {
            for (int j = -height; j < height; j++)
            {
                Vector2 point = new Vector2(j + x, i + y);
                if (Vector2.Distance(point, center) < distance)
                {
                    colors[(j + x) + (i + y) * deformationMask.width] = Color.white;
                } 
            }
        }

        deformationMask.SetPixels(colors);
        deformationMask.Apply();

    }

    Vector2 WorldToTerrainTextureSpace(Vector3 worldPosition)
    {
        Vector3 terrainPosition = worldPosition - terrain.transform.position;
        float xNormalized = terrainPosition.x / terrainData.size.x;
        float zNormalized = terrainPosition.z / terrainData.size.z;
        float xTex = xNormalized * deformationMask.width;
        float yTex = zNormalized * deformationMask.height;
        return new Vector2(xTex, yTex);
    }

    //create deformation mask to draw brown spots
    void InitializeDeformationMask()
    {
        deformationMask = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        deformationMask.wrapMode = TextureWrapMode.Repeat;
        deformationMask.filterMode = FilterMode.Bilinear;

        Color[] colors = new Color[deformationMask.width * deformationMask.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }
        deformationMask.SetPixels(colors);
        deformationMask.Apply();
        terrain.materialTemplate.SetTexture("_DeformationMask", deformationMask);
    }


    Vector2 WorldToTerrainSpace(Vector3 worldPosition)
    {
        Vector3 terrainPosition = worldPosition - terrain.transform.position;
        float xNormalized = terrainPosition.x / terrainData.size.x;
        float zNormalized = terrainPosition.z / terrainData.size.z;
        return new Vector2(xNormalized, zNormalized);
    }

    private void OnApplicationQuit()
    {
        terrain.terrainData = originialTerrainData;
    }
}
