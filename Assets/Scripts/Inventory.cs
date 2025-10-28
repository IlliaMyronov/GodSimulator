using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{ 
    [SerializeField] public float desiredNutritionStored;
    public Dictionary<Resource, float> inventory;

    private void Awake()
    {
        inventory = new Dictionary<Resource, float>();
    }
    public void AddItem(Resource type, float quantity)
    {
        if(inventory.ContainsKey(type))
        {
            inventory[type] += quantity;
            return;
        }

        inventory.Add(type, quantity);
        return;
    }

    public float ResourceAmount(Resource type)
    {
        if(inventory.ContainsKey(type))
        {
            return inventory[type];
        }
        return 0;
    }

    public bool HaveResource(Resource type)
    {
        if(type == null)
            return false;

        return inventory.ContainsKey(type);
    }

    public bool Consume(Resource type, float quantity)
    {
        if(inventory.ContainsKey(type))
        {
            if (inventory[type] >= quantity)
            {
                inventory[type] -= quantity;

                if (inventory[type] <= 0)
                {
                    inventory.Remove(type);
                }

                return true;
            }
        }
        return false;
    }
}