using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private List<TileInfo> allTiles;

    private Dictionary<string, TileInfo> tiles;


    private void Awake()
    {
        tiles = new Dictionary<string, TileInfo>();

        

        for(int i = 0; i < allTiles.Count; i++)
        {
            tiles.Add(allTiles[i].GetTileBase().name, allTiles[i]);
        }

        allTiles.Clear();
    }

    public TileInfo GetTile(string s)
    {
        return tiles.GetValueOrDefault(s);
    }
}
