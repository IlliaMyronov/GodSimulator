using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Reproduce : IGoal
{
    private PathFinder pathFinder;
    [SerializeField] private GameObject creaturePrefab;

    private void Awake()
    {
        pathFinder = FindObjectOfType<PathFinder>();
    }

    public override int Evaluate()
    {
        if(this.GetComponent<CreatureStats>().CanReproduce())
        {
            return 10;
        }

        return 0;
    }

    public override List<Action> GetOrders()
    {
        List<Action> orders = new List<Action>();

        orders.Add(new Action("reproduce", pathFinder.FindPath(SearchUtility.ConvertPosition(this.transform.position),
                                           SearchUtility.ConvertPosition(this.GetComponent<CreatureStats>().GetHouse().transform.position))));

        return orders;
    }

    public override void Execute(string action)
    {
        if(action == "reproduce")
        {
            GameObject newChar = Instantiate(creaturePrefab, this.transform.position, Quaternion.identity) as GameObject;

            this.GetComponent<CreatureStats>().Reproduce(newChar);
        }
    }
}
