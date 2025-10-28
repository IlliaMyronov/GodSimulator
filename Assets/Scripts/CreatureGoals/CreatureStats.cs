using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats : MonoBehaviour
{
    public float myHunger { get; private set; }
    [SerializeField] public float maxHealth { get; private set; }
    [SerializeField] public float maxHunger;
    [SerializeField] private float hungerRate;
    [SerializeField] private int childLimit;
    [SerializeField] private int expectedLifeSpan;
    [SerializeField] private float lifeSpamVariance;
    [SerializeField] private int adultAge;

    private float myHealth;
    private bool ownHouse;
    private GameObject myHouse;
    private List<GameObject> myChildren;
    private int lifeSpam;
    private float age;
    private float lastFixedTime;
    private bool isAdult;

    
    private void Awake()
    {
        myHealth = maxHealth;
        myHunger = maxHunger;
        ownHouse = false;
        myHouse = null;
        myChildren = new List<GameObject>();
        age = 0;
        lastFixedTime = Time.time;
        isAdult = false;

        this.transform.localScale = this.transform.localScale / 2;

        lifeSpam = Mathf.RoundToInt(NormalDistribution.GetRandom(expectedLifeSpan, lifeSpamVariance));
        Debug.Log("my lifespan is " + lifeSpam);
    }

    private void FixedUpdate()
    {
        float delta = Time.time - lastFixedTime;

        age += delta;

        if (myHunger > 0)
        {
            myHunger -= Time.deltaTime * hungerRate;
        }
        else
        {
            myHunger = 0;
            myHealth -= Time.deltaTime * 20;
        }

        if(myHealth < 0)
        {
            Debug.Log("I died of starvation ;(");
            Destroy(myHouse); myHouse = null;
            Destroy(gameObject);
        }

        if(!isAdult && age >= adultAge)
        {
            isAdult = true;
            this.transform.localScale *= 2;
            this.GetComponent<CreatureController>().NowAdult();
        }

        if(age > lifeSpam)
        {
            Debug.Log("I died of age :)");
            Destroy(myHouse); myHouse = null;
            Destroy(gameObject);
        }

        lastFixedTime = Time.time;
    }

    public void Feed(float amount)
    {
        myHunger += amount;
        if(myHunger > maxHunger)
        {
            myHunger = maxHunger;
        }
    }

    public bool HaveHome()
    {
        return ownHouse;
    }

    public void AssignHouse(GameObject newHome)
    {
        ownHouse = true;
        myHouse = newHome;
    }

    public GameObject GetHouse()
    {
        if(HaveHome())
        {
            return myHouse;
        }

        return null;
    }

    public bool CanReproduce()
    {
        if (ownHouse && myChildren.Count < childLimit)
        {
            if(myHouse.GetComponent<Building>().CurrentStage() > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void Reproduce(GameObject myChild)
    {
        myChildren.Add(myChild);
    }

    public bool IsAdult()
    {
        return isAdult;
    }
}
