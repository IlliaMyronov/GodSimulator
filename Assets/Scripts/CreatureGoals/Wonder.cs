using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wonder : IGoal
{
    [SerializeField] private PathFinder pathFinder;
    [SerializeField] private GameObject character;
    [SerializeField] private int wonderRange;

    public override int Evaluate()
    {
        return 1;
    }

    public override List<Action> GetOrders()
    {
        // pick a random tile nearby
        Vector2Int position = new Vector2Int((int)character.transform.position.x, (int)character.transform.position.y);
        Vector2Int target = new Vector2Int(position.x + Random.Range(-1 * wonderRange, wonderRange), position.y + Random.Range(-1 * wonderRange, wonderRange));
        Stack<Vector2Int> directions = pathFinder.FindPath(position, target);

        List<Action> orders = new List<Action>();
        orders.Add(new Action("nothing", directions));

        return orders;
    }

    public override void Execute(string action)
    {
        return;
    }
}
