using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats : MonoBehaviour
{
    public float myHunger { get; private set; }
    [SerializeField] public float maxHealth { get; private set; }
    [SerializeField] public float maxHunger;
    [SerializeField] private float hungerRate;
    
    private void Awake()
    {
        myHunger = maxHunger;
    }

    private void Update()
    {
        
        if(myHunger > 0)
        {
            myHunger -= Time.deltaTime * hungerRate;
        }
    }

    public void Feed(float amount)
    {
        myHunger += amount;
        if(myHunger > maxHunger)
        {
            myHunger = maxHunger;
        }
    }
}
