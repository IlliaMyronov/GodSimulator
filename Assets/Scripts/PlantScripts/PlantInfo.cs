using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInfo : MonoBehaviour
{
    // list that stores what resources can be gathered from here, example: oak, only wood, berry bushes: only food, cherry tree: food and wood
    [SerializeField] private List<string> resources;

    // list to store corresponding stage to a resource, let's say wood can always be gathered, but cherries only when they grow
    [SerializeField] private List<int> stageRequirement;

    [SerializeField] private PlantGrowth plantGrowth;

    public bool CanGather(string toGather)
    {
        int counter;
        for(counter = 0; counter < resources.Count; counter++)
        {
            if (resources[counter] == toGather)
            {
                if (plantGrowth.GetGrowthStage() >= stageRequirement[counter])
                {
                    return true;
                }
                else
                    break;
            }
        }

        return false;
    }
}
