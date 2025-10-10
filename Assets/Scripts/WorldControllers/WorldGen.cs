using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/*
public class WorldGen : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    private WorldSettings settings;
    private Vector2Int worldSize;
    private float maxContinentBorder;
    private float borderSmootheness;
    private float beachSize;
    private float shallowWaterSize;

    private List<List<GameObject>> map;
    private List<List<GameObject>> plants;
    private Vector2 startPerlisPos;

    // dictionary for looking for tiles (A* like)
    private Dictionary<Vector2Int, float> testedTiles;

    private void Awake()
    {
        settings = WorldSettingsRuntime.Instance.Settings;

        Debug.Log(settings.worldSize);

        testedTiles = new Dictionary<Vector2Int, float>();
        map = new List<List<GameObject>>();
        plants = new List<List<GameObject>>();
        
        worldSize = settings.worldSize;
        maxContinentBorder = settings.maxContinentBorder;
        borderSmootheness = settings.borderSmootheness;
        beachSize = settings.beachSize;
        shallowWaterSize = settings.shallowWaterSize;
        
    }

    public List<List<GameObject>> GenerateWorld()
    {
        startPerlisPos = new Vector2(Random.Range(0f, 99999f), Random.Range(0f, 99999f));
        for(int i = 0; i < worldSize.y; i++)
        {
            map.Add(new List<GameObject>());

            for(int j = 0; j < worldSize.x; j++)
            {

                float perlinValue = GeneratePerlinValue(startPerlisPos, j, i);

                //check if we are in the middle of drawing (not left/right nor top/bott) of map
                if ((i > maxContinentBorder * perlinValue && worldSize.y - i > maxContinentBorder * perlinValue) &&
                    (j > maxContinentBorder * perlinValue && worldSize.x - j > maxContinentBorder * perlinValue))
                {
                    // worldSize.x - j and worldSize.y - i was used but it resulted in coordinates and array location being mirrored
                    map[i].Add(this.CreateTile("Grass", new Vector3(j, i, 0)));

                }

                else
                {

                    map[i].Add(this.CreateTile("Water", new Vector3(j, i, 0)));

                }                

            }

        }


        this.CreateSand();

        this.CreateShallowWater();
        

        return map;
    }

    private float GeneratePerlinValue(Vector2 startPos, int x, int y)
    {
        return Mathf.PerlinNoise((((float)x / 1000) * borderSmootheness + startPos.x),
                                 (((float)y / 1000) * borderSmootheness + startPos.y));
    }

    // straight distance from block to block is 1, diagonal distance is rounded to 1.4
    
    private bool isTileNear(Vector2Int tileCoordinates, float distance, string searchedTileName)
    {
        // check if any tile is within distance
        if(distance < 1)
        {
            return false;
        }

        for(int i = 0; i < 8; i++)
        {

            float testTileDistance = 0f;

            // set distance to current test tile
            if (i % 2 == 0)
            {
                testTileDistance = 1f;
            }
            else
            {
                testTileDistance = 1.4f;
            }

            // test if tile is reachable
            if (distance >= testTileDistance)
            {
                Vector2Int testTile = new Vector2Int(Mathf.RoundToInt(tileCoordinates.x + Mathf.Cos(i * (Mathf.PI / 4))),
                                                     Mathf.RoundToInt(tileCoordinates.y + Mathf.Sin(i * (Mathf.PI / 4))));

                // check if testTile is outside map
                if ((testTile.x < 0 || testTile.x > worldSize.x - 1) || (testTile.y < 0 || testTile.y > worldSize.y - 1))
                {
                    return false;
                }

                else
                {
                    // check if tile is the one we are looking for
                    if (map[testTile.y][testTile.x].name == searchedTileName)
                    {
                        return true;
                    }

                    if (testedTiles.ContainsKey(testTile))
                    {

                        if (testedTiles[testTile] > distance - testTileDistance)
                        {
                            // test tile and rewrite shortest distance to a tile distance

                            testedTiles[testTile] = distance - testTileDistance;

                            if (isTileNear(testTile, distance - testTileDistance, searchedTileName))
                            {
                                return true;
                            }
                        }
                    }

                    else
                    {
                        // test the tile and add it to tested tiles list

                        testedTiles.Add(testTile, distance - testTileDistance);

                        if (isTileNear(testTile, distance - testTileDistance, searchedTileName))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    

    private GameObject CreateTile(string tileName, Vector3 pos)
    {
        GameObject tile = Instantiate(tileManager.GetTile(tileName)) as GameObject;

        tile.transform.position = pos;

        tile.name = tileName;

        return tile;
    }

    private void CreateSand()
    {

        for (int i = 0; i < map.Count; i++)
        {

            for (int j = 0; j < map[i].Count; j++)
            {

                if (map[i][j].name == "Grass")
                {
                    testedTiles.Clear();

                    if (isTileNear(new Vector2Int(j, i), beachSize, "Water"))
                    {
                        Destroy(map[i][j]);

                        map[i][j] = this.CreateTile("Sand", new Vector3(j, i, 0));

                    }
                }
            }
        }

    }

    private void CreateShallowWater()
    {
        for (int i = 0; i < map.Count; i++)
        {

            for (int j = 0; j < map[i].Count; j++)
            {

                if (map[i][j].name == "Water")
                {
                    testedTiles.Clear();

                    if (isTileNear(new Vector2Int(j, i), shallowWaterSize, "Sand"))
                    {

                        Destroy(map[i][j]);

                        map[i][j] = this.CreateTile("ShallowWater", new Vector3(j, i, 0));

                    }
                }
            }
        }
    }

    // Generates Plants the world starts with, so far it starts with no plants
    public List<List<GameObject>> GeneratePlants()
    {
        for(int i = 0; i < map.Count; i++)
        {
            plants.Add(new List<GameObject>());

            for(int j = 0; j < map[i].Count; j++)
            {
                plants[i].Add(null);
            }
        }

        return plants;
    }
}
*/