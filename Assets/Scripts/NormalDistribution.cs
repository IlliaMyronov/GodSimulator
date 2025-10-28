using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDistribution
{
    private static bool hasAvaliable = false;
    private static float avaliable = 0;

    public static float GetRandom(float mean, float std)
    {
        return GetZValue() * std + mean;
    }

    // returns random Z value from normal distribution
    private static float GetZValue()
    {
        if(hasAvaliable)
        {
            hasAvaliable = false;
            return avaliable;
        }

        float u1 = UnityEngine.Random.value;
        float u2 = UnityEngine.Random.value;

        avaliable = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
        hasAvaliable = true;
        return Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
    }
}
