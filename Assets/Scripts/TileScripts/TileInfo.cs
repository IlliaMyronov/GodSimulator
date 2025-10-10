using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileInfo
{
    [SerializeField] private int growthDifficulty;
    [SerializeField] private int moveDifficulty;
    [SerializeField] private TileBase tile;
    [SerializeField] private List<GameObject> growableObjects;
    [SerializeField] private float emptySpaceForGrowth;

    public bool hasFlora = false;

    public TileInfo(TileInfo toCopy)
    {
        this.growthDifficulty = toCopy.growthDifficulty;
        this.moveDifficulty = toCopy.moveDifficulty;
        this.tile = toCopy.tile;
        this.growableObjects = toCopy.growableObjects;
        this.emptySpaceForGrowth = toCopy.emptySpaceForGrowth;
        this.hasFlora = toCopy.hasFlora;
    }

    public int GetMoveDifficulty()
    {
        return moveDifficulty;
    }

    public bool IsFertile()
    {
        return (growableObjects.Count == 0) ? false : true;
    }

    public int GetGrowthDifficulty()
    {
        return growthDifficulty;
    }

    public GameObject GetRandomPlant()
    {
        return growableObjects[(int)Random.Range(0, growableObjects.Count)];
    }

    public TileBase GetTileBase()
    {
        return tile;
    }

    public float GetSpaceRequirement()
    {
        return emptySpaceForGrowth;
    }
}
