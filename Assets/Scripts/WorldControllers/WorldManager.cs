using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private TileMapGen worldGen;
    [SerializeField] private FloraSpawn floraSpawn;

    private List<List<TileInfo>> map;
    private Dictionary<Vector2Int, GameObject> plants;
    private Dictionary<Vector2Int, TileInfo> fertileTiles;

    private void Awake()
    {
        fertileTiles = new Dictionary<Vector2Int, TileInfo>();
        plants = new Dictionary<Vector2Int, GameObject>();
    }

    private void Start()
    {
        map = worldGen.GenerateWorld();

        floraSpawn.BeginEdit();
        this.PopulateFertileTiles();
        floraSpawn.EndEdit();
    }

    
    private void PopulateFertileTiles()
    {
        for(int i = 0; i < map.Count; i++)
        {
            for(int j = 0; j < map[i].Count; j++)
            {
                if(map[i][j].IsFertile())
                {
                    fertileTiles.Add(new Vector2Int(j, i), map[i][j]);
                }
            }
        }

        Debug.Log("num of fertile tiles " + fertileTiles.Count);
    }
    
    public Dictionary<Vector2Int, TileInfo> GetFertileTiles()
    {
        return fertileTiles;
    }

    public void AddPlant(Vector2Int coordinates, GameObject plant)
    {
        plants.Add(coordinates, plant);
    }

    public GameObject GetPlant(Vector2Int coordinates)
    {
        return plants[coordinates];
    }

    public List<List<TileInfo>> GetMap()
    {
        return map;
    }

    public TileInfo GetTile(Vector2Int coordinates)
    {
        return map[coordinates.y][coordinates.x];
    }

    public bool Gather(Vector2Int pos)
    {
        // pos is given in real coordinates, not array one
        pos = new Vector2Int(pos.x, map.Count - pos.y);

        if (plants.ContainsKey(pos))
        {
            Destroy(plants[pos]);
            map[pos.y][pos.x].hasFlora = false;
            return true;
        }

        return false;
    }
}
