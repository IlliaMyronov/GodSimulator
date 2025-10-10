using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGen : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float sandQuantity;
    [SerializeField] private float shallowWaterQuantity;

    private List<List<TileInfo>> world;
    private WorldSettings settings;
    private Vector2 startPerlisPos;

    private void Awake()
    {
        world = new List<List<TileInfo>>();
        settings = WorldSettingsRuntime.Instance.Settings;
        startPerlisPos = new Vector2(Random.Range(0f, 99999f), Random.Range(0f, 99999f));

        //scale sand and shallow water size according to sea level

        sandQuantity *= settings.maxContinentBorder / 50;
        shallowWaterQuantity *= settings.maxContinentBorder / 50;
    }

    public List<List<TileInfo>> GenerateWorld()
    {
        for(int i = 0; i < settings.worldSize.y; i++)
        {
            world.Add(new List<TileInfo>());

            for(int j = 0; j < settings.worldSize.x; j++)
            {
                float perlinValue = GeneratePerlinValue(startPerlisPos, j, i);


                // checks if land tile or water tile should be placed, considers continent border which is essential water level

                float isLandValue = settings.maxContinentBorder * perlinValue;

                //float distanceToLandEdge = Mathf.Min(
                //                                    (i - isLandValue),                 // top edge
                //                                    (settings.worldSize.y - i - isLandValue), // bottom edge
                //                                    (j - isLandValue),                 // left edge
                //                                    (settings.worldSize.x - j - isLandValue));  // right edge

                Vector4 distanceToLandEdge = new Vector4((i - isLandValue), (settings.worldSize.y - i - isLandValue),
                                                         (j - isLandValue), (settings.worldSize.x - j - isLandValue));

                float leastDistance = Mathf.Min(distanceToLandEdge.w, distanceToLandEdge.y, distanceToLandEdge.x, distanceToLandEdge.z);

                if (leastDistance > 0)
                {
                    if((leastDistance < sandQuantity))
                    {
                        AddTile(j, i, "Sand");
                    }
                    else
                    {
                        AddTile(j, i, "Grass");
                    }
                }

                else
                {
                    if ((Mathf.Abs(leastDistance) < shallowWaterQuantity))
                    {
                        AddTile(j, i, "ShallowWater");
                    }
                    else
                    {
                        AddTile(j, i, "DeepWater");
                    }
                }
            }
        }

        return world;
    }

    private float GeneratePerlinValue(Vector2 startPos, int x, int y)
    {
        return Mathf.PerlinNoise((((float)x / 1000) * settings.borderSmootheness + startPos.x),
                                 (((float)y / 1000) * settings.borderSmootheness + startPos.y));
    }

    private void AddTile(int x, int y, string name)
    {
        TileInfo toAdd = new TileInfo(tileManager.GetTile(name));
        world[y].Add(toAdd);
        tilemap.SetTile(new Vector3Int(x, settings.worldSize.y - y, 0), toAdd.GetTileBase());
    }
}
