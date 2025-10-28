using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FloraSpawn : MonoBehaviour
{
    private int growthIndex;

    //x and y correspond to position, and z is a timer for when to check if we can grow something
    private List<FertileTile> fertileCandidates;

    //reference to world manager to access fertile tiles
    [SerializeField] WorldManager worldManager;

    //variables to update growth index
    [SerializeField] private float tickRate;
    private float timer;
    private int globalTimer;

    //variable to track if we are rebuilding fertile candidates list
    private bool isRebuilding;

    // variable to check if fertile tiles in world manager are being edited
    private bool isEditingTiles;

    private void Awake()
    {
        growthIndex = 0;
        timer = 0f;
        globalTimer = 0;
        isRebuilding = false;
        isEditingTiles = true;

        fertileCandidates = new List<FertileTile>();
    }

    private void Start()
    {
        Rebuild();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tickRate && !isRebuilding && !isEditingTiles)
        {
            timer = 0f;
            tickRefresh();
        }

        if(growthIndex >= fertileCandidates.Count && !isRebuilding && !isEditingTiles)
        {
            growthIndex = 0;
            globalTimer = 0;
            timer = 0f;
            Rebuild();
        }
    }

    private void Rebuild()
    {
        if (isEditingTiles)
            return;

        isRebuilding = true;

        fertileCandidates = RebuildGrowthList();

        isRebuilding = false;
    }

    private void tickRefresh()
    {
        if(growthIndex >= fertileCandidates.Count)
        {
            return;
        }

        if (fertileCandidates.Count == 0)
        {
            return;
        }

        globalTimer++;
        
        Vector2Int tileCoordinates;
        while (globalTimer >= fertileCandidates[growthIndex].GrowthDifficulty)
        {
            tileCoordinates = fertileCandidates[growthIndex].Position;

            if (CanGrow(tileCoordinates))   
            {
                GameObject plant = Instantiate(worldManager.GetTile(tileCoordinates).GetRandomPlant(), new Vector3(tileCoordinates.x, tileCoordinates.y, 0), Quaternion.identity) as GameObject;
                    
                worldManager.AddPlant(tileCoordinates, plant);
                worldManager.GetTile(tileCoordinates).hasFlora = true;
            }

            growthIndex++;

            if(growthIndex >= fertileCandidates.Count)
            {
                return;
            }    
        }   
    }

    private List<FertileTile> RebuildGrowthList()
    {

        isRebuilding = true;

        List<FertileTile> tempList = new List<FertileTile>();
        foreach (var tile in worldManager.GetFertileTiles())
        {
            if (!tile.Value.isTaken)
            {
                FertileTile toAdd = new FertileTile();
                toAdd.Position = tile.Key;
                toAdd.Std = tile.Value.GetGrowthSpeedVariability();
                toAdd.GrowthDifficulty = Mathf.Max(Mathf.RoundToInt(NormalDistribution.GetRandom(tile.Value.GetGrowthDifficulty(), toAdd.Std)), 0);
                
                tempList.Add(toAdd);
            }
        }

        MergeSort(tempList, 0, tempList.Count - 1);
        return tempList;
    }

    // r = size - 1
    private void MergeSort(List<FertileTile> toSort, int l, int r)
    {
        int mid = l + (r - l) / 2;

        if (l >= r)
        {
            return;
        }

        // sort left side
        MergeSort(toSort, l, mid);

        // sort right side
        MergeSort(toSort, mid + 1, r);

        // here we have two sorted lists, one of them (l, (r / 2) - ((r+1) % 2)) and another one is (r / 2 + (r % 2), r)

        int leftPtr = l;
        int rightPtr = mid + 1;
        int index = 0;
        FertileTile[] sortedSublist = new FertileTile[r - l + 1];

        while(leftPtr < mid + 1 && rightPtr <= r)
        {
            // compare minimum of two sorted lists
            if (toSort[rightPtr].GrowthDifficulty < toSort[leftPtr].GrowthDifficulty)
            {
                sortedSublist[index] = toSort[rightPtr];

                index++;
                rightPtr++;
            }

            else
            {
                sortedSublist[index] = toSort[leftPtr];

                index++; leftPtr++;
            }
        }

        //these loops are mutually exclusive
        while(rightPtr <= r)
        {
            sortedSublist[index] = toSort[rightPtr];

            index++; rightPtr++;
        }

        while (leftPtr < mid + 1)
        {
            sortedSublist[index] = toSort[leftPtr];

            index++; leftPtr++;
        }

        for(int i = 0; i < r - l + 1; i++)
        {
            toSort[i + l] = sortedSublist[i];
        }

        return;
    } 
    
    private bool CanGrow(Vector2Int tilePos)
    {
        if(worldManager.GetTile(tilePos).GetGrowthDifficulty() == 0)
        {
            return false;
        }

        return (!HaveNeihbors(tilePos, -1, worldManager.GetTile(tilePos).GetSpaceRequirement()) && !worldManager.GetTile(tilePos).isTaken);
    }

    // returns true if there is an object within distance
    private bool HaveNeihbors(Vector2Int tilePos, int direction, float distanceToCheck)
    {
        float straightDistance = 1f;
        float diagonalDistance = 1.4f;

        // make sure that tilePos is not outside the map
        if((tilePos.x < 0 || tilePos.x >= worldManager.GetMap()[0].Count) ||
           (tilePos.y < 0 || tilePos.y >= worldManager.GetMap().Count))
        {
            return false;
        }

        if(worldManager.GetTile(tilePos).hasFlora)
        {
            return true;
        }

        if (distanceToCheck - straightDistance >= 0)
        {
            // check straight tiles
            // if initial node, check all 4 directions
            // if node came from straight direction, check its straight direction
            // if node came from diagonal direction, don't check its straight neighbors
            if(direction == -1)
            {
                if (HaveNeihbors(CalculateNewPos(tilePos, 0), 0, distanceToCheck - straightDistance)) { return true; }
                if (HaveNeihbors(CalculateNewPos(tilePos, 2), 2, distanceToCheck - straightDistance)) { return true; }
                if (HaveNeihbors(CalculateNewPos(tilePos, 4), 4, distanceToCheck - straightDistance)) { return true; }
                if (HaveNeihbors(CalculateNewPos(tilePos, 6), 6, distanceToCheck - straightDistance)) { return true; }
            }
            else if(direction % 2 == 0)
            {
                if (HaveNeihbors(CalculateNewPos(tilePos, direction), direction, distanceToCheck - straightDistance)) { return true; }
            }

            // check diagonal tiles
            // for initial tile, check all 4 diagonal tiles
            // if came from straight direction check 2 of its diagonals
            // if came from diagonal direction, check its diagonal
            if(distanceToCheck - diagonalDistance >= 0)
            {
                if (direction == -1)
                {
                    if (HaveNeihbors(CalculateNewPos(tilePos, 1), 1, distanceToCheck - diagonalDistance)) { return true; }
                    if (HaveNeihbors(CalculateNewPos(tilePos, 3), 3, distanceToCheck - diagonalDistance)) { return true; }
                    if (HaveNeihbors(CalculateNewPos(tilePos, 5), 5, distanceToCheck - diagonalDistance)) { return true; }
                    if (HaveNeihbors(CalculateNewPos(tilePos, 7), 7, distanceToCheck - diagonalDistance)) { return true; }
                }
                else if (direction % 2 == 0)
                {
                    if (HaveNeihbors(CalculateNewPos(tilePos, direction + 1), direction + 1, distanceToCheck - diagonalDistance)) { return true; }
                    if (HaveNeihbors(CalculateNewPos(tilePos, (direction - 1 + 8) % 8), (direction - 1 + 8) % 8, distanceToCheck - diagonalDistance)) { return true; }
                }
                else
                {
                    if (HaveNeihbors(CalculateNewPos(tilePos, direction), direction, distanceToCheck - diagonalDistance)) { return true; }
                }
            }

            return false;
        }

        else
        {
            return false;
        }
    }

    private Vector2Int CalculateNewPos(Vector2Int oldPos, int dir)
    {
        return new Vector2Int(oldPos.x + (Mathf.RoundToInt(Mathf.Cos(dir * (Mathf.PI / 4)))),
                              oldPos.y + (Mathf.RoundToInt(Mathf.Sin(dir * (Mathf.PI / 4)))));
    }

    public void BeginEdit()
    {
        isEditingTiles = true;
    }

    public void EndEdit()
    {
        isEditingTiles = false;
    }
}

public struct FertileTile
{
    public Vector2Int Position;
    public int GrowthDifficulty;
    public float Std;
}