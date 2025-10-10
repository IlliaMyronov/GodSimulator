using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public float desiredNutritionStored;
    public float nutritionStored;

    private void Awake()
    {
        nutritionStored = 0;
    }
}
