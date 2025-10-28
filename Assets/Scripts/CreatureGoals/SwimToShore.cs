using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimToShore : IGoal
{
    [SerializeField] private GameObject character;
    private WorldManager worldManager;
    private PathFinder pathFinder;
    [SerializeField] private int searchRadius;

    private void Awake()
    {
        worldManager = FindObjectOfType<WorldManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    public override int Evaluate()
    {
        Vector2Int position = SearchUtility.ConvertPosition(this.transform.position);

        if (!worldManager.GetMap()[position.y][position.x].isBuildable)
        {
            return 100;
        }

        return 0;
    }

    public override List<Action> GetOrders()
    {
        Vector2Int? target = SearchUtility.FindNearestTile(SearchUtility.ConvertPosition(this.transform.position), searchRadius, pos =>
        {
            if(!(pos.x < 0 || pos.y < 0 ||
                 pos.x > worldManager.GetMap()[0].Count || pos.y > worldManager.GetMap().Count))
            {
                TileInfo tile = worldManager.GetTile(pos);
                return tile.isBuildable;
            }

            return false;
        });

        if(target == null)
        {
            return new List<Action>();
        }

        List<Action> orders = new List<Action>();
        orders.Add(new Action("nothing", pathFinder.FindPath(SearchUtility.ConvertPosition(this.transform.position), target.Value)));

        return orders;
    }

    public override void Execute(string action)
    {
        if(action == "nothing")
        {
            return;
        }
    }
}
