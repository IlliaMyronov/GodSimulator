using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wonder : IGoal
{
    private PathFinder pathFinder;
    [SerializeField] private GameObject character;
    [SerializeField] private int wonderRange;
    private WorldManager worldManager;

    private void Awake()
    {
        worldManager = FindObjectOfType<WorldManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    public override int Evaluate()
    {
        return 1;
    }

    public override List<Action> GetOrders()
    {
        // pick a random tile nearby
        Vector2Int position = SearchUtility.ConvertPosition(this.transform.position);
        Vector2Int target = new Vector2Int(position.x + Random.Range(-1 * wonderRange, wonderRange), position.y + Random.Range(-1 * wonderRange, wonderRange));

        // avoid wondering into weird places
        int attempts = 0;
        while (!worldManager.GetMap()[target.y][target.x].isBuildable)
        {
            target = new Vector2Int(position.x + Random.Range(-1 * wonderRange, wonderRange), position.y + Random.Range(-1 * wonderRange, wonderRange));
            attempts++;
            if(attempts > 5)
            {
                List<Action> emptyOrders = new List<Action>();
                emptyOrders.Add(new Action("nothing", new Stack<Vector2Int>()));
                return new List<Action>();
            }
        }
        
        
        while (!worldManager.GetMap()[target.y][target.x].isBuildable)
        {
            target = new Vector2Int(position.x + Random.Range(-1 * wonderRange, wonderRange), position.y + Random.Range(-1 * wonderRange, wonderRange));
        }
        
        Stack<Vector2Int> directions = pathFinder.FindPath(position, target);

        List<Action> orders = new List<Action>();
        orders.Add(new Action("nothing", directions));
        // change speed, wandering is slow
        character.GetComponent<CreatureController>().SetSpeed(character.GetComponent<CreatureController>().GetSpeed() / 2);

        return orders;
    }

    public override void Execute(string action)
    {
        character.GetComponent<CreatureController>().SetSpeed(character.GetComponent<CreatureController>().GetSpeed() * 2);
        return;
    }
}
