using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StashFood : IGoal
{
    [SerializeField] private GameObject character;
    [SerializeField] private Resource food;
    [SerializeField] private int searchRadius;
    private PathFinder pathFinder;
    private WorldManager worldManager;

    private void Awake()
    {
        worldManager = FindObjectOfType<WorldManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    public override int Evaluate()
    {
        return Mathf.RoundToInt(Mathf.Max(character.GetComponent<Inventory>().desiredNutritionStored - character.GetComponent<Inventory>().ResourceAmount(food), 0));
    }

    public override List<Action> GetOrders()
    {
        List<Action> orders = new List<Action>();
        Vector2Int position = ConvertPosition(character.transform.position);

        GameObject target = SearchForResource.FindResource2(food, worldManager, position, searchRadius);

        if(target == null)
        {
            return orders;
        }

        Stack<Vector2Int> directions = pathFinder.FindPath(position, ConvertPosition(target.transform.position));

        orders.Add(new Action("gather", directions));

        return orders;
    }

    public override void Execute(string action)
    {
        /*
         * possible orders:
         * 1. gather
         */

        if (action == "gather")
        {
            float amountGathered;
            amountGathered = worldManager.Gather(ConvertPosition(character.transform.position), food);
            character.GetComponent<Inventory>().AddItem(food, amountGathered);
        }
    }

    private Vector2Int ConvertPosition(Vector3 toTransform)
    {
        return new Vector2Int(Mathf.RoundToInt(toTransform.x), Mathf.RoundToInt(toTransform.y));
    }
}
