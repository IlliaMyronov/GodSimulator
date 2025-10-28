using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileInfo
{
    [SerializeField] private int growthDifficulty;
    [SerializeField] private float growthSpeedVariability;
    [SerializeField] private int moveDifficulty;
    [SerializeField] private TileBase tile;
    [SerializeField] private List<GameObject> growableObjects;
    [SerializeField] private float emptySpaceForGrowth;
    [SerializeField] public bool isBuildable;

    public bool hasFlora = false;
    public bool isTaken = false;

    public TileInfo(TileInfo toCopy)
    {
        this.growthDifficulty = toCopy.growthDifficulty;
        this.moveDifficulty = toCopy.moveDifficulty;
        this.tile = toCopy.tile;
        this.growableObjects = toCopy.growableObjects;
        this.emptySpaceForGrowth = toCopy.emptySpaceForGrowth;
        this.hasFlora = toCopy.hasFlora;
        this.isBuildable = toCopy.isBuildable;
        this.growthSpeedVariability = toCopy.growthSpeedVariability;
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

    public float GetGrowthSpeedVariability()
    {
        return growthSpeedVariability;
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
