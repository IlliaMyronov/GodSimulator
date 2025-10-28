using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    [SerializeField] List<Sprite> growthSteps;
    [SerializeField] List<float> stepTime;

    private int currentStep;
    private float timeSinceGrown;

    private void Awake()
    {
        currentStep = 0;
        timeSinceGrown = 0;

        if(growthSteps != null)
        {
            if (growthSteps.Count > 0)
            {
                GetComponent<SpriteRenderer>().sprite = growthSteps[0];
            }
        }
        
        // randomize growth time
        for(int i = 0; i < stepTime.Count; i++)
        {
            stepTime[i] = Random.Range(stepTime[i] / 2, stepTime[i]);
        }
    }

    private void Update()
    {
        if (currentStep < stepTime.Count)
        {
            timeSinceGrown += Time.deltaTime;
            if (timeSinceGrown > stepTime[currentStep])
            {
                timeSinceGrown = 0;
                currentStep++;

                GetComponent<SpriteRenderer>().sprite = growthSteps[currentStep];
            }
        }
    }

    public int GetGrowthStage()
    {
        return currentStep;
    }

    public void ResetGrowth()
    {
        currentStep = 0;
        timeSinceGrown = 0;
        GetComponent<SpriteRenderer>().sprite = growthSteps[0];
    }
}
