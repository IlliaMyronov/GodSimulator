using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hunger : IGoal
{
    private CreatureStats stats;

    [SerializeField] private float hungerMattersThreshold;
    [SerializeField] private GameObject character;
    [SerializeField] private int searchRadius;
    [SerializeField] private Resource food;
    private PathFinder pathFinder;
    private WorldManager worldManager;

    private void Awake()
    {
        stats = character.GetComponent<CreatureStats>();

        worldManager = FindObjectOfType<WorldManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }
    public override int Evaluate()
    {
        int importance = EvaluationFormula();

        return importance;
    }

    public override List<Action> GetOrders()
    {
        Inventory inv = character.GetComponent<Inventory>();
        List<Action> orders = new List<Action>();

        if (inv != null)
        {
            if (inv.ResourceAmount(food) > 0)
            {
                orders.Add(new Action("eat from inventory", new Stack<Vector2Int>()));
                return orders;
            }
        }

        Vector2Int position = SearchUtility.ConvertPosition(character.transform.position);

        GameObject target = SearchForResource.FindResource2(food, worldManager, position, searchRadius);

        if(target == null)
        {
            return orders;
        }

        Stack<Vector2Int> directions = pathFinder.FindPath(position, SearchUtility.ConvertPosition(target.transform.position));
        orders.Add(new Action("eat from source", directions));

        return orders;
    }

    public override void Execute(string action)
    {
        /*
            existing actions:
                1. eat from inventory
                2. eat from source
         */

        if(action == "eat from inventory")
        {
            float amountEating = Mathf.Min(character.GetComponent<Inventory>().ResourceAmount(food), stats.maxHunger - stats.myHunger);

            if(character.GetComponent<Inventory>().Consume(food, amountEating))
            {
                stats.Feed(amountEating);
            }
        }

        if(action == "eat from source")
        {
            stats.Feed(worldManager.Gather(new Vector2Int(Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt(character.transform.position.y)), food));
        }
    }

    private int EvaluationFormula()
    {
        return (int)Mathf.Max((stats.maxHunger * hungerMattersThreshold - stats.myHunger) * ((stats.maxHunger - stats.myHunger) / stats.maxHunger), 0);
    }
}
