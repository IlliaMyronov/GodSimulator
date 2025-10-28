using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Resource", menuName = "Game/Resource")]
public class Resource : ScriptableObject
{
    public string resourceName;
    public Sprite icon;
}
