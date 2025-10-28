using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BuildHouse : IGoal
{
    [SerializeField] private GameObject character;
    [SerializeField] private int buildRadius;
    private WorldManager worldManager;
    private PathFinder pathFinder;
    [SerializeField] private GameObject buildingPrefab;

    private void Awake()
    {
        worldManager = FindObjectOfType<WorldManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    public override int Evaluate()
    {
        if(!this.GetComponent<CreatureStats>().HaveHome())
        {
            return 5;
        }

        return 0;
    }

    public override List<Action> GetOrders()
    {
        // find suitable spot
        Vector2Int position = SearchUtility.ConvertPosition(character.transform.position);
        List<List<TileInfo>> map = worldManager.GetMap();

        Vector2Int? buildingSite = SearchUtility.FindNearestTile(position, buildRadius, pos =>
        {
            // ensure tiles are of solide ground
            if (map[pos.y][pos.x].isBuildable && map[pos.y][pos.x + 1].isBuildable &&
                map[pos.y - 1][pos.x].isBuildable && map[pos.y - 1][pos.x + 1].isBuildable)
            {
                // ensure no building exists there
                if (!(map[pos.y][pos.x].isTaken || map[pos.y][pos.x + 1].isTaken ||
                    map[pos.y - 1][pos.x].isTaken || map[pos.y - 1][pos.x + 1].isTaken))
                {
                    return true;
                }
            }

            return false;
        });

        List<Action> orders = new List<Action>();

        if(buildingSite == null)
        {
            return orders;
        }

        Vector2Int buildingSiteExists = buildingSite.Value;

        if (map[buildingSiteExists.y][buildingSiteExists.x].hasFlora)
        {
            orders.Add(new Action("destroy plant", pathFinder.FindPath(position, buildingSiteExists)));
            position = buildingSiteExists;
        }
        if(map[buildingSiteExists.y][buildingSiteExists.x + 1].hasFlora)
        {
            orders.Add(new Action("destroy plant", pathFinder.FindPath(position, new Vector2Int(buildingSiteExists.x + 1, buildingSiteExists.y))));
            position = new Vector2Int(buildingSiteExists.x + 1, buildingSiteExists.y);
        }
        if(map[buildingSiteExists.y - 1][buildingSiteExists.x].hasFlora)
        {
            orders.Add(new Action("destroy plant", pathFinder.FindPath(position, new Vector2Int(buildingSiteExists.x, buildingSiteExists.y - 1))));
            position = new Vector2Int(buildingSiteExists.x, buildingSiteExists.y - 1);
        }
        if(map[buildingSiteExists.y - 1][buildingSiteExists.x + 1].hasFlora)
        {
            orders.Add(new Action("destroy plant", pathFinder.FindPath(position, new Vector2Int(buildingSiteExists.x + 1, buildingSiteExists.y - 1))));
            position = new Vector2Int(buildingSiteExists.x + 1, buildingSiteExists.y - 1);
        }

        orders.Add(new Action("build site", pathFinder.FindPath(position, buildingSiteExists)));

        return orders;
    }

    public override void Execute(string action)
    {
        Vector2Int position = SearchUtility.ConvertPosition(character.transform.position);
        List<List<TileInfo>> map = worldManager.GetMap();

        if (action == "build site" && map[position.y][position.x].isTaken == false)
        {
            GameObject building = Instantiate(buildingPrefab, new Vector3(position.x, position.y, -1), Quaternion.identity) as GameObject;

            map[position.y][position.x].isTaken = true;
            map[position.y][position.x + 1].isTaken = true;
            map[position.y - 1][position.x].isTaken = true;
            map[position.y - 1][position.x + 1].isTaken = true;
            this.GetComponent<CreatureStats>().AssignHouse(building);
        }

        if(action == "destroy plant")
        {
            worldManager.DestroyPlant(position);
        }
    }
}
