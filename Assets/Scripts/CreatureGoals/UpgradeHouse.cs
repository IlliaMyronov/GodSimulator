using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UpgradeHouse : IGoal
{
    [SerializeField] private int searchRadius;
    private WorldManager worldManager;
    private PathFinder pathFinder;

    private void Awake()
    {
        worldManager = FindObjectOfType<WorldManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    public override int Evaluate()
    {
        if(this.GetComponent<CreatureStats>().HaveHome())
        {
            GameObject house = this.GetComponent<CreatureStats>().GetHouse();

            if (house.GetComponent<Building>().CanUpgrade())
            {
                return 5;
            }
        }

        return 0;
    }

    public override List<Action> GetOrders()
    {
        List<Action> orders = new List<Action>();

        GameObject house = new GameObject();

        if (this.GetComponent<CreatureStats>().HaveHome())
        {
            house = this.GetComponent<CreatureStats>().GetHouse();
        }
        else
        {
            return orders;
        }

        int numberOfResourcesNeeded = house.GetComponent<Building>().UpgradeResourceQuantity();
        Resource requirement = house.GetComponent<Building>().UpgradeResource();

        // check if I already have any of materials used for upgrade
        if (this.GetComponent<Inventory>().HaveResource(house.GetComponent<Building>().UpgradeResource()))
        {
            // store these resources in the house
            house.GetComponent<Building>().AddResource(Mathf.Min(Mathf.FloorToInt(this.GetComponent<Inventory>().ResourceAmount(requirement)), numberOfResourcesNeeded));

            // remove consumed resources from inventory
            this.GetComponent<Inventory>().Consume(requirement, numberOfResourcesNeeded - house.GetComponent<Building>().UpgradeResourceQuantity());

            // update how many resources are needed
            numberOfResourcesNeeded = house.GetComponent<Building>().UpgradeResourceQuantity();
        }

        Vector2Int position = SearchUtility.ConvertPosition(this.transform.position);

        if(numberOfResourcesNeeded > 0)
        {
            GameObject target = SearchForResource.FindResource2(requirement, worldManager, position, searchRadius);

            if(target == null)
            {
                return orders;
            }

            orders.Add(new Action("gather", pathFinder.FindPath(position, SearchUtility.ConvertPosition(target.transform.position))));
            position = SearchUtility.ConvertPosition(target.transform.position);

            numberOfResourcesNeeded -= worldManager.GetPlant(position).GetComponent<PlantInfo>().ResourceAmount(requirement);
        }

        Stack<Vector2Int> directions = pathFinder.FindPath(position, SearchUtility.ConvertPosition(house.transform.position));

        orders.Add(new Action("upgrade", directions));
 
        return orders;
    }

    public override void Execute(string action)
    {
        Vector2Int position = SearchUtility.ConvertPosition(this.transform.position);

        GameObject house = new GameObject();

        if (this.GetComponent<CreatureStats>().HaveHome())
        {
            house = this.GetComponent<CreatureStats>().GetHouse();
        }
        else
        {
            return;
        }

        Resource requirement = house.GetComponent<Building>().UpgradeResource();

        if (action == "gather")
        {
            float amountGathered;
            amountGathered = worldManager.Gather(SearchUtility.ConvertPosition(this.transform.position), requirement);
            this.GetComponent<Inventory>().AddItem(requirement, amountGathered);
        }

        if (action == "upgrade")
        {
            int resourcesUsed = 0;

            resourcesUsed = Mathf.Min(house.GetComponent<Building>().UpgradeResourceQuantity(), Mathf.FloorToInt(this.GetComponent<Inventory>().ResourceAmount(requirement)));

            house.GetComponent<Building>().AddResource(resourcesUsed);
            this.GetComponent<Inventory>().Consume(requirement, resourcesUsed);

            house.GetComponent<Building>().Upgrade();
        }
    }
}
