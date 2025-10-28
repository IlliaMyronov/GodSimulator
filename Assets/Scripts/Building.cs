using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private List<BuildingInfo> buildingStages;
    [SerializeField] private Vector2Int buildingSize;

    private int resourcesStored;
    private int buildingStage;

    private void Awake()
    {
        buildingStage = 0;
        resourcesStored = 0;

        UpdateSprite();
    }

    public void AddResource(int quntity)
    {
        resourcesStored += quntity;
    }

    // need method to check if any future updates are possible
    // need method to return what resource is needed for an upgrade
    // need method to return how much of a resource is needed for an upgrade
    
    public bool CanUpgrade()
    {
        if(buildingStage + 1 < buildingStages.Count)
        {
            return true;
        }

        return false;
    }

    public int CurrentStage() { return buildingStage; }

    public Resource UpgradeResource()
    {
        if(CanUpgrade())
        {
            return buildingStages[buildingStage + 1].resource;
        }

        return null;
    }

    public int UpgradeResourceQuantity()
    {
        if(CanUpgrade())
        {
            return Mathf.Max(buildingStages[buildingStage + 1].cost - resourcesStored, 0);
        }

        return -1;
    }

    public bool Upgrade()
    {
        if(CanUpgrade())
        {
            if(UpgradeResourceQuantity() == 0)
            {
                buildingStage++;
                resourcesStored = 0;

                UpdateSprite();

                return true;
            }
        }

        return false;
    }

    private void UpdateSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = buildingStages[buildingStage].sprite;
    }
}

[System.Serializable]
public class BuildingInfo
{
    public Sprite sprite;
    public Resource resource;
    public int cost;

    public BuildingInfo(Resource myName, int myCost, Sprite mySprite)
    {
        resource = myName;
        cost = myCost;
        sprite = mySprite;
    }
}
