using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInfo : MonoBehaviour
{
    // list that stores what resources can be gathered from here, example: oak, only wood, berry bushes: only food, cherry tree: food and wood
    [SerializeField] private List<DropResourceInfo> resources;

    [SerializeField] private PlantGrowth plantGrowth;

    public bool CanGather(Resource toGather)
    {
        int counter;
        for(counter = 0; counter < resources.Count; counter++)
        {
            if (resources[counter].resource == toGather)
            {
                if (plantGrowth.GetGrowthStage() >= resources[counter].stageRequirment)
                {
                    return true;
                }
                else
                    break;
            }
        }

        return false;
    }

    public bool isBeingDestroyed(Resource resource)
    {
        for(int i = 0; i < resources.Count; i++)
        {
            if (resources[i].resource == resource)
            {
                return resources[i].isDestroyedOnGather;
            }
        }

        return false;
    }

    public int Gather(Resource resource)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].resource == resource)
            {
                if(!resources[i].isDestroyedOnGather)
                {
                    plantGrowth.ResetGrowth();
                }

                return resources[i].amount;
            }
        }

        return 0;
    }

    public int ResourceAmount(Resource resource)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].resource == resource)
            {
                return resources[i].amount;
            }
        }

        return 0;
    }
}

[System.Serializable]
public class DropResourceInfo
{
    public Resource resource;
    public int stageRequirment;
    public int amount;
    public bool isDestroyedOnGather;
}