using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings", menuName = "Settings/WorldSettings")]
public class WorldSettings : ScriptableObject
{
    public Vector2Int worldSize = new Vector2Int(100, 100);
    public float maxContinentBorder = 30f;
    public float borderSmootheness = 100f;
    public float beachSize = 1.4f;
    public float shallowWaterSize = 2f;
}